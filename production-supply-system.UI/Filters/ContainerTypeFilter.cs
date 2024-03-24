using System;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class ContainerTypeFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is ContainersInLot container && container.ContainerType.ContainerType.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
