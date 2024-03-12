using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class ContainerCommentFilter : ICustomFilter
    {
        private readonly string _filter;

        public ContainerCommentFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ContainerComment.Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
