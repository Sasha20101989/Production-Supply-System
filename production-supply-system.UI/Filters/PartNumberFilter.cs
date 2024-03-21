using System;
using DAL.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class PartNumberFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.PartNumber.PartNumber.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
