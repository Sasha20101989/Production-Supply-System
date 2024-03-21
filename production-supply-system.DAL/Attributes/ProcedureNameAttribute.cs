using System;

using DAL.Enums;

namespace DAL.Attributes
{
    /// <summary>
    /// Атрибут, используемый для указания схемы базы данных в перечислении.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр атрибута с указанием схемы базы данных.
    /// </remarks>
    /// <param name="schema">Схема базы данных.</param>
    [AttributeUsage(AttributeTargets.Enum)]
    internal sealed class ProcedureNameAttribute(DatabaseSchemas schema) : Attribute
    {

        /// <summary>
        /// Получает схему базы данных.
        /// </summary>
        public DatabaseSchemas Schema { get; } = schema;
    }
}
