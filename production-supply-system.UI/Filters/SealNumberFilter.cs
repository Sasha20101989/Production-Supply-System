using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class SealNumberFilter : ICustomFilter
    {
        private readonly string _filter;

        public SealNumberFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.SealNumber.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
