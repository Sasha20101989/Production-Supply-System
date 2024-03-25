using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Models;
using BLL.Properties;

using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.BomContext.Models;
using production_supply_system.EntityFramework.DAL.BomModels;
using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.Extensions;
using production_supply_system.EntityFramework.DAL.LotContext;
using production_supply_system.EntityFramework.DAL.LotContext.Models;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace BLL.Services
{
    public partial class DeliveryService(
        LotContext lotContext,
        IUserService userService,
        IProcessService processService,
        IExcelService excelService,
        IStaticDataService staticDataService,
        IBOMService bomService,
        ILogger<DeliveryService> logger) : IDeliveryService
    {
        private List<ProcessesStep> _stepCollection;
        private List<VinsInContainer> _vinContainers;
        private List<CustomsClearance> _customsClearances;
        private List<Case> _cases;
        private List<CustomsPart> _customsParts;

        public EventHandler<List<ProcessesStep>> NavigatedWithStepCollection { get; set; }

        private static string ConvertDateTimeToString(DateOnly? date)
        {
            return date is null ? null : (date?.ToString(Resources.ExportFormatDate));
        }

        private static string ConvertDateTimeToString(DateTime? date)
        {
            return date is null ? null : (date?.ToString(Resources.ExportFormatDate));
        }

        private static Cell CreateCell(string text)
        {
            Cell cell = new()
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text)
            };

            return cell;
        }

        private static bool IsValidVinNumber(string vin)
        {
            return VinRegex().IsMatch(vin);
        }

        private static CustomsClearance CreateCustomsClearance(ContainersInLot container, PartsInContainer partInContainer)
        {
            CustomsClearance customsClearance = new()
            {
                ContainerInLot = container,
                PartType = partInContainer.PartNumber.PartType,
                InvoceNumber = CreateCustomsClearanceNumber(container.Lot, partInContainer)
            };

            return customsClearance.InvoceNumber is null
                ? throw new Exception(Resources.ErrorInvoceNumberCustomsClearance)
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
            string symbol = partInContainer?.PartNumber?.PartType?.PartType == PartTypes.Body.ToString() ? Resources.SymbolBody : partInContainer?.PartNumber?.PartType?.PartType == PartTypes.Parts.ToString() ? Resources.SymbolParts : null;

            return symbol != null ? $"{lot.LotNumber}-{symbol}" : null;
        }

        private static Dictionary<int, int> CalculateTotalQuantities(List<PartsInContainer> partsInContainer)
        {
            Dictionary<int, int> totalQuantities = partsInContainer
                .GroupBy(part => part.PartNumberId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(part => part.Quantity)
                );

            return totalQuantities;
        }

        private static VinsInContainer TryCreateVinsInContainer(ContainersInLot container, Dictionary<string, CellInfo> validationErrors, Docmapper document, int? row)
        {
            VinsInContainer vinsInContainer = new()
            {
                ContainerInLot = container
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
                ? throw new Exception(string.Format(Resources.NotValidVIN, vinsInContainer.SupplierVinNumber))
                : vinsInContainer;

            throw new NotImplementedException();
        }

        private static List<PartPrice> UploadPartPrices(List<ProcessesStep> steps, User currentUser)
        {
            List<PartPrice> partPrices = [];

            IEnumerable<ProcessesStep> accessibleSteps = steps
                .Where(step => step.StepName == Steps.UploadPrice.ToString())
                .Where(step => HasAccess(step, currentUser));

            foreach (ProcessesStep step in accessibleSteps)
            {
                if (step.Docmapper.FirstDataRow is not null)
                {
                    List<IGrouping<string, PartPrice>> groupedPartPrices = Enumerable.Range((int)step.Docmapper.FirstDataRow, step.Docmapper.Data.GetLength(0) - (int)step.Docmapper.FirstDataRow)
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
                                throw new Exception(string.Format(Resources.PartNumberDifferentPrice, group.Key));
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
            }

            return partPrices;
        }

        private static List<ContainersInLot> UploadContainerTypes(List<ProcessesStep> stepCollection, User currentUser)
        {
            List<ContainersInLot> containers = [];

            IEnumerable<ProcessesStep> accessibleSteps = stepCollection
                .Where(step => step.StepName == Steps.UploadContainerTypes.ToString())
                .Where(step => HasAccess(step, currentUser));

            foreach (ProcessesStep step in accessibleSteps)
            {
                if (step.Docmapper.FirstDataRow is not null)
                {
                    containers.AddRange(
                    Enumerable.Range((int)step.Docmapper.FirstDataRow, step.Docmapper.Data.GetLength(0) - (int)step.Docmapper.FirstDataRow)
                              .Select(i => CreateContainerWithType(step.Docmapper, i))
                              .Where(container => container != null));
                }
            }

            return containers;

            throw new NotImplementedException();
        }

        private static Invoice TryCreateInvoiceAsync(Docmapper document, Lot lot, Dictionary<string, CellInfo> validationErrors)
        {
            Invoice invoice = new()
            {
                InvoiceDate = DateTime.Now,
                PurchaseOrder = lot.LotPurchaseOrder,
                PurchaseOrderId = lot.LotPurchaseOrder.Id,
                Shipper = lot.Shipper,
                ShipperId = lot.Shipper.Id
            };

            DocmapperContent content = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(Invoice).GetSystemColumnName(nameof(Invoice.InvoiceNumber)));

            if (content is null)
            {
                return null;
            }

            if (content.RowNr is null)
            {
                return null;
            }

            int row = (int)content.RowNr;
            int col = content.ColumnNr;

            if (!lot.TrySetAndValidateProperty(nameof(Lot.LotNumber), GraftLotNumber(lot, document), row, col, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            invoice.InvoiceNumber = lot.LotNumber;

            return invoice;
        }

        public async Task<SheetData> GetAllTracingForPartner2ToExport(List<DocmapperContent> content)
        {
            logger.LogInformation(Resources.Partner2GetAllTracing);

            SheetData sheetData = new();

            List<Tracing> tracing = await GetAllTracingAsync();

            IOrderedEnumerable<DocmapperContent> orderedByColumnContent = content.OrderBy(c => c.ColumnNr);

            IEnumerable<Task<(Tracing TracingItem, ContainersInLot ContainerInOpenLot)>> tasks = tracing.Select(async tracingItem =>
            {
                ContainersInLot containerInOpenLot = await GetContainerInOpenLot(tracingItem.ContainerInLot.ContainerNumber);

                return (TracingItem: tracingItem, ContainerInOpenLot: containerInOpenLot);
            });

            (Tracing TracingItem, ContainersInLot ContainerInOpenLot)[] results = await Task.WhenAll(tasks);

            List<Tracing> filteredTracing = results
                .Where(result => result.ContainerInOpenLot is not null)
                .Select(result => result.TracingItem)
                .ToList();

            Row headerRow = new();

            foreach (DocmapperContent item in orderedByColumnContent)
            {
                headerRow.Append(CreateCell(item.DocmapperColumn.ElementName));
            }

            sheetData.Append(headerRow);

            List<Partner2TracingExport> tracingExportData = [];

            List<Tracing> containersInTransshipment = filteredTracing.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.TransshipmentPort)).ToList();
            List<Tracing> containersInStp = filteredTracing.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.ArrivalTerminal)).ToList();
            List<Tracing> containersInCustomClearance = filteredTracing.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.CustomsTerminal)).ToList();
            List<Tracing> containersInContainerYard = filteredTracing.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.ContainerYard)).ToList();
            List<Tracing> containersInPlant = filteredTracing.Where(t => t.TraceLocation.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.FinalLocation)).ToList();

            foreach (Tracing item in filteredTracing)
            {
                if (!tracingExportData.Any(ted => ted.ContainerNumber == item.ContainerInLot.ContainerNumber))
                {
                    ContainersInLot container = item.ContainerInLot;

                    Lot lot = item.ContainerInLot.Lot;

                    Partner2TracingExport exportItem = new()
                    {
                        CarrierName = lot.Carrier.CarrierName,
                        CargoType = await GetContainerCargoType(item.ContainerInLotId),
                        LotNumber = lot.LotNumber,
                        ContainerNumber = container.ContainerNumber,
                        InvoiceNumber = lot.LotInvoice.InvoiceNumber,
                        TransportType = lot.LotTransportType.TransportType,
                        ImoCargo = container.ImoCargo,
                        SealNumber = container.SealNumber,
                        LotEtd = ConvertDateTimeToString(lot.LotEtd),
                        LotDepartureLocation = lot.LotDepartureLocation.LocationName,
                        TransshipmentPort = containersInTransshipment.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceLocation.LocationName,
                        TransshipmentEtd = ConvertDateTimeToString(containersInTransshipment.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceEtd),
                        TransshipmentAtd = ConvertDateTimeToString(containersInTransshipment.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAtd),
                        TransshipmentEta = ConvertDateTimeToString(containersInTransshipment.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceEta),
                        TransshipmentAta = ConvertDateTimeToString(containersInTransshipment.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        StpTerminal = containersInStp.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceLocation.LocationName,
                        StpAta = ConvertDateTimeToString(containersInStp.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        StpEta = ConvertDateTimeToString(containersInStp.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceEta),
                        StorageLastFreeDay = ConvertDateTimeToString(container.StorageLastFreeDay),
                        DetentionLastFreeDay = ConvertDateTimeToString(container.DetentionLastFreeDay),
                        CustomsClearanceTerminal = containersInCustomClearance.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceLocation.LocationName,
                        CustomsClearanceAta = ConvertDateTimeToString(containersInCustomClearance.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        CustomsClearanceEta = ConvertDateTimeToString(containersInCustomClearance.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        DocstoCsbDate = ConvertDateTimeToString(await GetCustomsBrokerDocumentByMaxDate(item.ContainerInLotId)),
                        ContainerYardAta = ConvertDateTimeToString(containersInContainerYard.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        ContainerYardAtd = ConvertDateTimeToString(containersInContainerYard.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAtd),
                        AtaNmgr = ConvertDateTimeToString(containersInPlant.FirstOrDefault(c => c.ContainerInLotId == item.ContainerInLotId)?.TraceAta),
                        TargetEta = ConvertDateTimeToString(lot.LotEta),
                        CiOnTheWay = container.CiOnTheWay,
                        Comment = container.ContainerComment
                    };

                    tracingExportData.Add(exportItem);
                }
            }

            foreach (Partner2TracingExport item in tracingExportData)
            {
                Row dataRow = new();

                foreach (DocmapperContent contentItem in orderedByColumnContent)
                {
                    object propValue = item.GetPropertyByColumnAttribute(contentItem.DocmapperColumn.SystemColumnName);

                    dataRow.Append(CreateCell(propValue?.ToString()));
                }

                sheetData.Append(dataRow);
            }

            logger.LogInformation($"{Resources.Partner2GetAllTracing} {Resources.Completed}");

            return sheetData;
        }

        private async Task<DateOnly?> GetCustomsBrokerDocumentByMaxDate(int containerInLotId)
        {
            CustomsClearance customsClearance = (await GetAllCustomsClearanceAsync())

                .Where(cc => cc.ContainerInLotId == containerInLotId)

                .OrderByDescending(cc => cc.DocsToCustomsDate)

                .FirstOrDefault();

            return customsClearance?.DocsToCustomsDate;
        }

        public async Task<CargoTypes> GetContainerCargoType(int containerInLotId)
        {
            //IEnumerable<IGrouping<int, PartsInContainer>> groupingParts = (await GetAllPartsInContainer())
            //    .Where(p => p.ContainerInLotId == containerInLotId)
            //    .GroupBy(p => p.ContainerInLotId);

            //IGrouping<int, PartsInContainer> containerGroup = groupingParts.FirstOrDefault();

            //if (containerGroup != null)
            //{
            //    bool hasBody = containerGroup.Any(p => p.PartNumber.PartType.PartType == PartTypes.Body);

            //    bool hasParts = containerGroup.Any(p => p.PartNumber.PartType.PartType == PartTypes.Parts);

            //    return hasBody && hasParts ? CargoTypes.Mix :
            //                                    hasBody ? CargoTypes.Body :
            //                                    hasParts ? CargoTypes.Parts : CargoTypes.Unknown;
            //}

            throw new NotImplementedException();

            return CargoTypes.Unknown;
        }

        public async Task<Lot> StartUploadLotAsync(List<ProcessesStep> steps, Lot lotDetails)
        {
            _stepCollection ??= [.. steps.OrderBy(collection => collection.Step)];

            User currentUser = await userService.GetCurrentUser(Environment.UserName);

            return await UploadLotContentAsync(lotDetails, _stepCollection, currentUser);
        }

        private async Task<Lot> UploadLotContentAsync(Lot lot, List<ProcessesStep> steps, User currentUser)
        {
            IEnumerable<ProcessesStep> accessibleSteps = steps
                .Where(step => step.StepName == Steps.UploadLotContent.ToString())
                .Where(step => HasAccess(step, currentUser));

            foreach (ProcessesStep step in accessibleSteps)
            {
                List<PartPrice> uploadedPartPrices = UploadPartPrices(steps, currentUser);

                lot.LotInvoice = TryCreateInvoiceAsync(step.Docmapper, lot, step.ValidationErrors);

                HandleValidationErrors(step);

                await ProcessContainersInLotAsync(step, lot, currentUser);

                HandleValidationErrors(step);

                _ = await SaveInvoiceAsync(lot.LotInvoice);

                lot.LotInvoiceId = lot.LotInvoice.Id;

                lot = await SaveLotAsync(lot);

                await SaveContainers(lot);

                //    await SaveCases(_cases);

                //    await SaveCustomsParts(_customsParts);

                //    await SavePartInContainers(_partsInContainer, lot);

                //    await SaveVinContainers(_vinContainers, lot);

                //    await SaveCustomsClearance(_customsClearances);

                //    await SavePartsInInvoice(await CreateUniquePartsInInvoiceAsync(_partsInContainer, lot, uploadedPartPrices));

                //    await SaveTracing(await CreateUniqueContainersTracingAsync(lot, _containers));
            }

            //return lot;

            throw new NotImplementedException();
        }
       
        private async Task ProcessContainersInLotAsync(ProcessesStep step, Lot lot, User currentUser)
        {
            _customsClearances = [];
            _customsParts = [];

            lot.ContainersInLots = UploadContainerTypes(_stepCollection, currentUser);

            if (step.Docmapper.FirstDataRow is not null)
            {
                int totalProgress = step.Docmapper.Data.GetLength(0) - (int)step.Docmapper.FirstDataRow;

                double currentProgress = 0.0;

                for (int row = (int)step.Docmapper.FirstDataRow; row < step.Docmapper.Data.GetLength(0); row++)
                {
                    double progress = currentProgress += 1.0;

                    ControllerDetails controller = new()
                    {
                        ProgressValue = Convert.ToDouble(progress / totalProgress),
                        Title = Resources.PleaseWait,
                        Message = string.Format(Resources.CheckingProgress, progress, totalProgress)
                    };

                    DeliveryLoadProgressUpdated?.Invoke(this, controller);

                    ContainersInLot draftContainer = await TryCreateContainer(lot, step.ValidationErrors, step.Docmapper, row);

                    ContainersInLot foundedContainer = lot.ContainersInLots.First(c => c.ContainerNumber == draftContainer.ContainerNumber);

                    foundedContainer.ContainerNumber = draftContainer.ContainerNumber;
                    foundedContainer.SealNumber = draftContainer.SealNumber;
                    foundedContainer.ContainerType = draftContainer.ContainerType;
                    foundedContainer.ContainerTypeId = draftContainer.ContainerTypeId;
                    foundedContainer.Lot = lot;

                    VinsInContainer vinsInContainer = TryCreateVinsInContainer(foundedContainer, step.ValidationErrors, step.Docmapper, row);

                    Case caseItem = await TryCreateCase(step.ValidationErrors, step.Docmapper, row);

                    PartsInContainer partInContainer = await TryCreatePartInContainerAsync(foundedContainer, vinsInContainer, caseItem, step.ValidationErrors, step.Docmapper, row);

                    foundedContainer.PartsInContainers.Add(partInContainer);

                    if (!string.IsNullOrEmpty(vinsInContainer.SupplierVinNumber))
                    {
                        if (partInContainer.PartNumber.PartType.PartType == PartTypes.Body.ToString())
                        {
                            foundedContainer.VinsInContainers.Add(vinsInContainer);
                        }
                    }

                    _customsParts.Add(partInContainer.PartNumber);

                    _customsClearances.Add(CreateCustomsClearance(foundedContainer, partInContainer));
                }
            }
        }

        private async Task<ContainersInLot> TryCreateContainer(Lot lot, Dictionary<string, CellInfo> validationErrors, Docmapper document, int? row)
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
                ContainersInLot containerWithType = lot.ContainersInLots.FirstOrDefault(c => c.ContainerNumber == container.ContainerNumber) ?? throw new Exception(string.Format(Resources.ContainerTypeMissing, container.ContainerNumber));

                container.ContainerType = await staticDataService.GetContainerTypeByName(containerWithType.ContainerType.ContainerType);

                if (container.ContainerType is null)
                {
                    throw new Exception(string.Format(Resources.ContainerTypeMissingInStaticData, containerWithType.ContainerType.ContainerType));
                }

                container.ContainerTypeId = container.ContainerType.Id;
            }

            return container;
        }

        private async Task<Case> TryCreateCase(Dictionary<string, CellInfo> validationErrors, Docmapper document, int? row)
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
                partCase.PackingType = await staticDataService.GetExistingPackingTypeByTypeAsync(partCase.PackingType.SupplierPackingType);

                if (partCase.PackingType is null)
                {
                    throw new Exception(string.Format(Resources.PackingTypeMissingInStaticData, packingType));
                }

                partCase.PackingTypeId = partCase.PackingType.Id;
            }

            return partCase;
        }

        private async Task<PartsInContainer> TryCreatePartInContainerAsync(ContainersInLot container, VinsInContainer vinsInContainer, Case caseItem, Dictionary<string, CellInfo> validationErrors, Docmapper document, int row)
        {
            PartsInContainer part = new()
            {
                ContainerInLot = container,
                ContainerInLotId = container.Id,
                PartInvoice = container.Lot.LotInvoice,
                PartInvoiceId = container.Lot.LotInvoice.Id,
                Case = caseItem,
                CaseId = caseItem.Id,
                PartNumber = new()
            };

            DocmapperContent contentPartsInContainer = document.DocmapperContents.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == typeof(PartsInContainer).GetSystemColumnName(nameof(PartsInContainer.Quantity)));

            if (!part.TrySetAndValidateProperty(nameof(PartsInContainer.Quantity), GraftValueFromRow(document, row, typeof(PartsInContainer), nameof(PartsInContainer.Quantity)), row + 1, contentPartsInContainer.ColumnNr, out Dictionary<string, CellInfo> result))
            {
                validationErrors.Merge(result);
            }

            part.PartNumber = await TryCreateCustomsPart(validationErrors, document, row);

            part.PartNumberId = part.PartNumber.PartNumberId;

            part.PartNumber.PartType = string.IsNullOrWhiteSpace(vinsInContainer.SupplierVinNumber)
                ? await staticDataService.GetPartTypeByNameAsync(PartTypes.Parts)
                : await staticDataService.GetPartTypeByNameAsync(PartTypes.Body);

            if (part.PartNumber.PartType is null)
            {
                throw new Exception(Resources.ErrorGettingTypeForPart);
            }

            part.PartNumber.PartTypeId = part.PartNumber.PartType.Id;

            return part;
        }

        private async Task<List<Tracing>> CreateTracing(Lot lot, ContainersInLot container)
        {
            List<Tracing> tracing = [];

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

            production_supply_system.EntityFramework.DAL.LotContext.Models.Location finalLocation = await staticDataService.GetFinalLocationAsync() ?? throw new Exception(Resources.FinalLocationNotFound);

            trace.TraceLocation = finalLocation;

            tracing.Add(trace);

            return tracing;
        }

        private async Task<PartsInInvoice> CreatePartInInvoiceAsync(PartsInContainer partInContainer, Lot lot, List<PartPrice> uploadedPartPrices, List<PartsInContainer> partsInContainer)
        {
            PartsInInvoice part = new()
            {
                Invoice = lot.LotInvoice,
                PartNumber = (await GetAllCustomsPartsAsync()).FirstOrDefault(p => p.PartNumber == partInContainer.PartNumber.PartNumber)
            };

            if (part.PartNumber is null)
            {
                throw new Exception(string.Format(Resources.PartByNumberNotFound, partInContainer.PartNumber.PartNumber));
            }

            PartPrice priceFromPrices = uploadedPartPrices.FirstOrDefault(pp => pp.PartNumber == part.PartNumber.PartNumber) ?? throw new Exception(string.Format(Resources.PriceByPartNumberNotFound, partInContainer.PartNumber.PartNumber));

            part.Price = priceFromPrices.Price;

            part.Quantity = CalculateTotalQuantities(partsInContainer).TryGetValue(part.PartNumber.PartNumberId, out int quantity)
                ? quantity
                : throw new Exception(string.Format(Resources.QuantityByPartNumberNotFound, partInContainer.PartNumber.PartNumber));

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
                Part newBomPart = new()
                {
                    PartNumber = customsPart.PartNumber,
                    PartName = customsPart.PartNameEng
                };

                Part bomPart = await bomService.GetExistingBomPartByPartNumberAsync(customsPart.PartNumber) ?? await bomService.SaveNewBomPartAsync(newBomPart);

                customsPart.PartNumberId = bomPart.Id;
                customsPart.DateAdd = bomPart.DateAdd;
                customsPart.HsCode = bomPart.Hscode;
            }

            return customsPart;
        }

        private async Task<List<PartsInInvoice>> CreateUniquePartsInInvoiceAsync(List<PartsInContainer> partsInContainer, Lot lot, List<PartPrice> uploadedPartPrices)
        {
            List<PartsInInvoice> partsInInvoices = [];

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
            List<Tracing> tracing = [];

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

        private void HandleValidationErrors(ProcessesStep step)
        {
            if (step.ValidationErrors.Any(s => s.Key != nameof(VinsInContainer.SupplierVinNumber)))
            {
                excelService.ColorCellsInDocument(step.ValidationErrors, step.Docmapper.SheetName, step.Docmapper.NgFolder, step.Docmapper.Folder);

                step.ValidationErrors.Clear();

                throw new Exception(string.Format(Resources.ErrorsInFile, step.Docmapper.Folder));
            }
        }

        #region Save data

        private async Task<Invoice> SaveInvoiceAsync(Invoice invoice)
        {
            return await GetExistingInvoiceAsync(invoice) is null
                ? await SaveNewInvoiceAsync(invoice)
                : throw new Exception(string.Format(Resources.InvoiceNumberHasBeenUploaded, invoice.InvoiceNumber));
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressDetailInContainer, progress, totalProgress)
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressCustomsPart, progress, totalProgress)
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressCase, progress, totalProgress)
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

        private async Task SaveContainers(Lot lot)
        {
            double totalProgress = lot.ContainersInLots.Count;

            double currentProgress = 0.0;

            foreach (ContainersInLot container in lot.ContainersInLots)
            {
                double progress = currentProgress += 1.0;

                ControllerDetails controller = new()
                {
                    ProgressValue = Convert.ToDouble(progress / totalProgress),
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressContainer, progress, totalProgress)
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
                    _ = await SaveNewContainerAsync(container);
                }
            }
        }

        private async Task SaveVinContainers(List<VinsInContainer> vinContainers, Lot lot)
        {
            //double totalProgress = vinContainers.Count;

            //double currentProgress = 0.0;

            //foreach (VinsInContainer vinContainer in vinContainers)
            //{
            //    double progress = currentProgress += 1.0;

            //    ControllerDetails controller = new()
            //    {
            //        ProgressValue = Convert.ToDouble(progress / totalProgress),
            //        Title = Resources.PleaseWait,
            //        Message = string.Format(Resources.SavingProgressVinContainer, progress, totalProgress)
            //    };

            //    DeliveryLoadProgressUpdated?.Invoke(this, controller);

            //    vinContainer.Modvar = (await GetAllModelVariantsAsync(true)).FirstOrDefault(md => md.PartNumber == vinContainer.PartNumber);

            //    if (vinContainer.ModvarId == 0)
            //    {
            //        throw new Exception(string.Format(Resources.ReleaseNotFound, vinContainer.SupplierVinNumber, vinContainer.PartNumber));
            //    }

            //    vinContainer.Lot = lot;
            //    vinContainer.ContainerInLotId = vinContainer.ContainerInLot.ContainerInLotId;

            //    _ = await GetExistingVinContainerAsync(vinContainer.SupplierVinNumber) ?? await SaveNewVinContainer(vinContainer);
            //}

            throw new NotImplementedException();
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressCustomsClearance, progress, totalProgress)
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                CustomsClearance existingCustomsClearance = await GetExistingCustomsClearanceItemAsync(сustomsClearance);

                if (existingCustomsClearance is not null)
                {
                    сustomsClearance.ContainerInLotId = сustomsClearance.ContainerInLot.Id;
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressPartInInvoice, progress, totalProgress)
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
                    Title = Resources.PleaseWait,
                    Message = string.Format(Resources.SavingProgressTracing, progress, totalProgress)
                };

                DeliveryLoadProgressUpdated?.Invoke(this, controller);

                _ = await GetExistingTraceAsync(trace) ?? await SaveNewTraceAsync(trace);
            }
        }

        #endregion Save data

        #region Get Existing data
        private static bool HasAccess(ProcessesStep step, User user)
        {
            return step.Section.Id == user.Section.Id;
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

            partInContainer.ContainerInLot = await GetContainerByIdAsync(partInContainer.ContainerInLotId);

            if (partInContainer.CaseId is not null)
            {
                partInContainer.Case = await GetCaseByIdAsync((int)partInContainer.CaseId);
            }

            partInContainer.PartNumber = await GetPartNumberByIdAsync(partInContainer.PartNumberId);

            partInContainer.PartInvoice = await GetInvoiceByIdAsync(partInContainer.PartInvoiceId);

            return partInContainer;
        }

        private async Task<PartsInInvoice> GetExistingPartInInvoiceAsync(PartsInInvoice uniquePart)
        {
            return (await GetAllPartsInInvoice()).FirstOrDefault(pi => pi.PartNumberId == uniquePart.PartNumberId && pi.InvoiceId == uniquePart.InvoiceId);
        }

        private async Task<CustomsClearance> GetExistingCustomsClearanceItemAsync(CustomsClearance customsClearance)
        {
            return (await GetAllCustomsClearanceAsync()).FirstOrDefault(cc => cc.ContainerInLotId == customsClearance.ContainerInLot.Id);
        }

        private async Task<Invoice> GetExistingInvoiceAsync(Invoice invoice)
        {
            return await lotContext.Invoices.FirstOrDefaultAsync(l => l.InvoiceNumber == invoice.InvoiceNumber);
        }

        private async Task<Lot> GetExistingLotAsync(Lot lot)
        {
            Lot existingLot = await lotContext.Lots
                .Include(l => l.Carrier)
                .Include(l => l.ContainersInLots)
                .ThenInclude(l => l.VinsInContainers)
                .Include(l => l.ContainersInLots)
                .ThenInclude(l => l.PartsInContainers)
                .ThenInclude(l => l.Case)
                .ThenInclude(l => l.PackingType)
                .Include(l => l.ContainersInLots)
                .ThenInclude(l => l.PartsInContainers)
                .ThenInclude(l => l.PartNumber)
                .ThenInclude(l => l.PartType)
                .Include(l => l.ContainersInLots)
                .ThenInclude(l => l.ContainerType)
                .Include(l => l.DeliveryTerms)
                .Include(l => l.LotArrivalLocation)
                .ThenInclude(l => l.LocationType)
                .Include(l => l.LotCustomsLocation)
                .ThenInclude(l => l.LocationType)
                .Include(l => l.LotDepartureLocation)
                .ThenInclude(l => l.LocationType)
                .Include(l => l.Shipper)
                .ThenInclude(l => l.PurchaseOrders)
                .Include(l => l.LotPurchaseOrder)
                .ThenInclude(l => l.OrderType)
                .Include(l => l.LotInvoice)
                .FirstOrDefaultAsync(l => l.LotNumber == lot.LotNumber);

            return existingLot is null ? null : existingLot;
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
        private static object GraftValueFromRow(Docmapper document, int? row, Type model, string nameOfProperty)
        {
            object result = document.GetValue(model, nameOfProperty, row);

            return result;
        }

        private static object GraftLotNumber(Lot lot, Docmapper document)
        {
            if (lot.LotNumber == Resources.LotNumberEmptySymbol || string.IsNullOrWhiteSpace(lot.LotNumber))
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
            //try
            //{
            //    CreateTraceParameters parameters = new(newTrace);

            //    logger.LogInformation($"{Resources.LogTracingAdd}: '{JsonConvert.SerializeObject(newTrace)}'");

            //    Tracing tracing = await tracingRepository.CreateAsync(newTrace, StoredProcedureInbound.AddNewTrace, parameters);

            //    logger.LogInformation($"{Resources.LogTracingAdd} {Resources.Completed}");

            //    return tracing;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTracingAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<PartsInInvoice> SaveNewPartInInvoiceAsync(PartsInInvoice newPart)
        {
            //try
            //{
            //    CreatePartInInvoiceParameters parameters = new(newPart);

            //    logger.LogInformation($"{Resources.LogPartsInInvoiceAdd}: '{JsonConvert.SerializeObject(newPart)}'");

            //    PartsInInvoice partInInvoice = await partInInvoiceRepository.CreateAsync(newPart, StoredProcedureInbound.AddNewPartInInvoice, parameters);

            //    logger.LogInformation($"{Resources.LogPartsInInvoiceAdd} {Resources.Completed}");

            //    return partInInvoice;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogPartsInInvoiceAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<PartsInContainer> SaveNewPartInContainer(PartsInContainer newPart)
        {
            //try
            //{
            //    CreatePartInContainerParameters parameters = new(newPart);

            //    logger.LogInformation($"{Resources.LogPartsInContainerAdd}: '{JsonConvert.SerializeObject(newPart)}'");

            //    PartsInContainer partInContainer = await partInContainerRepository.CreateAsync(newPart, StoredProcedureInbound.AddNewPartInContainer, parameters);

            //    logger.LogInformation($"{Resources.LogPartsInContainerAdd} {Resources.Completed}");

            //    return partInContainer;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogPartsInContainerAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<CustomsClearance> SaveNewCustomsClearance(CustomsClearance newCustomsClearance)
        {
            //try
            //{
            //    CreateCustomsClearanceParameters parameters = new(newCustomsClearance);

            //    logger.LogInformation($"{Resources.LogCustomsClearanceAdd}: '{JsonConvert.SerializeObject(newCustomsClearance)}'");

            //    CustomsClearance customsClearance = await customsClearanceRepository.CreateAsync(newCustomsClearance, StoredProcedureCustoms.AddNewCustomsClearance, parameters);

            //    logger.LogInformation($"{Resources.LogCustomsClearanceAdd} {Resources.Completed}");

            //    return customsClearance;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCustomsClearanceAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<VinsInContainer> SaveNewVinContainer(VinsInContainer newVinContainer)
        {
            //try
            //{
            //    CreateVinContainerParameters parameters = new(newVinContainer);

            //    logger.LogInformation($"{Resources.LogVinsInContainerAdd}: '{JsonConvert.SerializeObject(newVinContainer)}'");

            //    VinsInContainer vinInContainer = await vinContainerRepository.CreateAsync(newVinContainer, StoredProcedurePlanning.AddNewVinContainer, parameters);

            //    logger.LogInformation($"{Resources.LogVinsInContainerAdd} {Resources.Completed}");

            //    return vinInContainer;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogVinsInContainerAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<Invoice> SaveNewInvoiceAsync(Invoice invoice)
        {
            try
            {
                logger.LogInformation($"{Resources.LogInvoiceAdd}: '{JsonConvert.SerializeObject(invoice)}'");

                _ = await lotContext.Invoices.AddAsync(invoice);

                _ = await lotContext.SaveChangesAsync();

                logger.LogInformation($"{Resources.LogInvoiceAdd} {Resources.Completed}");

                return invoice;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogInvoiceAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task<Lot> SaveNewLotAsync(Lot newLot)
        {
            try
            {
                logger.LogInformation($"{Resources.LogLotAdd}: '{JsonConvert.SerializeObject(newLot)}'");

                lotContext.Entry(newLot).State = EntityState.Added;

                _ = await lotContext.SaveChangesAsync();

                logger.LogInformation($"{Resources.LogLotAdd} {Resources.Completed}");

                return newLot;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogLotAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task<CustomsPart> SaveNewCustomsPart(CustomsPart newCustomsPart)
        {
            //try
            //{
            //    CreateCustomsPartParameters parameters = new(newCustomsPart);

            //    logger.LogInformation($"{Resources.LogCustomsPartAdd}: '{JsonConvert.SerializeObject(newCustomsPart)}'");

            //    CustomsPart customsPart = await customsPartRepository.CreateAsync(newCustomsPart, StoredProcedureCustoms.AddNewCustomsPart, parameters);

            //    logger.LogInformation($"{Resources.LogCustomsPartAdd} {Resources.Completed}");

            //    return customsPart;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCustomsPartAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<Case> SaveNewCase(Case newCase)
        {
            //try
            //{
            //    CreateCaseParameters parameters = new(newCase);

            //    logger.LogInformation($"{Resources.LogCaseAdd}: '{JsonConvert.SerializeObject(newCase)}'");

            //    Case caseItem = await caseRepository.CreateAsync(newCase, StoredProcedureInbound.AddNewCase, parameters);

            //    logger.LogInformation($"{Resources.LogCaseAdd} {Resources.Completed}");

            //    return caseItem;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCaseAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<ContainersInLot> SaveNewContainerAsync(ContainersInLot newContainer)
        {
            try
            {
                logger.LogInformation($"{Resources.LogContainersInLotAdd}: '{JsonConvert.SerializeObject(newContainer)}'");

                lotContext.Entry(newContainer).State = EntityState.Added;

                _ = await lotContext.SaveChangesAsync();

                logger.LogInformation($"{Resources.LogContainersInLotAdd} {Resources.Completed}");

                return newContainer;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogContainersInLotAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        #endregion

        #region Getters

        /// <inheritdoc />
        public async Task<Lot> GetLotByIdAsync(int lotId)
        {
            try
            {
                logger.LogTrace($"{string.Format(Resources.LogLotGetById, lotId)}");

                Lot lot = (await GetAllLotsAsync()).FirstOrDefault(l => l.Id == lotId);

                logger.LogTrace($"{string.Format(Resources.LogLotGetById, lotId)} {Resources.Completed}");

                return lot;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogLotGetById, lotId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<List<ContainersInLot>> GetAllContainersByLotIdAsync(int lotId)
        {
            try
            {
                logger.LogTrace($"{string.Format(Resources.LogContainersInLotGetByLotId, lotId)}");

                IEnumerable<ContainersInLot> containers = (await GetAllContainersAsync())
                    .Where(c => c.LotId == lotId);

                logger.LogTrace($"{string.Format(Resources.LogContainersInLotGetByLotId, lotId)} {Resources.Completed}");

                foreach (ContainersInLot container in containers)
                {
                    container.ContainerType = await staticDataService.GetContainerTypeByIdAsync(container.ContainerTypeId);
                }

                return containers.ToList();
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogContainersInLotGetByLotId, lotId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            //try
            //{
            //    logger.LogTrace(Resources.LogPurchaseOrderGet);

            //    IEnumerable<PurchaseOrder> purchaseOrders = await purchaseOrderRepository.GetAllAsync();

            //    logger.LogInformation($"{Resources.LogPurchaseOrderGet} {Resources.Completed}");

            //    return purchaseOrders;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogPurchaseOrderGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<List<PartsInContainer>> GetPartsForContainerAsync(int containerId)
        {
            List<PartsInContainer> parts = [];

            try
            {
                logger.LogTrace($"{string.Format(Resources.LogPartsInContainerGetByContainerId, containerId)}");

                parts = (await GetAllPartsInContainer())
                    .Where(c => c.ContainerInLotId == containerId)
                    .ToList();

                logger.LogTrace($"{string.Format(Resources.LogPartsInContainerGetByContainerId, containerId)} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogPartsInContainerGetByContainerId, containerId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
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
                    part.PartNumber.PartType = await staticDataService.GetPartTypeByIdAsync((int)part.PartNumber.PartTypeId);
                }
            }

            return parts;
        }

        /// <inheritdoc />
        public async Task<int> GetquantityContainersForLotId(int lotId)
        {
            return (await GetAllContainersByLotIdAsync(lotId)).Count;
        }

        /// <inheritdoc />
        public List<PurchaseOrder> GetPurchaseOrdersForShipper(Shipper shipper)
        {
            try
            {
                logger.LogTrace($"{string.Format(Resources.LogPurchaseOrderGet, shipper.Id)}");

                List<PurchaseOrder> orders = lotContext.PurchaseOrders
                    .Include(p => p.OrderType)
                    .Where(p => p.ShipperId == shipper.Id).ToList();

                logger.LogTrace($"{string.Format(Resources.LogPurchaseOrderGet, shipper.Id)} {Resources.Completed}");

                return orders;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogPurchaseOrderGet, shipper.Id)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task<CustomsPart> GetPartNumberByIdAsync(int partNumberId)
        {
            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogCustomsPartGetById, partNumberId)}");

            //    CustomsPart part = await customsPartRepository.GetByIdAsync(partNumberId);

            //    logger.LogTrace($"{string.Format(Resources.LogCustomsPartGetById, partNumberId)} {Resources.Completed}");

            //    if (part is not null)
            //    {
            //        part.PartType = await staticDataService.GetPartTypeByIdAsync(part.PartTypeId);
            //    }

            //    return part;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogCustomsPartGetById, partNumberId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<Case> GetCaseByIdAsync(int caseId)
        {
            //Case caseItem;

            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogCaseGetById, caseId)}");

            //    caseItem = await caseRepository.GetByIdAsync(caseId);

            //    logger.LogTrace($"{string.Format(Resources.LogCaseGetById, caseId)} {Resources.Completed}");
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogCaseGetById, caseId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            //if (caseItem.PackingTypeId is not null)
            //{
            //    caseItem.PackingType = await staticDataService.GetPackingTypeByIdAsync((int)caseItem.PackingTypeId);
            //}

            //return caseItem;

            throw new NotImplementedException();
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int lotPurchaseOrderId)
        {
            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogPurchaseOrderGetById, lotPurchaseOrderId)}");

            //    PurchaseOrder purchaseOrder = await purchaseOrderRepository.GetByIdAsync(lotPurchaseOrderId);

            //    logger.LogTrace($"{string.Format(Resources.LogPurchaseOrderGetById, lotPurchaseOrderId)} {Resources.Completed}");

            //    if (purchaseOrder is not null)
            //    {
            //        purchaseOrder.Shipper = await staticDataService.GetShipperByIdAsync(purchaseOrder.ShipperId);

            //        purchaseOrder.OrderType = await staticDataService.GetPurchaseOrderTypeById(purchaseOrder.OrderTypeId);
            //    }

            //    return purchaseOrder;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogPurchaseOrderGetById, lotPurchaseOrderId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<List<Tracing>> GetAllTracingAsync()
        {
            //IEnumerable<Tracing> tracing;

            //try
            //{
            //    logger.LogTrace($"{Resources.LogTracingGet}");

            //    tracing = await tracingRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogTracingGet} {Resources.Completed}");
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTracingGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            //foreach (Tracing item in tracing)
            //{
            //    item.ContainerInLot = await GetContainerByIdAsync(item.ContainerInLotId);

            //    item.TraceLocation = await staticDataService.GetLocationByIdAsync(item.TraceLocationId);
            //}

            //return tracing?.ToList();

            throw new NotImplementedException();
        }

        private async Task<ContainersInLot> GetContainerByIdAsync(int containerInLotId)
        {
            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogContainersInLotGetById, containerInLotId)}");

            //    ContainersInLot containerInLot = await containerRepository.GetByIdAsync(containerInLotId);

            //    logger.LogTrace($"{string.Format(Resources.LogContainersInLotGetById, containerInLotId)} {Resources.Completed}");

            //    return containerInLot;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogContainersInLotGetById, containerInLotId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogInvoiceGetById, invoiceId)}");

            //    Invoice invoice = await invoiceRepository.GetByIdAsync(invoiceId);

            //    logger.LogTrace($"{string.Format(Resources.LogInvoiceGetById, invoiceId)} {Resources.Completed}");

            //    if (invoice is not null)
            //    {
            //        invoice.Shipper = await staticDataService.GetShipperByIdAsync(invoice.ShipperId);

            //        if (invoice.PurchaseOrderId is not null)
            //        {
            //            invoice.PurchaseOrder = await GetPurchaseOrderByIdAsync((int)invoice.PurchaseOrderId);
            //        }
            //    }

            //    return invoice;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogInvoiceGetById, invoiceId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<PartsInContainer>> GetAllPartsInContainer()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogPartsInContainerGet}");

            //    IEnumerable<PartsInContainer> parts = await partInContainerRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogPartsInContainerGet} {Resources.Completed}");

            //    if (parts is not null)
            //    {
            //        foreach (PartsInContainer item in parts)
            //        {
            //            item.PartNumber = await GetPartNumberByIdAsync(item.PartNumberId);
            //        }
            //    }

            //    return parts;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogPartsInContainerGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<PartsInInvoice>> GetAllPartsInInvoice()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogPartsInInvoiceGet}");

            //    IEnumerable<PartsInInvoice> partsInInvoice = await partInInvoiceRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogPartsInInvoiceGet} {Resources.Completed}");

            //    return partsInInvoice;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogPartsInInvoiceGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<CustomsClearance>> GetAllCustomsClearanceAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogCustomsClearanceGet}");

            //    IEnumerable<CustomsClearance> customsClearances = await customsClearanceRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogCustomsClearanceGet} {Resources.Completed}");

            //    return customsClearances;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCustomsClearanceGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<Invoice>> GetAllInvoiceItemsAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogInvoiceGet}");

            //    IEnumerable<Invoice> invoices = await invoiceRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogInvoiceGet} {Resources.Completed}");

            //    return invoices;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogInvoiceGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<VinsInContainer>> GetAllVinContainersAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogVinsInContainerGet}");

            //    IEnumerable<VinsInContainer> vinsInContainers = await vinContainerRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogVinsInContainerGet} {Resources.Completed}");

            //    return vinsInContainers;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogVinsInContainerGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<BodyModelVariant>> GetAllModelVariantsAsync(bool beforeRefresh)
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogBodyModelVariantGet}");

            //    IEnumerable<BodyModelVariant> bodyModelVariants = await bodyModelVariantRepository.GetAllAsync(beforeRefresh);

            //    logger.LogTrace($"{Resources.LogBodyModelVariantGet} {Resources.Completed}");

            //    return bodyModelVariants;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogBodyModelVariantGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogCaseGet}");

            //    IEnumerable<Case> cases = await caseRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogCaseGet} {Resources.Completed}");

            //    return cases;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCaseGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        public async Task<List<Lot>> GetAllLotsAsync()
        {
            try
            {
                logger.LogTrace($"{Resources.LogLotGet}");

                List<Lot> lots = await lotContext.Lots
                    .Include(l => l.Carrier)
                    .Include(l => l.ContainersInLots)
                    .ThenInclude(l => l.VinsInContainers)
                    .Include(l => l.ContainersInLots)
                    .ThenInclude(l => l.PartsInContainers)
                    .ThenInclude(l => l.Case)
                    .ThenInclude(l => l.PackingType)
                    .Include(l => l.ContainersInLots)
                    .ThenInclude(l => l.PartsInContainers)
                    .ThenInclude(l => l.PartNumber)
                    .ThenInclude(l => l.PartType)
                    .Include(l => l.ContainersInLots)
                    .ThenInclude(l => l.ContainerType)
                    .Include(l => l.DeliveryTerms)
                    .Include(l => l.LotArrivalLocation)
                    .ThenInclude(l => l.LocationType)
                    .Include(l => l.LotCustomsLocation)
                    .ThenInclude(l => l.LocationType)
                    .Include(l => l.LotDepartureLocation)
                    .ThenInclude(l => l.LocationType)
                    .Include(l => l.Shipper)
                    .ThenInclude(l => l.PurchaseOrders)
                    .Include(l => l.LotPurchaseOrder)
                    .ThenInclude(l => l.OrderType)
                    .Include(l => l.LotInvoice)
                    .ToListAsync();

                logger.LogTrace($"{Resources.LogLotGet} {Resources.Completed}");

                return lots;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogLotGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task<IEnumerable<CustomsPart>> GetAllCustomsPartsAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogCustomsPartGet}");

            //    IEnumerable<CustomsPart> customsParts = await customsPartRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogCustomsPartGet} {Resources.Completed}");

            //    return customsParts;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogCustomsPartGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<ContainersInLot>> GetAllContainersAsync()
        {
            //try
            //{
            //    logger.LogTrace($"{Resources.LogContainersInLotGet}");

            //    IEnumerable<ContainersInLot> containers = await containerRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogContainersInLotGet} {Resources.Completed}");

            //    foreach (ContainersInLot container in containers)
            //    {
            //        container.Lot = await GetLotByIdAsync(container.LotId);
            //    }

            //    return containers;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogContainersInLotGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        public async Task<List<ProcessesStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            return [.. (await processService.GetProcessStepsByUserSectionAsync(appProcess)).OrderBy(s => s.Step)];
        }

        private async Task<ContainersInLot> GetContainerInOpenLot(string containerNumber)
        {
            try
            {
                logger.LogTrace($"{string.Format(Resources.LogContainersInLotByOpenLot, containerNumber)}");

                //List<Lot> cachedLots = await lotContext.Lots.ToListAsync();

                //ContainersInLot container = await lotContext.ContainersInLots
                //    .FirstOrDefaultAsync(c => c.ContainerNumber == containerNumber &&
                //    cachedLots.Any(lot => lot.CloseDate == null && lot.Id == c.LotId));

                ContainersInLot container = await lotContext.ContainersInLots
                     .Include(c => c.Lot)
                     .Where(c => c.ContainerNumber == containerNumber && c.Lot.CloseDate == null)
                     .FirstOrDefaultAsync();

                logger.LogTrace($"{string.Format(Resources.LogContainersInLotByOpenLot, containerNumber)} {Resources.Completed}");

                return container;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogContainersInLotByOpenLot, containerNumber)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        #endregion

        #region Rollback

        //TODO:Lot-Invoice---?-Container-tracing-parts-CustomsClerance-vinContainer-Plannedsequence

        #endregion Rollback

        /// <inheritdoc />
        public event EventHandler<ControllerDetails> DeliveryLoadProgressUpdated;

        [GeneratedRegex(@"^[A-HJ-NPR-Z0-9]{17}$")]
        private static partial Regex VinRegex();
    }
}
