using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DAL.Attributes;
using DAL.Models.Document;

namespace BLL.Helpers
{
    /// <summary>
    /// Вспомогательный класс для валидации свойств объекта на основе определенных атрибутов.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Выполняет валидацию свойства в переданный коллекции моделей
        /// </summary>
        /// <param name="nameOfProperty">Свойство передаваемое в формате 'пример: nameOf(Name)'</param>
        /// <param name="propertyValue">Значение этого свойства</param>
        /// <param name="models">Список моделей в которых необходимо найти это свойство и провалидировать по указанным атрибутам</param>
        /// <returns>Возвращает список ошибок или null</returns>
        public static List<CustomError> ValidatePropertyInCollection(string nameOfProperty, object propertyValue, List<Type> models)
        {
            foreach (Type model in models)
            {
                PropertyInfo propertyModelInfo = model.GetProperty(nameOfProperty);

                if (propertyModelInfo is not null)
                {
                    return ValidateProperty(nameOfProperty, propertyValue, propertyModelInfo);
                }
            }

            return null;
        }
        /// <summary>
        /// Выполняет валидацию значения свойства объекта на основе определенных атрибутов.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="propertyValue">Значение свойства.</param>
        /// <param name="propertyInfo">Информация о свойстве.</param>
        /// <returns>Список строковых представлений ошибок валидации или null, если ошибок нет.</returns>
        public static List<CustomError> ValidateProperty(string propertyName, object propertyValue, PropertyInfo propertyInfo)
        {
            List<CustomError> errors = new();

            if (Attribute.IsDefined(propertyInfo, typeof(RequiredAttribute)))
            {
                if (propertyValue == null || (propertyValue is string str && string.IsNullOrWhiteSpace(str)))
                {
                    string message = GetRequiredErrorMessage(propertyInfo) is null
                                ? $"Property '{propertyName}' is required but not provided."
                                : GetRequiredErrorMessage(propertyInfo);

                    CustomError customError = new()
                    {
                        ErrorMessage = message
                    };

                    errors.Add(customError);
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
                        string message = GetMaxLengthErrorMessage(propertyInfo) is null
                                ? $"Property '{propertyInfo.Name}' exceeds the maximum length of {maxLengthAttribute.Length} characters."
                                : GetMaxLengthErrorMessage(propertyInfo);

                        CustomError customError = new()
                        {
                            ErrorMessage = message
                        };

                        errors.Add(customError);
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

                            CustomError customError = new()
                            {
                                ErrorMessage = message
                            };

                            errors.Add(customError);
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

        /// <summary>
        /// Получает сообщение об ошибке из атрибута, если оно есть.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Сообщение об ошибке или null, если сообщение не установлено.</returns>
        private static string GetRequiredErrorMessage(PropertyInfo propertyInfo)
        {
            if (propertyInfo is not null)
            {
                IEnumerable<ValidationAttribute> validationAttributes = propertyInfo.GetCustomAttributes<RequiredAttribute>(true);

                RequiredAttribute reqAttribute = validationAttributes.FirstOrDefault(attr => attr is RequiredAttribute) as RequiredAttribute;

                if (reqAttribute is not null)
                {
                    return reqAttribute.ErrorMessage;
                }
            }

            return null;
        }

        /// <summary>
        /// Получает сообщение об ошибке из атрибута, если оно есть.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Сообщение об ошибке или null, если сообщение не установлено.</returns>
        private static string GetMaxLengthErrorMessage(PropertyInfo propertyInfo)
        {
            if (propertyInfo is not null)
            {
                IEnumerable<ValidationAttribute> validationAttributes = propertyInfo.GetCustomAttributes<MaxLengthAttribute>(true);

                MaxLengthAttribute maxLengthAttribute = validationAttributes.FirstOrDefault(attr => attr is MaxLengthAttribute) as MaxLengthAttribute;

                if (maxLengthAttribute is not null)
                {
                    return maxLengthAttribute.ErrorMessage;
                }
            }

            return null;
        }
    }
}