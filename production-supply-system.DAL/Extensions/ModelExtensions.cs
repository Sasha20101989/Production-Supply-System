﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace DAL.Extensions
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Возвращает системное имя атрибута ColumnAttribute для указанного свойства в заданной модели.
        /// </summary>
        /// <param name="targetModel">Модель, в которой будет производиться поиск свойства с атрибутом.</param>
        /// <param name="nameOfProperty">Имя свойства, для которого осуществляется поиск атрибута с системным именем.</param>
        /// <returns>Системное имя атрибута ColumnAttribute.</returns>
        /// <exception cref="InvalidOperationException">
        /// Вызывается, если свойство не найдено, отсутствует атрибут ColumnAttribute или тип свойства не является string.
        /// Также вызывается, если атрибут ColumnAttribute или его свойство Name равны null.
        /// </exception>
        public static string GetSystemColumnName(this Type modelType, string nameOfProperty)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

             PropertyInfo propertyModelInfo = modelType.GetProperty(nameOfProperty);

            if (propertyModelInfo != null && Attribute.IsDefined(propertyModelInfo, typeof(ColumnAttribute)))
            {
                ColumnAttribute systemColumnNameAttribute = (ColumnAttribute)propertyModelInfo.GetCustomAttribute(typeof(ColumnAttribute));

                if (systemColumnNameAttribute != null && systemColumnNameAttribute.Name != null)
                {
                    return systemColumnNameAttribute.Name;
                }
                else
                {
                    throw new InvalidOperationException($"ColumnAttribute in {nameOfProperty} or its {nameOfProperty} property is null.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Property {nameOfProperty} not found, ColumnAttribute is missing, or the property type is not string.");
            }
        }

        /// <summary>
        /// Возвращает тип модели, содержащей свойство с указанным системным именем атрибута ColumnAttribute.
        /// </summary>
        /// <param name="models">Список моделей для поиска.</param>
        /// <param name="systemColumnNameAttribute">Системное имя атрибута ColumnAttribute.</param>
        /// <returns>Тип модели, если найдена одна соответствующая модель.</returns>
        /// <exception cref="InvalidOperationException">
        /// Вызывается, если найдено более одной модели с указанным атрибутом ColumnAttribute.
        /// Также вызывается, если не найдено ни одной модели с указанным атрибутом ColumnAttribute.
        /// </exception>
        public static Type GetModelBySystemColumnName(List<Type> models, string systemColumnNameAttribute)
        {
            Type foundModel = null;

            foreach (Type modelType in models)
            {
                PropertyInfo[] properties = modelType.GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(ColumnAttribute)) && propertyInfo.PropertyType == typeof(string))
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));

                        if (columnAttribute != null && columnAttribute.Name == systemColumnNameAttribute)
                        {
                            if (foundModel == null)
                            {
                                foundModel = modelType;
                            }
                            else
                            {
                                throw new InvalidOperationException($"More than one model found with the specified ColumnAttribute '{systemColumnNameAttribute}'.");
                            }
                        }
                    }
                }
            }

            if (foundModel is null)
            {
                throw new InvalidOperationException($"No model found with the specified ColumnAttribute '{systemColumnNameAttribute}'.");
            }

            return foundModel;
        }

        /// <summary>
        /// Возвращает имя свойства в модели, содержащей свойство с указанным системным именем атрибута ColumnAttribute.
        /// </summary>
        /// <param name="models">Список моделей для поиска.</param>
        /// <param name="systemColumnNameAttribute">Системное имя атрибута ColumnAttribute.</param>

        /// <returns>Имя свойства, если найдена одна соответствующая модель.</returns>

        /// <exception cref="InvalidOperationException">

        /// Вызывается, если найдено более одной модели с указанным атрибутом ColumnAttribute.

        /// Также вызывается, если не найдено ни одной модели с указанным атрибутом ColumnAttribute.

        /// </exception>

        public static string GetPropertyNameBySystemColumnName(List<Type> models, string systemColumnNameAttribute)
        {
            Type foundModel = null;

            string foundPropertyName = null;

            foreach (Type modelType in models)
            {
                PropertyInfo[] properties = modelType.GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(ColumnAttribute)) && propertyInfo.PropertyType == typeof(string))
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));

                        if (columnAttribute != null && columnAttribute.Name == systemColumnNameAttribute)
                        {
                            if (foundModel == null)
                            {
                                foundModel = modelType;

                                foundPropertyName = propertyInfo.Name;
                            }
                            else
                            {
                                throw new InvalidOperationException($"More than one model found with the specified ColumnAttribute '{systemColumnNameAttribute}'.");
                            }
                        }
                    }
                }
            }

            if (foundModel == null)
            {
                throw new InvalidOperationException($"No model found with the specified ColumnAttribute '{systemColumnNameAttribute}'.");
            }

            return foundPropertyName;
        }

        public static void SetProperty(this object model, string propertyName, object value)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            PropertyInfo propertyInfo = model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Поле '{propertyName}' не найдено в модели данных '{model.GetType().Name}'");
            }

            Type propertyType = propertyInfo.PropertyType;

            if (propertyType == typeof(decimal) || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(decimal)))
            {
                if (value is null)
                {
                    propertyInfo.SetValue(model, value);
                }
                else if (value.ToString() == "")
                {
                    propertyInfo.SetValue(model, null);
                }
                else if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    propertyInfo.SetValue(model, null);
                }
                else
                {
                    if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
                    {
                        propertyInfo.SetValue(model, decimalValue);
                    }
                    else
                    {
                        throw new ArgumentException($"Ошибка в поле '{propertyName}', не верный формат данных.");
                    }
                }
            }
            else if (propertyType == typeof(int) || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(int)))
            {
                if (int.TryParse(value?.ToString(), out int intValue))
                {
                    propertyInfo.SetValue(model, intValue);
                }
                else
                {
                    throw new ArgumentException($"Ошибка в поле '{propertyName}', не верный формат данных.");
                }
            }
            else if (propertyType == typeof(string))
            {
                propertyInfo.SetValue(model, value?.ToString());
            }
            else if (propertyType == typeof(DateTime) || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(propertyType) == typeof(DateTime)))
            {
                if (DateTime.TryParse(value?.ToString(), out DateTime dateTimeValue))
                {
                    propertyInfo.SetValue(model, dateTimeValue);
                }
                else
                {
                    throw new ArgumentException($"Ошибка в поле '{propertyName}', не верный формат данных.");
                }
            }
            else
            {
                throw new NotImplementedException($"Convert value to '{propertyType}' for property '{propertyName}' not implemented.");
            }
        }

        public static object GetPropertyValue(this object model, string propertyName)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            PropertyInfo propertyInfo = model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {propertyName} not found in type {model.GetType().Name}");
            }

            return propertyInfo.GetValue(model);
        }
    }
}
