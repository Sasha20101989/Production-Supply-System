using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Models;

using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Extensions;
using DAL.Models;
using DAL.Models.BOM;
using DAL.Models.Document;
using DAL.Models.Inbound;
using DAL.Models.Master;
using DAL.Models.Partscontrol;
using DAL.Models.Planning;
using DAL.Parameters.Customs;
using DAL.Parameters.Inbound;
using DAL.Parameters.Planning;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BLL.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ILogger<DeliveryService> _logger;
        private readonly IUserService _userService;
        private readonly IProcessService _processService;
        private readonly IStaticDataService _staticDataService;
        private readonly IExcelService _excelService;
        private readonly IBOMService _bomService;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Lot> _lotRepository;
        private readonly IRepository<ContainersInLot> _containerRepository;
        private readonly IRepository<PartsInContainer> _partInContainerRepository;
        private readonly IRepository<PartsInInvoice> _partInInvoiceRepository;
        private readonly IRepository<CustomsPart> _customsPartRepository;
        private readonly IRepository<Case> _caseRepository;
        private readonly IRepository<BodyModelVariant> _bodyModelVariantRepository;
        private readonly IRepository<VinsInContainer> _vinContainerRepository;
        private readonly IRepository<CustomsClearance> _customsClearanceRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<Tracing> _tracingRepository;
        private List<ProcessStep> _stepCollection;
        private List<VinsInContainer> _vinContainers;
        private List<CustomsClearance> _customsClearances;
        private List<ContainersInLot> _containers;
        private List<Case> _cases;
        private List<CustomsPart> _customsParts;
        private List<PartsInContainer> _partsInContainer;

        public DeliveryService(
            IUserService userService,
            IRepository<Invoice> invoiceRepository,
            IProcessService processService,
            IExcelService excelService,
            IRepository<Lot> lotRepository,
            IRepository<ContainersInLot> containerRepository,
            IRepository<PartsInContainer> partInContainerRepository,
            IRepository<PartsInInvoice> partInInvoiceRepository,
            IRepository<CustomsPart> customsPartRepository,
            IRepository<Case> caseRepository,
            IRepository<BodyModelVariant> bodyModelVariantRepository,
            IRepository<VinsInContainer> vinContainerRepository,
            IRepository<CustomsClearance> customsClearanceRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IRepository<Tracing> tracingRepository,
            IStaticDataService staticDataService,
            IBOMService bomService,
            ILogger<DeliveryService> logger)
        {
            _userService = userService;
            _invoiceRepository = invoiceRepository;
            _processService = processService;
            _excelService = excelService;
            _lotRepository = lotRepository;
            _containerRepository = containerRepository;
            _partInContainerRepository = partInContainerRepository;
            _partInInvoiceRepository = partInInvoiceRepository;
            _customsPartRepository = customsPartRepository;
            _caseRepository = caseRepository;
            _bodyModelVariantRepository = bodyModelVariantRepository;
            _vinContainerRepository = vinContainerRepository;
            _customsClearanceRepository = customsClearanceRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _tracingRepository = tracingRepository;
            _staticDataService = staticDataService;
            _bomService = bomService;
            _logger = logger;
        }

        public EventHandler<List<ProcessStep>> NavigatedWithStepCollection { get; set; }

        private static bool IsValidVinNumber(string vin)
        {
            string vinPattern = @"^[A-HJ-NPR-Z0-9]{17}$";

            return Regex.IsMatch(vin, vinPattern);
        }

        private static CustomsClearance CreateCustomsClearance(ContainersInLot container, PartsInContainer partInContainer, Lot lot)
        {
            CustomsClearance customsClearance = new()
            {
                ContainersInLot = container,
                PartType = partInContainer.PartNumber.PartType,
                InvoceNumber = CreateCustomsClearanceNumber(lot, partInContainer)
            };

            return customsClearance.InvoceNumber is null
                ? throw new Exception($"Ошибка формирования номера инвойса для таможенной процедуры.")
                : customsClearance;
        }

        private static ContainersInLot CreateContainerWithType(Docmapper document, int row)
        {
            string containerNumber = GraftValueFromRow(document, row, typeof(ContainersInLot), nameof(ContainersInLot.ContainerNumber))?.ToString();
            string containerType = GraftValueFromRow(document, row, typeof(TypesOfContainer), nameof(TypesOfContainer.ContainerType))?.ToString();

            if (!string.IsNullOrWhiteSpace(containerNumber) && !string.IsNullOrWhiteSpace(containerType))
            {
                ContainersInLot container = new()
                {
                    ContainerNumber = containerNumber,

                    ContainerType = new()
                    {
                        ContainerType = containerType
                    }
                };

                return container;
            }

            return null;
        }

        private static PartPrice CreatePartPrice(Docmapper document, int row)
        {
            PartPrice partPrice = new();

            DocmapperContent contentPartNumber = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(PartPrice).GetSystemColumnName(nameof(PartPrice.PartNumber)));

            if (partPrice.TrySetAndValidateProperty(nameof(PartPrice.PartNumber), GraftValueFromRow(document, row, typeof(PartPrice), nameof(PartPrice.PartNumber)), row + 1, contentPartNumber.ColumnNr, out Dictionary<string, CellInfo> _result))
            {
                DocmapperContent contentPrice = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(PartPrice).GetSystemColumnName(nameof(PartPrice.Price)));

                if (partPrice.TrySetAndValidateProperty(nameof(PartPrice.Price), GraftValueFromRow(document, row, typeof(PartPrice), nameof(PartPrice.Price)), row + 1, contentPrice.ColumnNr, out _result))
                {
                    return partPrice;
                }
            }

            return null;
        }

        private static string CreateCustomsClearanceNumber(Lot lot, PartsInContainer partInContainer)
        {
            string symbolBody = "A";
            string symbolParts = "B";

            string symbol = partInContainer?.PartNumber?.PartType?.PartType == PartTypes.Body ? symbolBody : partInContainer?.PartNumber?.PartType?.PartType == PartTypes.Parts ? symbolParts : null;

            return symbol != null ? $"{lot.LotNumber}-{symbol}" : null;
        }

        private static Dictionary<int, decimal> CalculateTotalQuantities(List<PartsInContainer> partsInContainer)
        {
            Dictionary<int, decimal> totalQuantities = partsInContainer
                .GroupBy(part => part.PartNumberId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(part => part.Quantity)
                );

            return totalQuantities;
        }

        private static VinsInContainer TryCreateVinsInContainer(ContainersInLot container, Lot lot, Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            VinsInContainer vinsInContainer = new()
            {
                ContainerInLot = container,
                Lot = lot
            };

            CustomsPart customsPart = new();

            DocmapperContent contentPartNumber = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(CustomsPart).GetSystemColumnName(nameof(CustomsPart.PartNumber)));

            if (!customsPart.TrySetAndValidateProperty(nameof(CustomsPart.PartNumber), GraftValueFromRow(document, row, typeof(CustomsPart), nameof(CustomsPart.PartNumber)), row + 1, contentPartNumber.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent content = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(VinsInContainer).GetSystemColumnName(nameof(VinsInContainer.SupplierVinNumber)));

            int col = content.ColumnNr;

            if (!vinsInContainer.TrySetAndValidateProperty(nameof(VinsInContainer.SupplierVinNumber), GraftValueFromRow(document, row, typeof(VinsInContainer), nameof(VinsInContainer.SupplierVinNumber)), row + 1, col, out result))
            {
                validationErrors.Merge(result);
            }

            return !string.IsNullOrWhiteSpace(vinsInContainer.SupplierVinNumber) && !IsValidVinNumber(vinsInContainer.SupplierVinNumber)
                ? throw new Exception($"Не валидный VIN номер '{vinsInContainer.SupplierVinNumber}'")
                : vinsInContainer;
        }

        public async Task ExportAllTracing(string exportFilePath)
        {
            _logger.LogInformation($"Start of export file.");

            List<Tracing> tracingInOpenLots = new();

            foreach (Tracing item in await GetAllTracingAsync())
            {
                if (await GetContainerInOpenLot(item.ContainerInLot.ContainerNumber) is not null)
                {
                    item.ContainerInLot.CargoType = await GetContainerCargoType(item.ContainerInLotId);

                    item.TraceLocation = await _staticDataService.GetLocationByIdAsync(item.TraceLocationId);

                    tracingInOpenLots.Add(item);
                }
            }

            IEnumerable<Tracing> containersInTransshipment = tracingInOpenLots.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.TransshipmentPort));
        }

        public async Task<CargoTypes> GetContainerCargoType(int containerInLotId)
        {
            IEnumerable<IGrouping<int, PartsInContainer>> groupingParts = (await GetAllPartsInContainer())
                .Where(p => p.ContainerInLotId == containerInLotId)
                .GroupBy(p => p.ContainerInLotId);

            IGrouping<int, PartsInContainer> containerGroup = groupingParts.FirstOrDefault();

            if (containerGroup != null)
            {
                bool hasBody = containerGroup.Any(p => p.PartNumber.PartType.PartType == PartTypes.Body);

                bool hasParts = containerGroup.Any(p => p.PartNumber.PartType.PartType == PartTypes.Parts);

                return hasBody && hasParts ? CargoTypes.Mix :
                                                hasBody ? CargoTypes.Body :
                                                hasParts ? CargoTypes.Parts : CargoTypes.Unknown;
            }

            return CargoTypes.Unknown;
        }

        public async Task<Lot> StartProcessAsync(List<ProcessStep> steps, Lot lotDetails)
        {
            if (_stepCollection == null)
            {
                _stepCollection = steps.OrderBy(collection => collection.Step).ToList();
            }

            return await UploadLotContentAsync(lotDetails, _stepCollection);
        }

        private async Task<Lot> UploadLotContentAsync(Lot lotDetails, List<ProcessStep> stepCollection)
        {
            Lot lot = new();

            foreach (ProcessStep step in stepCollection.Where(HasAccess).Where(step => step.StepName == Steps.UploadLotContent))
            {
                List<PartPrice> uploadedPartPrices = UploadPartPrices(_stepCollection);

                lot = await CreateLotAndTryCreateInvoiceAsync(lotDetails, step.ValidationErrors, step.Docmapper);

                HandleValidationErrors(step);

                await ProcessContainersInLotAsync(step, lot);

                HandleValidationErrors(step);

                lot.LotInvoice = await SaveInvoiceAsync(lot);

                lot = await SaveLotAsync(lot);

                await SaveContainers(_containers, lot);

                await SaveCases(_cases);

                await SaveCustomsParts(_customsParts);

                await SavePartInContainers(_partsInContainer, lot);

                await SaveVinContainers(_vinContainers, lot);

                await SaveCustomsClearance(_customsClearances);

                await SavePartsInInvoice(await CreateUniquePartsInInvoiceAsync(_partsInContainer, lot, uploadedPartPrices));

                await SaveTracing(await CreateUniqueContainersTracingAsync(lot, _containers));
            }

            return lot;
        }

        private List<PartPrice> UploadPartPrices(List<ProcessStep> stepCollection)
        {
            List<PartPrice> partPrices = new();

            foreach (ProcessStep step in stepCollection.Where(HasAccess).Where(step => step.StepName == Steps.UploadPrice))
            {
                List<IGrouping<string, PartPrice>> groupedPartPrices = Enumerable.Range(step.Docmapper.FirstDataRow, step.Docmapper.Data.GetLength(0) - step.Docmapper.FirstDataRow)
                    .Select(i => CreatePartPrice(step.Docmapper, i))
                    .Where(pp => pp != null)
                    .GroupBy(pp => pp.PartNumber)
                    .ToList();

                foreach (IGrouping<string, PartPrice> group in groupedPartPrices)
                {
                    PartPrice previousPartPrice = null;

                    foreach (PartPrice partPrice in group)
                    {
                        if (previousPartPrice != null && partPrice.Price != previousPartPrice.Price)
                        {
                            throw new Exception($"Номер детали '{group.Key}' имеет разные цены.");
                        }
                        else
                        {
                            if (!partPrices.Any(pp => pp.PartNumber == partPrice.PartNumber))
                            {
                                partPrices.Add(partPrice);
                            }

                            previousPartPrice = partPrice;
                        }
                    }
                }
            }

            return partPrices;
        }

        private List<ContainersInLot> UploadContainerTypes(List<ProcessStep> stepCollection)
        {
            List<ContainersInLot> containers = new();

            foreach (ProcessStep step in stepCollection.Where(HasAccess).Where(step => step.StepName == Steps.UploadContainerTypes))
            {
                containers.AddRange(
                    Enumerable.Range(step.Docmapper.FirstDataRow, step.Docmapper.Data.GetLength(0) - step.Docmapper.FirstDataRow)
                              .Select(i => CreateContainerWithType(step.Docmapper, i))
                              .Where(container => container != null)
                );
            }

            return containers;
        }

        private async Task<Lot> CreateLotAndTryCreateInvoiceAsync(Lot lotDetails, Dictionary<string, CellInfo> validationErrors, Docmapper document)
        {
            Lot lot = new()
            {
                LotPurchaseOrder = lotDetails.LotPurchaseOrder,
                Shipper = lotDetails.Shipper,
                Carrier = lotDetails.Carrier,
                LotTransport = lotDetails.LotTransport,
                DeliveryTerms = lotDetails.DeliveryTerms,
                LotTransportType = lotDetails.LotTransportType,
                LotArrivalLocation = lotDetails.LotArrivalLocation,
                LotDepartureLocation = lotDetails.LotDepartureLocation,
                LotCustomsLocation = lotDetails.LotCustomsLocation,
                LotEta = lotDetails.LotEta,
                LotAta = lotDetails.LotAta,
                LotEtd = lotDetails.LotEtd,
                LotAtd = lotDetails.LotAtd,
                LotTransportDocument = lotDetails.LotTransportDocument,
                LotComment = lotDetails.LotComment
            };

            lot.LotInvoice = await TryCreateInvoiceAsync(DateTime.Now, document, lot, validationErrors);

            return lot;
        }

        private async Task<Invoice> TryCreateInvoiceAsync(DateTime initialDate, Docmapper document, Lot lot, Dictionary<string, CellInfo> validationErrors)
        {
            Invoice invoice = new()
            {
                InvoiceDate = initialDate,
                PurchaseOrder = await GetPurchaseOrderById(lot.LotPurchaseOrderId),
                Shipper = await _staticDataService.GetShipperByIdAsync(lot.ShipperId)
            };

            DocmapperContent content = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Invoice).GetSystemColumnName(nameof(Invoice.InvoiceNumber)));

            int row = (int)content.RowNr;
            int col = content.ColumnNr;

            if (!lot.TrySetAndValidateProperty(nameof(Lot.LotNumber), GraftLotNumber(lot, document), row, col, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            invoice.InvoiceNumber = lot.LotNumber;

            return invoice;
        }

        private async Task ProcessContainersInLotAsync(ProcessStep step, Lot lot)
        {
            _partsInContainer = new();
            _containers = new();
            _cases = new();
            _customsClearances = new();
            _vinContainers = new();
            _customsParts = new();

            List<ContainersInLot> uploadedContainers = UploadContainerTypes(_stepCollection);

            double totalProgress = step.Docmapper.Data.GetLength(0) - step.Docmapper.FirstDataRow;

            double currentProgress = 0.0;

            for (int row = step.Docmapper.FirstDataRow; row < step.Docmapper.Data.GetLength(0); row++)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Проверка {progress} из {totalProgress} строк."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                ContainersInLot container = await TryCreateContainer(uploadedContainers, step.ValidationErrors, step.Docmapper, row);

                _containers.Add(container);

                Case caseItem = await TryCreateCase(step.ValidationErrors, step.Docmapper, row);

                _cases.Add(caseItem);

                VinsInContainer vinsInContainer = TryCreateVinsInContainer(container, lot, step.ValidationErrors, step.Docmapper, row);

                PartsInContainer partInContainer = await TryCreatePartInContainerAsync(vinsInContainer, container, lot, caseItem, step.ValidationErrors, step.Docmapper, row);

                _partsInContainer.Add(partInContainer);

                _customsClearances.Add(CreateCustomsClearance(container, partInContainer, lot));
            }
        }

        private async Task<ContainersInLot> TryCreateContainer(List<ContainersInLot> uploadedContainers, Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            ContainersInLot container = new();

            DocmapperContent contentSealNumber = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(ContainersInLot).GetSystemColumnName(nameof(ContainersInLot.SealNumber)));

            DocmapperContent contentContainerNumber = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(ContainersInLot).GetSystemColumnName(nameof(ContainersInLot.ContainerNumber)));

            if (!container.TrySetAndValidateProperty(nameof(ContainersInLot.SealNumber), GraftValueFromRow(document, row, typeof(ContainersInLot), nameof(ContainersInLot.SealNumber)), row + 1, contentSealNumber.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            if (!container.TrySetAndValidateProperty(nameof(ContainersInLot.ContainerNumber), GraftValueFromRow(document, row, typeof(ContainersInLot), nameof(ContainersInLot.ContainerNumber)), row + 1, contentContainerNumber.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }
            else
            {
                ContainersInLot containerWithType = uploadedContainers.FirstOrDefault(c => c.ContainerNumber == container.ContainerNumber);

                if (containerWithType is null)
                {
                    throw new Exception($"Для контейнера '{container.ContainerNumber}' отсутствует информация о типе контейнера.");
                }

                container.ContainerType = await _staticDataService.GetContainerTypeByName(containerWithType.ContainerType.ContainerType);

                if (container.ContainerType is null)
                {
                    throw new Exception($"Тип контейнера '{containerWithType.ContainerType.ContainerType}' не найден.");
                }
            }

            return container;
        }

        private async Task<Case> TryCreateCase(Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            Case partCase = new()
            {
                PackingType = new()
            };

            DocmapperContent contentCaseNo = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.CaseNo)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.CaseNo), GraftValueFromRow(document, row, typeof(Case), nameof(Case.CaseNo)), row + 1, contentCaseNo.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentLength = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.Length)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.Length), GraftValueFromRow(document, row, typeof(Case), nameof(Case.Length)), row + 1, contentLength.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentWidth = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.Width)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.Width), GraftValueFromRow(document, row, typeof(Case), nameof(Case.Width)), row + 1, contentWidth.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentHeight = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.Height)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.Height), GraftValueFromRow(document, row, typeof(Case), nameof(Case.Height)), row + 1, contentHeight.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentVolume = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.Volume)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.Volume), GraftValueFromRow(document, row, typeof(Case), nameof(Case.Volume)), row + 1, contentVolume.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentNetWeight = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.NetWeight)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.NetWeight), GraftValueFromRow(document, row, typeof(Case), nameof(Case.NetWeight)), row + 1, contentNetWeight.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentGrossWeight = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Case).GetSystemColumnName(nameof(Case.GrossWeight)));

            if (!partCase.TrySetAndValidateProperty(nameof(Case.GrossWeight), GraftValueFromRow(document, row, typeof(Case), nameof(Case.GrossWeight)), row + 1, contentGrossWeight.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentTypesOfPacking = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(TypesOfPacking).GetSystemColumnName(nameof(TypesOfPacking.SupplierPackingType)));

            object packingType = GraftValueFromRow(document, row, typeof(TypesOfPacking), nameof(TypesOfPacking.SupplierPackingType));

            if (!partCase.PackingType.TrySetAndValidateProperty(nameof(TypesOfPacking.SupplierPackingType), packingType, row + 1, contentTypesOfPacking.ColumnNr, out result))
            {
                if (!_cases.Any(c => c.CaseNo == partCase.CaseNo && string.IsNullOrEmpty(packingType?.ToString())))
                {
                    validationErrors.Merge(result);
                }
            }
            else
            {
                partCase.PackingType = await _staticDataService.GetExistingPackingTypeByTypeAsync(partCase.PackingType.SupplierPackingType);

                if (partCase.PackingType is null)
                {
                    throw new Exception($"В базе данных не обнаружен тип упаковки '{packingType}'.");
                }
            }

            return partCase;
        }

        private async Task<PartsInContainer> TryCreatePartInContainerAsync(VinsInContainer vinsInContainer, ContainersInLot container, Lot lot, Case caseItem, Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            PartsInContainer part = new()
            {
                ContainerInLot = container,
                PartInvoice = lot.LotInvoice,
                Case = caseItem,
                PartNumber = new()
            };

            DocmapperContent contentPartsInContainer = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(PartsInContainer).GetSystemColumnName(nameof(PartsInContainer.Quantity)));

            if (!part.TrySetAndValidateProperty(nameof(PartsInContainer.Quantity), GraftValueFromRow(document, row, typeof(PartsInContainer), nameof(PartsInContainer.Quantity)), row + 1, contentPartsInContainer.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            part.PartNumber = await TryCreateCustomsPart(validationErrors, document, row);

            if (string.IsNullOrWhiteSpace(vinsInContainer.SupplierVinNumber))
            {
                part.PartNumber.PartType = await _staticDataService.GetPartTypeByNameAsync(PartTypes.Parts);
            }
            else
            {
                part.PartNumber.PartType = await _staticDataService.GetPartTypeByNameAsync(PartTypes.Body);
            }

            if (part.PartNumber.PartType is null)
            {
                throw new Exception($"Ошибка получения типа для детали.");
            }

            _customsParts.Add(part.PartNumber);

            if (part.PartNumber.PartType.PartType == PartTypes.Body)
            {
                vinsInContainer.PartNumber = part.PartNumber.PartNumber;

                _vinContainers.Add(vinsInContainer);
            }

            return part;
        }

        private async Task<List<Tracing>> CreateTracing(Lot lot, ContainersInLot container)
        {
            List<Tracing> tracing = new();

            if (lot.LotDepartureLocation is not null)
            {
                Tracing traceDeparture = new()
                {
                    ContainerInLot = container,
                    TraceAtd = lot.LotAtd,
                    TraceTransport = lot.LotTransport,
                    TransportationType = lot.LotTransportType,
                    TraceTransportDocument = lot.LotTransportDocument,
                    TraceLocation = lot.LotDepartureLocation
                };

                tracing.Add(traceDeparture);
            }

            if (lot.LotArrivalLocation is not null)
            {
                Tracing traceArrival = new()
                {
                    Carrier = lot.Carrier,
                    ContainerInLot = container,
                    TraceLocation = lot.LotArrivalLocation
                };

                tracing.Add(traceArrival);
            }

            if (lot.LotCustomsLocation is not null)
            {
                Tracing traceCustoms = new()
                {
                    Carrier = lot.Carrier,
                    ContainerInLot = container,
                    TraceLocation = lot.LotCustomsLocation
                };

                tracing.Add(traceCustoms);
            }

            Tracing trace = new()
            {
                Carrier = lot.Carrier,
                ContainerInLot = container,
                TraceEta = lot.LotEta,
            };

            Location finalLocation = await _staticDataService.GetFinalLocationAsync();

            if (finalLocation is null)
            {
                throw new Exception($"Не найдена финальная локация при формировании трейсинга.");
            }

            trace.TraceLocation = finalLocation;

            tracing.Add(trace);

            return tracing;
        }

        private async Task<PartsInInvoice> CreatePartInInvoiceAsync(PartsInContainer partInContainer, Lot lot, List<PartPrice> uploadedPartPrices, List<PartsInContainer> partsInContainer)
        {
            PartsInInvoice part = new()
            {
                Invoice = lot.LotInvoice
            };

            part.PartNumber = (await GetAllCustomsPartsAsync()).FirstOrDefault(p => p.PartNumber == partInContainer.PartNumber.PartNumber);

            if (part.PartNumber is null)
            {
                throw new Exception($"Не найдена деталь по номеру '{partInContainer.PartNumber.PartNumber}'");
            }

            PartPrice priceFromPrices = uploadedPartPrices.FirstOrDefault(pp => pp.PartNumber == part.PartNumber.PartNumber);

            if (priceFromPrices is null)
            {
                throw new Exception($"Не найдена цена по номеру детали '{partInContainer.PartNumber.PartNumber}'");
            }

            part.Price = priceFromPrices.Price;

            part.Quantity = CalculateTotalQuantities(partsInContainer).TryGetValue(part.PartNumber.Id, out decimal quantity)
                ? quantity
                : throw new Exception($"Не найдено количество для детали с PartNumberId '{part.PartNumber.PartNumber}'");

            return part;
        }

        private async Task<CustomsPart> TryCreateCustomsPart(Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            CustomsPart customsPart = new();

            DocmapperContent contentPartNumber = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(CustomsPart).GetSystemColumnName(nameof(CustomsPart.PartNumber)));

            if (!customsPart.TrySetAndValidateProperty(nameof(CustomsPart.PartNumber), GraftValueFromRow(document, row, typeof(CustomsPart), nameof(CustomsPart.PartNumber)), row + 1, contentPartNumber.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            DocmapperContent contentPartNameEng = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(CustomsPart).GetSystemColumnName(nameof(CustomsPart.PartNameEng)));

            if (!customsPart.TrySetAndValidateProperty(nameof(CustomsPart.PartNameEng), GraftValueFromRow(document, row, typeof(CustomsPart), nameof(CustomsPart.PartNameEng)), row + 1, contentPartNameEng.ColumnNr, out result))
            {
                validationErrors.Merge(result);
            }
            else
            {
                BomPart newBomPart = new()
                {
                    PartNumber = customsPart.PartNumber,
                    PartName = customsPart.PartNameEng
                };

                BomPart bomPart = await _bomService.GetExistingBomPart(customsPart.PartNumber) ?? await _bomService.SaveNewBomPart(newBomPart);

                customsPart.PartNumberId = bomPart.Id;
            }

            return customsPart;
        }

        private async Task<List<PartsInInvoice>> CreateUniquePartsInInvoiceAsync(List<PartsInContainer> partsInContainer, Lot lot, List<PartPrice> uploadedPartPrices)
        {
            List<PartsInInvoice> partsInInvoices = new();

            foreach (IGrouping<int, PartsInContainer> group in partsInContainer.GroupBy(part => part.PartNumberId))
            {
                IEnumerable<PartsInContainer> uniqueParts = group.GroupBy(part => part.PartNumber.PartNumber)
                                       .Select(partGroup => partGroup.First());

                foreach (PartsInContainer uniquePart in uniqueParts)
                {
                    PartsInInvoice partInInvoices = await CreatePartInInvoiceAsync(uniquePart, lot, uploadedPartPrices, partsInContainer);

                    partsInInvoices.Add(partInInvoices);
                }
            }

            return partsInInvoices;
        }

        private async Task<List<Tracing>> CreateUniqueContainersTracingAsync(Lot lot, List<ContainersInLot> containers)
        {
            List<Tracing> tracing = new();

            foreach (IGrouping<string, ContainersInLot> group in containers.GroupBy(container => container.ContainerNumber))
            {
                IEnumerable<ContainersInLot> uniqueContainers = group.GroupBy(container => container.ContainerNumber)
                                       .Select(containerGroup => containerGroup.First());

                foreach (ContainersInLot uniqueContainer in uniqueContainers)
                {
                    List<Tracing> tracingItems = await CreateTracing(lot, uniqueContainer);

                    tracing.AddRange(tracingItems);
                }
            }

            return tracing;
        }

        private void HandleValidationErrors(ProcessStep step)
        {
            if (step.ValidationErrors.Any(s => s.Key != nameof(VinsInContainer.SupplierVinNumber)))
            {
                _excelService.ColorCellsInDocument(step.ValidationErrors, step.Docmapper.SheetName, step.Docmapper.NgFolder, step.Docmapper.Folder);

                step.ValidationErrors.Clear();

                throw new Exception($"Имеются ошибки препятствующие дальнейшему продолжению, исправьте ошибки в файле '{step.Docmapper.Folder}'");
            }
        }

        #region Save data

        private async Task<Invoice> SaveInvoiceAsync(Lot lot)
        {
            return await GetExistingInvoiceAsync(lot) is null
                ? await SaveNewInvoiceAsync(lot)
                : throw new Exception($"Документ с номером инвойса '{lot.LotInvoice.InvoiceNumber}' уже загружен.");
        }

        private async Task<Lot> SaveLotAsync(Lot lot)
        {
            return await GetExistingLotAsync(lot) ?? await SaveNewLotAsync(lot);
        }

        private async Task SavePartInContainers(List<PartsInContainer> partsInContainer, Lot lot)
        {
            double totalProgress = partsInContainer.Count;

            double currentProgress = 0.0;

            foreach (PartsInContainer partInContainer in partsInContainer)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение детали в контейнер {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                partInContainer.PartInvoice = lot.LotInvoice;

                PartsInContainer existingPart = await GetExistingPartInContainerAsync(partInContainer);

                if (existingPart is not null)
                {
                    partInContainer.Id = existingPart.Id;
                    partInContainer.ContainerInLotId = partInContainer.ContainerInLot.Id;
                    partInContainer.CaseId = partInContainer.Case.Id;
                    partInContainer.PartNumberId = partInContainer.PartNumber.PartNumberId;
                }
                else
                {
                    _ = await SaveNewPartInContainer(partInContainer);

                    partInContainer.ContainerInLotId = partInContainer.ContainerInLot.Id;
                    partInContainer.CaseId = partInContainer.Case.Id;
                    partInContainer.PartNumberId = partInContainer.PartNumber.PartNumberId;
                }
            }
        }

        private async Task SaveCustomsParts(List<CustomsPart> customsParts)
        {
            double totalProgress = customsParts.Count;

            double currentProgress = 0.0;

            foreach (CustomsPart customsPart in customsParts)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение детали {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                _ = await GetExistingCustomsPartAsync(customsPart) ?? await SaveNewCustomsPart(customsPart);
            }
        }

        private async Task SaveCases(List<Case> cases)
        {
            double totalProgress = cases.Count;

            double currentProgress = 0.0;

            foreach (Case caseItem in cases)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение кейса {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                Case existingCase = await GetExistingCaseAsync(caseItem);

                if (existingCase is not null)
                {
                    caseItem.Id = existingCase.Id;
                }
                else
                {
                    _ = await SaveNewCase(caseItem);
                }
            }
        }

        private async Task SaveContainers(List<ContainersInLot> containers, Lot lot)
        {
            double totalProgress = containers.Count;

            double currentProgress = 0.0;

            foreach (ContainersInLot container in containers)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение контейнера {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                ContainersInLot containerInOpenLot = await GetContainerInOpenLot(container.ContainerNumber);

                if (containerInOpenLot is not null)
                {
                    container.Id = containerInOpenLot.Id;
                    container.Lot = await GetLotByIdAsync(containerInOpenLot.LotId);
                }
                else
                {
                    container.Lot = lot;

                    _ = await SaveNewContainerAsync(container);
                }
            }
        }

        private async Task SaveVinContainers(List<VinsInContainer> vinContainers, Lot lot)
        {
            double totalProgress = vinContainers.Count;

            double currentProgress = 0.0;

            foreach (VinsInContainer vinContainer in vinContainers)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение вин-контейнер {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                vinContainer.Modvar = (await GetAllModelVariantsAsync(true)).FirstOrDefault(md => md.PartNumber == vinContainer.PartNumber);

                if (vinContainer.ModvarId == 0)
                {
                    throw new Exception($"Не найден релиз для кузова '{vinContainer.SupplierVinNumber}' и детали '{vinContainer.PartNumber}' проверьте корректность ввода или обратитесь в ДСС.");
                }

                vinContainer.Lot = lot;
                vinContainer.ContainerInLotId = vinContainer.ContainerInLot.Id;

                _ = await GetExistingVinContainerAsync(vinContainer.SupplierVinNumber) ?? await SaveNewVinContainer(vinContainer);
            }
        }

        private async Task SaveCustomsClearance(List<CustomsClearance> customsClearances)
        {
            double totalProgress = customsClearances.Count;

            double currentProgress = 0.0;

            foreach (CustomsClearance сustomsClearance in customsClearances)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение таможенной очистки {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                CustomsClearance existingCustomsClearance = await GetExistingCustomsClearanceItemAsync(сustomsClearance);

                if (existingCustomsClearance is not null)
                {
                    сustomsClearance.ContainerInLotId = сustomsClearance.ContainersInLot.Id;
                }
                else
                {
                    _ = await SaveNewCustomsClearance(сustomsClearance);
                }
            }
        }

        private async Task SavePartsInInvoice(List<PartsInInvoice> partsInInvoice)
        {
            double totalProgress = partsInInvoice.Count;

            double currentProgress = 0.0;

            foreach (PartsInInvoice part in partsInInvoice)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение детали в инвойсе {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                _ = await GetExistingPartInInvoiceAsync(part) ?? await SaveNewPartInInvoiceAsync(part);
            }
        }

        private async Task SaveTracing(List<Tracing> tracings)
        {
            double totalProgress = tracings.Count;

            double currentProgress = 0.0;

            foreach (Tracing trace in tracings)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = $"Пожалуйста подождите",
                    Message = $"Сохранение трейсинга {progress} из {totalProgress}."
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                _ = await GetExistingTraceAsync(trace) ?? await SaveNewTraceAsync(trace);
            }
        }

        #endregion Save data

        #region Get Existing data
        private bool HasAccess(ProcessStep step)
        {
            return step.Section.Id == _userService.GetCurrentUser().Section?.Id;
        }

        private async Task<Tracing> GetExistingTraceAsync(Tracing trace)
        {
            return (await GetAllTracingAsync())
                 .FirstOrDefault(
                t => t.ContainerInLotId == trace.ContainerInLotId &&
                t.CarrierId == trace.CarrierId &&
                t.TraceTransportId == trace.TraceTransportId &&
                t.TransportationTypeId == trace.TransportationTypeId &&
                t.TraceLocationId == trace.TraceLocationId &&
                t.TraceTransportDocument == trace.TraceTransportDocument &&
                t.TraceAta == trace.TraceAta &&
                t.TraceAtd == trace.TraceAtd &&
                t.TraceEta == trace.TraceEta &&
                t.TraceEtd == trace.TraceEtd);
        }

        private async Task<PartsInContainer> GetExistingPartInContainerAsync(PartsInContainer part)
        {
            PartsInContainer partInContainer = (await GetAllPartsInContainer())
                .FirstOrDefault(
                pc => pc.ContainerInLotId == part.ContainerInLot.Id &&
                pc.CaseId == part.Case.Id &&
                pc.PartNumberId == part.PartNumber.PartNumberId &&
                pc.Quantity == part.Quantity &&
                pc.PartInvoiceId == part.PartInvoice.Id);

            if (partInContainer is null)
            {
                return null;
            }

            partInContainer.ContainerInLot = (await GetAllContainersAsync()).
                FirstOrDefault(c => c.Id == partInContainer.ContainerInLotId);

            if (partInContainer.ContainerInLot is null)
            {
                throw new Exception($"Не обнаружен контейнер по уникальному идентификатору {partInContainer.ContainerInLotId}");
            }

            partInContainer.Case = (await GetAllCasesAsync()).
                FirstOrDefault(c => c.Id == partInContainer.CaseId);

            if (partInContainer.Case is null)
            {
                throw new Exception($"Не обнаружен кейс по уникальному идентификатору {partInContainer.CaseId}");
            }

            partInContainer.PartNumber = (await GetAllCustomsPartsAsync()).
                FirstOrDefault(c => c.Id == partInContainer.PartNumberId);

            if (partInContainer.PartNumber is null)
            {
                throw new Exception($"Не обнаружена деталь по уникальному идентификатору {partInContainer.PartNumberId}");
            }

            partInContainer.PartInvoice = (await GetAllInvoiceItemsAsync()).
                FirstOrDefault(c => c.Id == partInContainer.PartInvoiceId);

            if (partInContainer.PartInvoice is null)
            {
                throw new Exception($"Не обнаружен инвойс по уникальному идентификатору {partInContainer.PartInvoiceId}");
            }

            return partInContainer;
        }

        private async Task<PartsInInvoice> GetExistingPartInInvoiceAsync(PartsInInvoice uniquePart)
        {
            return (await GetAllPartsInInvoice()).FirstOrDefault(pi => pi.PartNumberId == uniquePart.PartNumberId && pi.InvoiceId == uniquePart.InvoiceId);
        }

        private async Task<CustomsClearance> GetExistingCustomsClearanceItemAsync(CustomsClearance customsClearance)
        {
            return (await GetAllCustomsClearanceAsync()).FirstOrDefault(cc => cc.ContainerInLotId == customsClearance.ContainersInLot.Id);
        }

        private async Task<Invoice> GetExistingInvoiceAsync(Lot lot)
        {
            IEnumerable<Invoice> invoices = await GetAllInvoiceItemsAsync();

            return invoices.FirstOrDefault(l => l.InvoiceNumber == lot.LotInvoice.InvoiceNumber);
        }

        private async Task<Lot> GetExistingLotAsync(Lot lot)
        {
            Lot existingLot = (await GetAllLotsAsync()).FirstOrDefault(l => l.LotNumber == lot.LotNumber);

            if (existingLot is not null)
            {
                existingLot.LotInvoice ??= await GetInvoiceByIdAsync(lot.LotInvoiceId);
                existingLot.Carrier ??= await _staticDataService.GetCarrierByIdAsync(lot.CarrierId);
                existingLot.DeliveryTerms ??= await _staticDataService.GetDeliveryTermByIdAsync(lot.DeliveryTermsId);
                existingLot.LotArrivalLocation ??= await _staticDataService.GetLocationByIdAsync(lot.LotArrivalLocationId);
                existingLot.LotDepartureLocation ??= await _staticDataService.GetLocationByIdAsync(lot.LotDepartureLocationId);
                existingLot.LotTransportType ??= await _staticDataService.GetTransportTypeByIdAsync(lot.LotTransportTypeId);
                existingLot.Shipper ??= await _staticDataService.GetShipperByIdAsync(lot.ShipperId);
                existingLot.LotPurchaseOrder ??= await GetPurchaseOrderByIdAsync(lot.LotPurchaseOrderId);

                if (lot.LotTransportId is not null)
                {
                    existingLot.LotTransport = await _staticDataService.GetTransportByIdAsync((int)lot.LotTransportId);
                }

                if (lot.LotCustomsLocationId is not null)
                {
                    existingLot.LotCustomsLocation = await _staticDataService.GetLocationByIdAsync((int)lot.LotCustomsLocationId);
                }

                return existingLot;
            }

            return null;
        }

        private async Task<VinsInContainer> GetExistingVinContainerAsync(string supplierVinNumber)
        {
            return (await GetAllVinContainersAsync()).FirstOrDefault(vc => vc.SupplierVinNumber == supplierVinNumber);
        }

        private async Task<CustomsPart> GetExistingCustomsPartAsync(CustomsPart part)
        {
            return (await GetAllCustomsPartsAsync())
                .FirstOrDefault(c => c.PartNumber == part.PartNumber);
        }

        private async Task<Case> GetExistingCaseAsync(Case caseItem)
        {
            return (await GetAllCasesAsync()).FirstOrDefault(c => c.CaseNo == caseItem.CaseNo);
        }

        #endregion

        #region Graft
        private static object GraftValueFromRow(Docmapper document, int row, Type model, string nameOfProperty)
        {
            object result = document.GetValue(model, nameOfProperty, row);

            return result;
        }

        private static object GraftLotNumber(Lot lot, Docmapper document)
        {
            if (lot.LotNumber == "-" || string.IsNullOrWhiteSpace(lot.LotNumber))
            {
                object result = document.GetValue(typeof(Invoice), nameof(Invoice.InvoiceNumber));

                return result;
            }
            else
            {
                return lot.LotNumber;
            }
        }

        #endregion

        #region Setters

        private async Task<Tracing> SaveNewTraceAsync(Tracing newTrace)
        {
            CreateTraceParameters parameters = new(newTrace);

            try
            {
                return await _tracingRepository.CreateAsync(newTrace, StoredProcedureInbound.AddNewTrace, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new trace {JsonConvert.SerializeObject(newTrace)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления трейсинга в базу данных.");
            }
        }

        private async Task<PartsInInvoice> SaveNewPartInInvoiceAsync(PartsInInvoice newPart)
        {
            CreatePartInInvoiceParameters parameters = new(newPart);

            try
            {
                return await _partInInvoiceRepository.CreateAsync(newPart, StoredProcedureInbound.AddNewPartInInvoice, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new part in invoice {JsonConvert.SerializeObject(newPart)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления детали в инвойсе в базу данных.");
            }
        }

        private async Task<PartsInContainer> SaveNewPartInContainer(PartsInContainer newPart)
        {
            CreatePartInContainerParameters parameters = new(newPart);

            try
            {
                return await _partInContainerRepository.CreateAsync(newPart, StoredProcedureInbound.AddNewPartInContainer, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new part in container {JsonConvert.SerializeObject(newPart)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления детали в контейнере в базу данных.");
            }
        }

        private async Task<CustomsClearance> SaveNewCustomsClearance(CustomsClearance newCustomsClearance)
        {

            CreateCustomsClearanceParameters parameters = new(newCustomsClearance);

            try
            {
                return await _customsClearanceRepository.CreateAsync(newCustomsClearance, StoredProcedureCustoms.AddNewCustomsClearance, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new customs clearance {JsonConvert.SerializeObject(newCustomsClearance)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления таможенной процедуры в базу данных.");
            }
        }

        private async Task<VinsInContainer> SaveNewVinContainer(VinsInContainer newVinContainer)
        {
            CreateVinContainerParameters parameters = new(newVinContainer);

            try
            {
                return await _vinContainerRepository.CreateAsync(newVinContainer, StoredProcedurePlanning.AddNewVinContainer, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new vin container {JsonConvert.SerializeObject(newVinContainer)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления вин-контейнер в базу данных.");
            }
        }

        private async Task<Invoice> SaveNewInvoiceAsync(Lot lot)
        {
            CreateInvoiceParameters parameters = new(lot.LotInvoice);

            try
            {
                return await _invoiceRepository.CreateAsync(lot.LotInvoice, StoredProcedureInbound.AddNewInvoice, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new invoice {JsonConvert.SerializeObject(lot.LotInvoice)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления инвойса в базу данных.");
            }
        }

        private async Task<Lot> SaveNewLotAsync(Lot newLot)
        {
            CreateLotParameters parameters = new(newLot);

            try
            {
                Lot lot = await _lotRepository.CreateAsync(newLot, StoredProcedureInbound.AddNewLot, parameters);

                if (lot is not null)
                {
                    lot.LotInvoice ??= await GetInvoiceByIdAsync(lot.LotInvoiceId);
                    lot.Carrier ??= await _staticDataService.GetCarrierByIdAsync(lot.CarrierId);
                    lot.DeliveryTerms ??= await _staticDataService.GetDeliveryTermByIdAsync(lot.DeliveryTermsId);
                    lot.LotArrivalLocation ??= await _staticDataService.GetLocationByIdAsync(lot.LotArrivalLocationId);
                    lot.LotDepartureLocation ??= await _staticDataService.GetLocationByIdAsync(lot.LotDepartureLocationId);
                    lot.LotTransportType ??= await _staticDataService.GetTransportTypeByIdAsync(lot.LotTransportTypeId);
                    lot.Shipper ??= await _staticDataService.GetShipperByIdAsync(lot.ShipperId);
                    lot.LotPurchaseOrder ??= await GetPurchaseOrderByIdAsync(lot.LotPurchaseOrderId);

                    if (lot.LotTransportId is not null)
                    {
                        lot.LotTransport = await _staticDataService.GetTransportByIdAsync((int)lot.LotTransportId);
                    }

                    if (lot.LotCustomsLocationId is not null)
                    {
                        lot.LotCustomsLocation = await _staticDataService.GetLocationByIdAsync((int)lot.LotCustomsLocationId);
                    }

                    return lot;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new lot {JsonConvert.SerializeObject(newLot)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления лота в базу данных.");
            }
        }

        private async Task<CustomsPart> SaveNewCustomsPart(CustomsPart newCustomsPart)
        {
            CreateCustomsPartParameters parameters = new(newCustomsPart);

            try
            {
                return await _customsPartRepository.CreateAsync(newCustomsPart, StoredProcedureCustoms.AddNewCustomsPart, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new customs part {JsonConvert.SerializeObject(newCustomsPart)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления детали в базу данных.");
            }
        }

        private async Task<Case> SaveNewCase(Case newCase)
        {
            CreateCaseParameters parameters = new(newCase);

            try
            {
                return await _caseRepository.CreateAsync(newCase, StoredProcedureInbound.AddNewCase, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new case {JsonConvert.SerializeObject(newCase)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления кейса в базу данных.");
            }
        }

        private async Task<ContainersInLot> SaveNewContainerAsync(ContainersInLot newContainer)
        {
            CreateContainerParameters parameters = new(newContainer);

            try
            {
                return await _containerRepository.CreateAsync(newContainer, StoredProcedureInbound.AddNewContainer, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new container {JsonConvert.SerializeObject(newContainer)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления контейнера в базу данных.");
            }
        }

        #endregion

        #region Getters

        /// <inheritdoc />
        public async Task<Lot> GetLotByIdAsync(int lotId)
        {
            Lot lot;
            Carrier carrier;
            Shipper shipper;
            Invoice invoice;
            PurchaseOrder order;
            TypesOfOrder orderType;
            TermsOfDelivery termsOfDelivery;

            try
            {
                lot = await _lotRepository.GetByIdAsync(lotId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get lot by id {lotId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения лота по уникальному идентификатору '{lotId}'");
            }

            carrier = await _staticDataService.GetCarrierByIdAsync(lot.CarrierId);

            lot.Carrier = carrier;

            order = await GetPurchaseOrderByIdAsync(lot.LotPurchaseOrderId);

            lot.LotPurchaseOrder = order;

            orderType = await _staticDataService.GetPurchaseOrderTypeById(lot.LotPurchaseOrder.OrderTypeId);

            lot.LotPurchaseOrder.OrderType = orderType;

            invoice = await GetInvoiceByIdAsync(lot.LotInvoiceId);

            lot.LotInvoice = invoice;

            shipper = await _staticDataService.GetShipperByIdAsync(lot.LotInvoice.ShipperId);

            lot.Shipper = shipper;
            lot.LotPurchaseOrder.Shipper = shipper;
            invoice.Shipper = shipper;
            invoice.PurchaseOrder = order;

            termsOfDelivery = await _staticDataService.GetDeliveryTermByIdAsync(lot.DeliveryTermsId);

            lot.DeliveryTerms = termsOfDelivery;

            lot.LotArrivalLocation = await _staticDataService.GetLocationByIdAsync(lot.LotArrivalLocationId);

            if (lot.LotArrivalLocation is not null)
            {
                lot.LotArrivalLocation.LocationType = await _staticDataService.GetLocationTypeByIdAsync(lot.LotArrivalLocation.LocationTypeId);
            }

            if (lot.LotCustomsLocationId is not null)
            {
                lot.LotCustomsLocation = await _staticDataService.GetLocationByIdAsync((int)lot.LotCustomsLocationId);

                if (lot.LotCustomsLocation is not null)
                {
                    lot.LotCustomsLocation.LocationType = await _staticDataService.GetLocationTypeByIdAsync(lot.LotCustomsLocation.LocationTypeId);
                }
            }

            lot.LotDepartureLocation = await _staticDataService.GetLocationByIdAsync(lot.LotDepartureLocationId);

            if (lot.LotDepartureLocation is not null)
            {
                lot.LotDepartureLocation.LocationType = await _staticDataService.GetLocationTypeByIdAsync(lot.LotDepartureLocation.LocationTypeId);
            }

            if (lot.LotTransportId is not null)
            {
                lot.LotTransport = await _staticDataService.GetTransportByIdAsync((int)lot.LotTransportId);

                if (lot.LotTransport is not null)
                {
                    lot.LotTransportType = await _staticDataService.GetTransportTypeByIdAsync(lot.LotTransportTypeId);
                }
            }

            return lot;
        }

        /// <inheritdoc />
        public async Task<List<ContainersInLot>> GetAllContainersByLotIdAsync(int lotId)
        {
            try
            {
                IEnumerable<ContainersInLot> containers = (await GetAllContainersAsync())
                    .Where(c => c.LotId == lotId);

                foreach (ContainersInLot container in containers)
                {
                    container.ContainerType = await _staticDataService.GetContainerTypeById(container.ContainerTypeId);
                }

                return containers.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list containers by lot id {lotId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения контейнеров для лота.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            try
            {
                return await _purchaseOrderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list purchase orders: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения заказов.");
            }
        }

        /// <inheritdoc />
        public async Task<List<PartsInContainer>> GetPartsForContainerAsync(int containerId)
        {
            List<PartsInContainer> parts = new();

            try
            {
                parts = (await GetAllPartsInContainer())
                    .Where(c => c.ContainerInLotId == containerId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list parts by container id {containerId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения деталей для контейнера.");
            }

            foreach (PartsInContainer part in parts)
            {
                if (part.CaseId is not null)
                {
                    part.Case = await GetCaseByIdAsync((int)part.CaseId);
                }

                part.PartInvoice = await GetInvoiceByIdAsync(part.PartInvoiceId);

                part.PartNumber = await GetPartNumberByIdAsync(part.PartNumberId);

                if (part.PartNumber is not null)
                {
                    part.PartNumber.PartType = await _staticDataService.GetPartTypeByIdAsync((int)part.PartNumber.PartTypeId);
                }
            }

            return parts;
        }

        /// <inheritdoc />
        public async Task<int> GetquantityContainersForLotId(int lotId)
        {
            _logger.LogInformation($"Start getting quantity containers for lot with uniq id '{lotId}'");

            List<ContainersInLot> containers = await GetAllContainersByLotIdAsync(lotId);

            _logger.LogInformation($"Getting quantity containers for lot with uniq id '{lotId}' completed with result: '{containers.Count}'");

            return containers.Count;
        }

        private async Task<CustomsPart> GetPartNumberByIdAsync(int partNumberId)
        {
            try
            {
                CustomsPart part = await _customsPartRepository.GetByIdAsync(partNumberId);

                if (part is not null)
                {
                    part.PartType = await _staticDataService.GetPartTypeByIdAsync(part.PartTypeId);
                }

                return part;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get customs part by id {partNumberId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения таможенного обозначения детали по уникальному идентификатору '{partNumberId}'.");
            }
        }

        private async Task<Case> GetCaseByIdAsync(int caseId)
        {
            Case caseItem;

            try
            {
                caseItem = await _caseRepository.GetByIdAsync(caseId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get case by id {caseId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения кейса по уникальному идентификатору '{caseId}'.");
            }

            if (caseItem.PackingTypeId is not null)
            {
                caseItem.PackingType = await _staticDataService.GetPackingTypeByIdAsync((int)caseItem.PackingTypeId);
            }

            return caseItem;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderById(int lotPurchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(lotPurchaseOrderId);

                if (purchaseOrder is not null)
                {
                    purchaseOrder.Shipper = await _staticDataService.GetShipperByIdAsync(purchaseOrder.ShipperId);

                    purchaseOrder.OrderType = await _staticDataService.GetPurchaseOrderTypeById(purchaseOrder.OrderTypeId);
                }

                return purchaseOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get order by id {lotPurchaseOrderId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения заказа по уникальному идентификатору '{lotPurchaseOrderId}'");
            }
        }

        private async Task<CustomsPart> GetPartByPartNumber(string partNumber)
        {
            try
            {
                return (await GetAllCustomsPartsAsync()).FirstOrDefault(p => p.PartNumber == partNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get customs part by part number '{partNumber}': {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения таможенной детали по номеру детали '{partNumber}'.");
            }
        }

        private async Task<List<Tracing>> GetAllTracingAsync()
        {
            IEnumerable<Tracing> tracing;

            try
            {
                tracing = await _tracingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list tracing: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения трейсинга.");
            }

            try
            {
                foreach (Tracing item in tracing)
                {
                    item.ContainerInLot = await GetContainerByIdAsync(item.ContainerInLotId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list tracing: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения трейсинга.");
            }

            return tracing?.ToList();
        }

        private async Task<ContainersInLot> GetContainerByIdAsync(int containerInLotId)
        {
            try
            {
                return await _containerRepository.GetByIdAsync(containerInLotId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get container by id '{containerInLotId}': {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения контейнера по уникальному идентификатору '{containerInLotId}'.");
            }
        }

        private async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            try
            {
                return await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get purchase order by id '{purchaseOrderId}': {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения заказа по уникальному идентификатору '{purchaseOrderId}'.");
            }
        }

        private async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            try
            {
                Invoice invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

                if (invoice is not null)
                {
                    invoice.Shipper = await _staticDataService.GetShipperByIdAsync(invoice.ShipperId);

                    if (invoice.PurchaseOrderId is not null)
                    {
                        invoice.PurchaseOrder = await GetPurchaseOrderById((int)invoice.PurchaseOrderId);
                    }
                }

                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get invoice by id {invoiceId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения инвойса по уникальному идентификатору '{invoiceId}'.");
            }
        }

        private async Task<IEnumerable<PartsInContainer>> GetAllPartsInContainer()
        {
            try
            {
                IEnumerable<PartsInContainer> parts = await _partInContainerRepository.GetAllAsync();

                if (parts is not null)
                {
                    foreach (PartsInContainer item in parts)
                    {
                        item.PartNumber = await GetPartNumberByIdAsync(item.PartNumberId);
                    }
                }

                return parts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list parts in container: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения деталей в контейнере.");
            }
        }

        private async Task<IEnumerable<PartsInInvoice>> GetAllPartsInInvoice()
        {
            try
            {
                return await _partInInvoiceRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list parts in invoice: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения деталей в лоте.");
            }
        }

        private async Task<IEnumerable<CustomsClearance>> GetAllCustomsClearanceAsync()
        {
            try
            {
                return await _customsClearanceRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all customs clearance items list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка таможенных процедур.");
            }
        }

        private async Task<IEnumerable<Invoice>> GetAllInvoiceItemsAsync()
        {
            try
            {
                return await _invoiceRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all invoice items list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка инвойсов.");
            }
        }

        private async Task<IEnumerable<VinsInContainer>> GetAllVinContainersAsync()
        {
            try
            {
                return await _vinContainerRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all vin containers list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка вин-контейнер.");
            }
        }

        private async Task<IEnumerable<BodyModelVariant>> GetAllModelVariantsAsync(bool beforeRefresh)
        {
            try
            {
                return await _bodyModelVariantRepository.GetAllAsync(beforeRefresh);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all model variants list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка вариантов моделей.");
            }
        }

        private async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            try
            {
                return await _caseRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get cases list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка кейсов.");
            }
        }

        public async Task<List<Lot>> GetAllLotsAsync()
        {
            IEnumerable<Lot> lots;
            try
            {
                lots = await _lotRepository.GetAllAsync();

                foreach (Lot lot in lots)
                {
                    lot.Carrier = await _staticDataService.GetCarrierByIdAsync(lot.CarrierId);

                    lot.DeliveryTerms = await _staticDataService.GetDeliveryTermByIdAsync(lot.DeliveryTermsId);

                    lot.LotArrivalLocation = await _staticDataService.GetLocationByIdAsync(lot.LotArrivalLocationId);

                    lot.LotDepartureLocation = await _staticDataService.GetLocationByIdAsync(lot.LotDepartureLocationId);

                    lot.LotInvoice = await GetInvoiceByIdAsync(lot.LotInvoiceId);

                    lot.LotPurchaseOrder = await GetPurchaseOrderById(lot.LotPurchaseOrderId);

                    lot.LotTransportType = await _staticDataService.GetTransportTypeByIdAsync(lot.LotTransportTypeId);

                    lot.Shipper = await _staticDataService.GetShipperByIdAsync(lot.ShipperId);

                    if (lot.LotTransportId is not null)
                    {
                        lot.LotTransport = await _staticDataService.GetTransportByIdAsync((int)lot.LotTransportId);
                    }

                    if (lot.LotCustomsLocationId is not null)
                    {
                        lot.LotCustomsLocation = await _staticDataService.GetLocationByIdAsync((int)lot.LotCustomsLocationId);
                    }
                }

                return lots.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all lot items list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка лотов.");
            }
        }

        private async Task<IEnumerable<CustomsPart>> GetAllCustomsPartsAsync()
        {
            try
            {
                return await _customsPartRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all customs part items list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка деталей.");
            }
        }

        private async Task<IEnumerable<ContainersInLot>> GetAllContainersAsync()
        {
            try
            {
                return await _containerRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all container items list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка контейнеров.");
            }
        }

        public async Task<List<ProcessStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            try
            {
                return (await _processService.GetProcessStepsByUserSectionAsync(appProcess)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get operation for process '{appProcess}' from database: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения операций для процесса.");
            }
        }

        private async Task<ContainersInLot> GetContainerInOpenLot(string containerNumber)
        {
            try
            {
                IEnumerable<Lot> cachedLots = await GetAllLotsAsync();

                return (await GetAllContainersAsync())
                    .FirstOrDefault(c => c.ContainerNumber == containerNumber &&
                    cachedLots.Any(lot => lot.CloseDate == null && lot.Id == c.LotId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get container '{containerNumber}' in open lot: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения контейнера в открытых лотах.");
            }
        }
        #endregion

        #region Rollback

        //Lot-Invoice---?-Container-tracing-parts-CustomsClerance-vinContainer-Plannedsequence

        private async Task RollbackLotCreation(Lot lot)
        {
            if (lot.LotInvoice != null)
            {
                await RollbackInvoiceCreation(lot.LotInvoice);
            }
        }

        private async Task RollbackInvoiceCreation(Invoice invoice)
        {
            try
            {
                await DeleteInvoice(invoice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task DeleteInvoice(Invoice invoice)
        {
            await _invoiceRepository.RemoveAsync(invoice.Id, StoredProcedureInbound.DeleteInvoice);
        }

        #endregion Rollback

        /// <inheritdoc />
        public event EventHandler<ControllerDetails> DeliveryLoadProgressUpdated;
    }
}
