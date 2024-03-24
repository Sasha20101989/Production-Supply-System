using System.ComponentModel.DataAnnotations;

namespace production_supply_system.EntityFramework.DAL.Attributes
{
    /// <summary>
    /// Пользовательский атрибут валидации для проверки, что целочисленное свойство
    /// имеет значение больше или равное указанному минимальному значению.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="MinAttribute"/>.
    /// </remarks>
    /// <param name="min">Минимальное значение.</param>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MinAttribute(int min) : ValidationAttribute
    {
        /// <summary>
        /// Минимальное значение, которому должно соответствовать свойство.
        /// </summary>
        public int Min { get; } = min;

        /// <summary>
        /// Проверяет, соответствует ли значение свойства условиям атрибута.
        /// </summary>
        /// <param name="value">Значение свойства.</param>
        /// <returns>true, если значение допустимо; в противном случае — false.</returns>
        public override bool IsValid(object? value) => value is int intValue && intValue >= Min;

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
