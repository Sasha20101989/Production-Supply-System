using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BLL.Contracts;

using Microsoft.Extensions.Logging;

using production_supply_system.EntityFramework.DAL.BomModels;
using production_supply_system.EntityFramework.DAL.Context;
using production_supply_system.EntityFramework.DAL.LotContext;

namespace BLL.Services
{
    public class BOMService(LotContext db, ILogger<DeliveryService> logger) : IBOMService
    {

        /// <inheritdoc />
        public async Task<BomPart> SaveNewBomPartAsync(BomPart newBomPart)
        {
            //CreateBomPartParameters parameters = new(newBomPart);

            //try
            //{
            //    logger.LogInformation(Resources.LogBomPartAdd);

            //    BomPart part = await bomPartRepository.CreateAsync(newBomPart, StoredProcedureDbo.AddNewBomPart, parameters);

            //    logger.LogInformation($"{Resources.LogBomPartAdd} {Resources.Completed}");

            //    return part;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogBomPartAdd}: {JsonConvert.SerializeObject(newBomPart)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<BomPart> GetExistingBomPartByPartNumberAsync(string partNumber)
        {
            //string message = string.Format(Resources.LogBomPartGetExisting, partNumber);

            //logger.LogInformation(message);

            //IEnumerable<BomPart> parts = await GetAllBomPartsAsync();

            //BomPart part = parts.FirstOrDefault(c => c.PartNumber == partNumber);

            //logger.LogInformation($"{message} {Resources.Completed} {string.Format(Resources.LogWithResult, JsonConvert.SerializeObject(part))}");

            //return part;

            throw new NotImplementedException();
        }

        private async Task<IEnumerable<BomPart>> GetAllBomPartsAsync()
        {
            //try
            //{
            //    logger.LogInformation(Resources.LogBomPartsGet);

            //    IEnumerable<BomPart> parts = await bomPartRepository.GetAllAsync();

            //    logger.LogInformation($"{Resources.LogBomPartsGet} {Resources.Completed}");

            //    return parts;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogBomPartsGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }
    }
}
