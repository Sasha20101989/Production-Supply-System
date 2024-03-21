using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class ContainerCommentFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ContainerComment.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
