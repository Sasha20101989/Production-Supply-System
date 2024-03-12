using System;
using DAL.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class ContainerNumberFilter : ICustomFilter
    {
        private readonly string _filter;

        public ContainerNumberFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ContainerNumber.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
