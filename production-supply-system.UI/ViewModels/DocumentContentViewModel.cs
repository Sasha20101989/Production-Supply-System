using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о содержимом документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="DocumentContentViewModel"/>.
    /// </remarks>
    public partial class DocumentContentViewModel(List<Type> models, ILogger logger) : ValidatedViewModel<DocumentContentViewModel, List<Type>>(models, logger)
    {
        [ObservableProperty]
        private int _docmapperContentId;

        [ObservableProperty]
        private int _docmapperColumnId;

        [ObservableProperty]
        private int _docmapperId;

        [ObservableProperty]
        private string _elementName;

        [ObservableProperty]
        private string _systemColumnName;

        [ObservableProperty]
        private int _columnNr;

        [ObservableProperty]
        private int? _rowNr;
    }
}
