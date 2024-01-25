using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Attributes
{
    /// <summary>
    /// Пользовательский атрибут валидации для проверки, что целочисленное свойство
    /// имеет значение больше или равное указанному минимальному значению.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MinAttribute : ValidationAttribute
    {
        /// <summary>
        /// Минимальное значение, которому должно соответствовать свойство.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MinAttribute"/>.
        /// </summary>
        /// <param name="min">Минимальное значение.</param>
        public MinAttribute(int min)
        {
            Min = min;
        }

        /// <summary>
        /// Проверяет, соответствует ли значение свойства условиям атрибута.
        /// </summary>
        /// <param name="value">Значение свойства.</param>
        /// <returns>true, если значение допустимо; в противном случае — false.</returns>
        public override bool IsValid(object value)
        {
            return value is int intValue && intValue >= Min;
        }

        /// <summary>
        /// Форматирует сообщение об ошибке для невалидного значения свойства.
        /// </summary>
        /// <param name="name">Имя свойства.</param>
        /// <returns>Сообщение об ошибке.</returns>
        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be greater than or equal to {Min}.";
        }
    }
}
