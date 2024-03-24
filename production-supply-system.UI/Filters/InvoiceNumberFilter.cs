using System;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class InvoiceNumberFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.PartInvoice.InvoiceNumber.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
