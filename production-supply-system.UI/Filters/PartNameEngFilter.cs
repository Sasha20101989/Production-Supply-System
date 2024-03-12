using DAL.Models;
using System;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class PartNameEngFilter: ICustomFilter
    {
        private readonly string _filter;

        public PartNameEngFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.PartNumber.PartNameEng.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
