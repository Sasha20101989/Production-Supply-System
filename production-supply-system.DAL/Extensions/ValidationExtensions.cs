using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DAL.Models;
using DAL.Models.Document;

namespace DAL.Extensions
{
    /// <summary>
    /// Расширение для валидации модели с использованием атрибутов данных.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Валидация указанного свойства модели.
        /// </summary>
        /// <example>
        /// Пример использования:
        /// <code>
        /// Lot lot = new();
        /// Dictionary&lt;string, CellInfo&gt; validationResult;
        /// if (lot.TryValidateProperty(nameof(Lot.LotNumber), out validationResult))
        /// {
        ///     // Свойство прошло валидацию успешно
        /// }
        /// else
        /// {
        ///     // Обработка ошибок валидации
        ///     foreach (var error in validationResult)
        ///     {
        ///         Console.WriteLine($"Свойство: {error.Key}, Значение: {error.Value.Value}, Ошибки: {string.Join(", ", error.Value.Errors)}");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T">Тип модели.</typeparam>
        /// <param name="instance">Экземпляр модели.</param>
        /// <param name="propertyName">Имя свойства для валидации.</param>
        /// <param name="outResult">Результат валидации.</param>
        /// <returns>Возвращает true, если валидация прошла успешно, и false в противном случае.</returns>
        public static bool TryValidateProperty(this object instance, string propertyName, int row, int col, out Dictionary<string, CellInfo> outResult)
        {
            ArgumentNullException.ThrowIfNull(instance);

            outResult = [];

            PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(instance);

                List<ValidationResult> results = [];

                ValidationContext context = new(instance) { MemberName = propertyName };

                if (!Validator.TryValidateProperty(value, context, results))
                {
                    if (results is not null)
                    {
                        CellInfo cellInfo = new()
                        {
                            Errors = [],
                            Value = instance.GetPropertyValue(propertyName)
                        };

                        foreach (ValidationResult validationResult in results)
                        {
                            CustomError customError = new()
                            {
                                ErrorMessage = validationResult.ErrorMessage,
                                Row = row,
                                Column = col
                            };

                            cellInfo.Errors.Add(customError);

                            if (outResult.TryGetValue(propertyName, out CellInfo cellValue))
                            {
                                cellValue.Errors.AddRange(cellInfo.Errors);
                            }
                            else
                            {
                                outResult.Add(propertyName, cellInfo);
                            }
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Валидация всей модели.
        /// </summary>
        /// <example>
        /// <code>
        /// IEnumerable<ValidationResult> results = lot.Validate();
        /// foreach (ValidationResult validationResult in results)
        /// {
        ///      if (validationResult != ValidationResult.Success)
        ///      {
        ///         // Обработка ошибок валидации
        ///      }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T">Тип модели.</typeparam>
        /// <param name="instance">Экземпляр модели.</param>
        /// <returns>Результаты валидации.</returns>
        public static IEnumerable<ValidationResult> Validate<T>(this T instance)
        {
            List<ValidationResult> validationResults = [];

            ValidationContext context = new(instance);

            _ = Validator.TryValidateObject(instance, context, validationResults, true);

            return validationResults;
        }

        /// <summary>
        /// Попытка установки значения и валидации свойства модели.
        /// </summary>
        /// <example>
        /// Пример использования:
        /// <code>
        /// Dictionary&lt;string, CellInfo&gt; validationResult;
        /// if (model.TryCreateAndValidateProperty(nameof(MyModel.MyProperty), "NewValue", out validationResult))
        /// {
        ///     // Свойство успешно установлено и прошло валидацию
        /// }
        /// else
        /// {
        ///     // Обработка ошибок валидации
        ///     foreach (var error in validationResult)
        ///     {
        ///         Console.WriteLine($"Свойство: {error.Key}, Значение: {error.Value.Value}, Ошибки: {string.Join(", ", error.Value.Errors)}");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="model">Экземпляр модели.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="propertyValue">Значение для установки.</param>
        /// <param name="outResult">Результат валидации.</param>
        /// <returns>Возвращает true, если валидация прошла успешно, и false в противном случае.</returns>
        public static bool TrySetAndValidateProperty(this object model, string propertyName, object propertyValue, int row, int col, out Dictionary<string, CellInfo> outResult)
        {
            outResult = [];

            model.SetProperty(propertyName, propertyValue);

            if (!model.TryValidateProperty(propertyName, row, col, out Dictionary<string, CellInfo> validationResult))
            {
                outResult = validationResult;
            }

            return outResult.Count == 0;
        }
    }
}
