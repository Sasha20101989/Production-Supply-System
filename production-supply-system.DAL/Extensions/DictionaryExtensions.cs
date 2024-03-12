using System;
using System.Collections.Generic;

using DAL.Models;

namespace DAL.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Merge(this Dictionary<string, CellInfo> destination, Dictionary<string, CellInfo> source)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (KeyValuePair<string, CellInfo> kvp in source)
            {
                if (destination.ContainsKey(kvp.Key))
                {
                    destination[kvp.Key].Errors.AddRange(kvp.Value.Errors);
                }
                else
                {
                    destination.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
