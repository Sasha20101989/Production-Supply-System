using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class IMOFilter : ICustomFilter
    {
        private readonly string _filter;

        public IMOFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ImoCargo.ToString().Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
