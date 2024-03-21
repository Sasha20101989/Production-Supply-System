using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class QuantityFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Quantity.ToString().Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
