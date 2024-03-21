using System;
using System.Collections.Generic;

using DAL.Models;

namespace DAL.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Merge(this Dictionary<string, CellInfo> destination, Dictionary<string, CellInfo> source)
        {
            ArgumentNullException.ThrowIfNull(destination);

            ArgumentNullException.ThrowIfNull(source);

            foreach (KeyValuePair<string, CellInfo> kvp in source)
            {
                if (destination.TryGetValue(kvp.Key, out CellInfo value))
                {
                    value.Errors.AddRange(kvp.Value.Errors);
                }
                else
                {
                    destination.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
