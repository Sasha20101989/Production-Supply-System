using System;
using DAL.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class PartNumberFilter : ICustomFilter
    {
        private readonly string _filter;

        public PartNumberFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.PartNumber.PartNumber.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
