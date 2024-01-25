using System;

using DAL.Enums;

namespace DAL.Attributes
{
    /// <summary>
    /// Атрибут, используемый для указания схемы базы данных в перечислении.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    internal sealed class ProcedureNameAttribute : Attribute
    {
        /// <summary>
        /// Инициализирует новый экземпляр атрибута с указанием схемы базы данных.
        /// </summary>
        /// <param name="schema">Схема базы данных.</param>
        public ProcedureNameAttribute(DatabaseSchemas schema)
        {
            Schema = schema;
        }

        /// <summary>
        /// Получает схему базы данных.
        /// </summary>
        public DatabaseSchemas Schema { get; }
    }
}
