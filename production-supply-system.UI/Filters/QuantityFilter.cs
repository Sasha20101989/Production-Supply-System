using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class QuantityFilter : ICustomFilter
    {
        private readonly string _filter;

        public QuantityFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Quantity.ToString().Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
