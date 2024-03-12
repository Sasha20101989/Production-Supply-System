using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class InvoiceNumberFilter : ICustomFilter
    {
        private readonly string _filter;

        public InvoiceNumberFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.PartInvoice.InvoiceNumber.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
