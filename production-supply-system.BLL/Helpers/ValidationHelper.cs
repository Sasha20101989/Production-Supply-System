using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DAL.Attributes;

namespace BLL.Helpers
{
    /// <summary>
    /// Вспомогательный класс для валидации свойств объекта на основе определенных атрибутов.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Выполняет валидацию значения свойства объекта на основе определенных атрибутов.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="propertyValue">Значение свойства.</param>
        /// <param name="propertyInfo">Информация о свойстве.</param>
        /// <returns>Список строковых представлений ошибок валидации или null, если ошибок нет.</returns>
        public static List<string> ValidateProperty(string propertyName, object propertyValue, PropertyInfo propertyInfo)
        {
            List<string> errors = new();

            if (Attribute.IsDefined(propertyInfo, typeof(RequiredAttribute)))
            {
                if (propertyValue == null || (propertyValue is string str && string.IsNullOrWhiteSpace(str)))
                {
                    errors.Add($"Property '{propertyName}' is required but not provided.");
                }
            }

            if (Attribute.IsDefined(propertyInfo, typeof(MaxLengthAttribute)) && propertyInfo.PropertyType == typeof(string))
            {
                MaxLengthAttribute maxLengthAttribute = (MaxLengthAttribute)propertyInfo.GetCustomAttribute(typeof(MaxLengthAttribute));

                if (propertyValue is not null)
                {
                    string maxLengthValue = propertyValue.ToString();

                    if (maxLengthValue != null && maxLengthValue.Length > maxLengthAttribute.Length)
                    {
                        errors.Add($"Property '{propertyInfo.Name}' exceeds the maximum length of {maxLengthAttribute.Length} characters.");
                    }
                }
            }

            if (Attribute.IsDefined(propertyInfo, typeof(MinAttribute)))
            {
                if (propertyValue is not null)
                {
                    if (propertyValue is int intvalue)
                    {
                        MinAttribute minAttribute = (MinAttribute)propertyInfo.GetCustomAttribute(typeof(MinAttribute));

                        if (intvalue < minAttribute.Min)
                        {
                            string message = GetMinAttributeErrorMessage(propertyInfo) is null
                                ? $"Property '{propertyInfo.Name}' must be greater than or equal to {minAttribute.Min}."
                                : GetMinAttributeErrorMessage(propertyInfo);

                            errors.Add(message);
                        }
                    }
                }
            }

            return errors.Any() ? errors : null;
        }

        /// <summary>
        /// Получает сообщение об ошибке из атрибута, если оно есть.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Сообщение об ошибке или null, если сообщение не установлено.</returns>
        private static string GetMinAttributeErrorMessage(PropertyInfo propertyInfo)
        {
            if (propertyInfo is not null)
            {
                IEnumerable<ValidationAttribute> validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>(true);

                ValidationAttribute minAttribute = validationAttributes.FirstOrDefault(attr => attr is MinAttribute) as MinAttribute;

                if (minAttribute is not null)
                {
                    return minAttribute.ErrorMessage;
                }
            }

            return null;
        }
    }
}