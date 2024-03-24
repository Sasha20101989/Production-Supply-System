using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о колонке контента документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="DocumentColumnViewModel"/>.
    /// </remarks>
    public partial class DocumentColumnViewModel(List<Type> models, ILogger logger) : ValidatedViewModel<DocumentColumnViewModel, List<Type>>(models, logger)
    {
        [ObservableProperty]
        private string _elementName;

        [ObservableProperty]
        private string _systemColumnName;
    }
}
