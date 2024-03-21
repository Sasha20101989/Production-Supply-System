using System;
using DAL.Models;
using UI_Interface.Contracts;

namespace UI_Interface.Filters
{
    public class CaseNumberFilter(string filter) : ICustomFilter
    {
        public bool PassesFilter(object item)
        {
            return item is PartsInContainer part && part.Case.CaseNo.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
