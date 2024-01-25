using DAL.Attributes;
using DAL.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DAL.Data.StoredProcedures
{
    /// <summary>
    /// Сопоставление хранимых процедур для разных типов перечислений.
    /// </summary>
    public static class StoredProcedures
    {
        static StoredProcedures()
        {
            IEnumerable<Type> targetEnums = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsEnum
                    && t.CustomAttributes.Any(a => a.AttributeType == typeof(ProcedureNameAttribute)));

            foreach(Type target in targetEnums)
            {
                ProcedureNameAttribute attribute = target.GetCustomAttribute<ProcedureNameAttribute>();
                Map.Add(target.UnderlyingSystemType, e => $"{attribute.Schema}.{Resource.StoredProceduresNaming}{GetDescription(e)}");
            }
        }

        /// <summary>
        /// Получает значение атрибута Description для элемента перечисления
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>Значение атрибута Description, или пустая строка, если атрибут отсутствует.</returns>
        public static string GetDescription(Enum storedProcedure)
        {
            FieldInfo field = storedProcedure.GetType().GetField(storedProcedure.ToString());

            DescriptionAttribute[] descriptionAttributes =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Сопоставление типов перечислений словаря с соответствующими именами хранимых процедур.
        /// </summary>
        public static readonly Dictionary<Type, Func<Enum, string>> Map = new();
    }
}

