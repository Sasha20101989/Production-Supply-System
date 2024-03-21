using System;
using DAL.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class SupplierPackingTypeFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Case.PackingType.SupplierPackingType.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
