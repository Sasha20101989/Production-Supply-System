using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class ContainerTypeFilter : ICustomFilter
    {
        private readonly string _filter;

        public ContainerTypeFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ContainerType.ContainerType.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
