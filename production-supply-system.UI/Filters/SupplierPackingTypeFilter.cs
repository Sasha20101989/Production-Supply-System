using System;
using DAL.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class SupplierPackingTypeFilter : ICustomFilter
    {
        private readonly string _filter;

        public SupplierPackingTypeFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Case.PackingType.SupplierPackingType.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
