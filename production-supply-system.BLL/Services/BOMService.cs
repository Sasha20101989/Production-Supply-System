using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.Properties;

using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models.BOM;
using DAL.Parameters;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BLL.Services
{
    public class BOMService(IRepository<BomPart> bomPartRepository, ILogger<DeliveryService> logger) : IBOMService
    {

        /// <inheritdoc />
        public async Task<BomPart> SaveNewBomPartAsync(BomPart newBomPart)
        {
            CreateBomPartParameters parameters = new(newBomPart);

            try
            {
                logger.LogInformation(Resources.LogBomPartAdd);

                BomPart part = await bomPartRepository.CreateAsync(newBomPart, StoredProcedureDbo.AddNewBomPart, parameters);

                logger.LogInformation($"{Resources.LogBomPartAdd} {Resources.Completed}");

                return part;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogBomPartAdd}: {JsonConvert.SerializeObject(newBomPart)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<BomPart> GetExistingBomPartByPartNumberAsync(string partNumber)
        {
            string message = string.Format(Resources.LogBomPartGetExisting, partNumber);

            logger.LogInformation(message);

            IEnumerable<BomPart> parts = await GetAllBomPartsAsync();

            BomPart part = parts.FirstOrDefault(c => c.PartNumber == partNumber);

            logger.LogInformation($"{message} {Resources.Completed} {string.Format(Resources.LogWithResult, JsonConvert.SerializeObject(part))}");

            return part;
        }

        private async Task<IEnumerable<BomPart>> GetAllBomPartsAsync()
        {
            try
            {
                logger.LogInformation(Resources.LogBomPartsGet);

                IEnumerable<BomPart> parts = await bomPartRepository.GetAllAsync();

                logger.LogInformation($"{Resources.LogBomPartsGet} {Resources.Completed}");

                return parts;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogBomPartsGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }
    }
}
