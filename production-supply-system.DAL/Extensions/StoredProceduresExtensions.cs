using DAL.Attributes;
using DAL.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DAL.Extensions
{
    /// <summary>
    /// Сопоставление хранимых процедур для разных типов перечислений.
    /// </summary>
    public static class StoredProceduresExtensions
    {
        static StoredProceduresExtensions()
        {
            IEnumerable<Type> targetEnums = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsEnum
                    && t.CustomAttributes.Any(a => a.AttributeType == typeof(ProcedureNameAttribute)));

            foreach(Type target in targetEnums)
            {
                ProcedureNameAttribute attribute = target.GetCustomAttribute<ProcedureNameAttribute>();
                Map.Add(target.UnderlyingSystemType, e => $"{attribute.Schema}.{Resource.StoredProceduresNaming}{EnumExtensions.GetDescription(e)}");
            }
        }

        /// <summary>
        /// Сопоставление типов перечислений словаря с соответствующими именами хранимых процедур.
        /// </summary>
        public static readonly Dictionary<Type, Func<Enum, string>> Map = new();
    }
}

