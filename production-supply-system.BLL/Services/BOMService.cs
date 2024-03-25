using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.BomContext;
using production_supply_system.EntityFramework.DAL.BomContext.Models;
using production_supply_system.EntityFramework.DAL.BomModels;
using production_supply_system.EntityFramework.DAL.Context;
using production_supply_system.EntityFramework.DAL.LotContext;

namespace BLL.Services
{
    public class BOMService(BomContext bomContext, ILogger<DeliveryService> logger) : IBOMService
    {

        /// <inheritdoc />
        public async Task<Part> SaveNewBomPartAsync(Part newBomPart)
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
        public async Task<Part> GetExistingBomPartByPartNumberAsync(string partNumber)
        {
            try
            {
                string message = string.Format(Resources.LogBomPartGetExisting, partNumber);

                logger.LogInformation(message);

                Part part = await bomContext.Parts
                    .Include(x => x.ExtColorNavigation)
                    .Include(x => x.IntColorNavigation)
                    .Include(x => x.PartType)
                    .Include(x => x.SupplierCode)
                    .FirstOrDefaultAsync(c => c.PartNumber == partNumber);

                logger.LogInformation($"{message} {Resources.Completed} {string.Format(Resources.LogWithResult, JsonConvert.SerializeObject(part))}");

                return part;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogBomPartAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }
    }
}
