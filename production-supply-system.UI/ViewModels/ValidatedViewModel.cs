using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BLL.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel, обеспечивающий валидацию свойств с использованием атрибутов и методов вспомогательного класса ValidationHelper.
    /// Реализует интерфейс IDataErrorInfo для интеграции с WPF и обеспечения отображения ошибок в пользовательском интерфейсе.
    /// </summary>
    /// <typeparam name="TViewModel">Тип ViewModel, для которой проводится валидация.</typeparam>
    /// <typeparam name="TModelCollection">Тип коллекции моделей, используемых для валидации свойств ViewModel.</typeparam>
    public class ValidatedViewModel<TViewModel, TModelCollection> : ObservableObject, IDataErrorInfo
    {
        private bool _hasErrors;

        private readonly Dictionary<string, string> ErrorsByPropertyName = new();

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ValidatedViewModel"/> class.
        /// </summary>
        /// <param name="models">Список типов моделей, используемых для валидации свойств ViewModel.</param>
        public ValidatedViewModel(List<Type> models)
        {
            Models = models;
        }

        /// <summary>
        /// Список типов моделей, используемых для валидации свойств ViewModel.
        /// </summary>
        public List<Type> Models { get; }

        /// <summary>
        /// Возвращает строку ошибки, обобщенную для всей ViewModel.
        /// </summary>
        public string Error => string.Empty;

        /// <summary>
        /// Возвращает или задает значение, указывающее, содержит ли ViewModel ошибки.
        /// </summary>
        public bool HasErrors
        {
            get => _hasErrors;
            set => _ = SetProperty(ref _hasErrors, value);
        }

        /// <summary>
        /// Индексатор, реализующий валидацию свойств модели документа.
        /// Возвращает строковое представление ошибки валидации для указанного свойства.
        /// Обновляет состояние ошибок и вызывает событие при изменении ошибок.
        /// </summary>
        /// <param name="columnName">Имя проверяемого свойства.</param>
        /// <returns>Строковое представление ошибки валидации или null, если ошибок нет.</returns>
        public string this[string columnName]
        {
            get
            {
                List<string> errors = new();

                List<string> validatedCollection = new();

                PropertyInfo propertyViewModelInfo = typeof(TViewModel).GetProperty(columnName);

                if (propertyViewModelInfo is not null)
                {
                    object propertyValue = propertyViewModelInfo.GetValue(this);

                    foreach (Type model in Models)
                    {
                        PropertyInfo propertyModelInfo = model.GetProperty(columnName.ToString());

                        if (propertyModelInfo is not null)
                        {
                            validatedCollection = ValidationHelper.ValidateProperty(columnName, propertyValue, propertyModelInfo);
                        }
                    }

                    if (validatedCollection is not null)
                    {
                        errors.AddRange(validatedCollection);
                    }

                    if (errors.Count > 0)
                    {
                        foreach (string error in errors)
                        {
                            AddError(columnName, error);
                        }
                    }
                    else
                    {
                        ClearError(columnName);
                    }
                }

                HasErrorsUpdated?.Invoke(this, HasErrors);

                return string.Join(Environment.NewLine, errors);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Добавляет ошибку валидации для указанного свойства и обновляет состояние ошибок.
        /// </summary>
        /// <param name="propertyName">Имя свойства, к которому добавляется ошибка.</param>
        /// <param name="message">Строковое представление ошибки валидации.</param>
        private void AddError(string propertyName, string message)
        {
            if (!ErrorsByPropertyName.ContainsKey(propertyName))
            {
                ErrorsByPropertyName[propertyName] = string.Empty;
            }

            if (!ErrorsByPropertyName[propertyName].Contains(message))
            {
                ErrorsByPropertyName[propertyName] = message;
                OnErrorsChanged(propertyName);
            }

            HasErrors = ErrorsByPropertyName.Any();
        }

        /// <summary>
        /// Очищает ошибку валидации для указанного свойства и обновляет состояние ошибок.
        /// </summary>
        /// <param name="propertyName">Имя свойства, для которого очищается ошибка.</param>
        private void ClearError(string propertyName)
        {
            if (ErrorsByPropertyName.ContainsKey(propertyName))
            {
                _ = ErrorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }

            HasErrors = ErrorsByPropertyName.Any();
        }

        /// <summary>
        /// Событие, уведомляющее об изменении ошибок валидации.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Событие, уведомляющее об обновлении состояния наличия ошибок.
        /// </summary>
        public event EventHandler<bool> HasErrorsUpdated;
    }
}
