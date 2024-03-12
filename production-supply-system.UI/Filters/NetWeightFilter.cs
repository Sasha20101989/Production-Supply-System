using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class NetWeightFilter : ICustomFilter
    {
        private readonly string _filter;

        public NetWeightFilter(string filter)
        {
            _filter = filter;
        }

        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Case.NetWeight.ToString().Contains(_filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
