using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models.BOM;
using DAL.Parameters;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BLL.Services
{
    public class BOMService : IBOMService
    {
        private readonly IRepository<BomPart> _bomPartRepository;

        private readonly ILogger<DeliveryService> _logger;

        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public BOMService(IRepository<BomPart> bomPartRepository, ILogger<DeliveryService> logger)
        {
            _bomPartRepository = bomPartRepository;

            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BomPart>> GetAllBomPartsAsync()
        {
            try
            {
                return await _bomPartRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list parts: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения деталей из сборки материалов.");
            }
        }

        /// <inheritdoc />
        public async Task<BomPart> SaveNewBomPart(BomPart newBomPart)
        {
            CreateBomPartParameters parameters = new(newBomPart);

            try
            {
                return await _bomPartRepository.CreateAsync(newBomPart, StoredProcedureDbo.AddNewBomPart, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new part {JsonConvert.SerializeObject(newBomPart)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления детали в базу данных сборки материалов.");
            }
        }

        /// <inheritdoc />
        public async Task<BomPart> GetExistingBomPart(string partNumber)
        {
            _logger.LogInformation($"Start searching for an existing part by part number '{partNumber}'");

            IEnumerable<BomPart> parts = await GetAllBomPartsAsync();

            BomPart part = parts.FirstOrDefault(c => c.PartNumber == partNumber);

            _logger.LogInformation($"Searching for an existing part by part number '{partNumber}' completed with result: '{part}'");

            return part;
        }
    }
}
