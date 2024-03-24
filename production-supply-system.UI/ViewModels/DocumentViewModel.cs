using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о документе для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public partial class DocumentViewModel(List<Type> models, ILogger logger) : ValidatedViewModel<DocumentViewModel, List<Type>>(models, logger)
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _docmapperName;

        [ObservableProperty]
        private string? _defaultFolder;

        [ObservableProperty]
        private string _sheetName;

        [ObservableProperty]
        private int? _firstDataRow;

        [ObservableProperty]
        private bool _isActive;
    }
}
