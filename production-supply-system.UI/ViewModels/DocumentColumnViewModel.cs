using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using DAL.Models.Document;

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
        private DocmapperColumn _documentColumn = new();

        /// <summary>
        /// Получает или задает значение названия.
        /// </summary>
        public string ElementName
        {
            get => DocumentColumn.ElementName;
            set => _ = SetProperty(DocumentColumn.ElementName, value, DocumentColumn, (model, elemName) => model.ElementName = elemName);
        }

        /// <summary>
        /// Получает или задает значение системного названия.
        /// </summary>
        public string SystemColumnName
        {
            get => DocumentColumn.SystemColumnName;
            set => _ = SetProperty(DocumentColumn.SystemColumnName, value, DocumentColumn, (model, sysName) => model.SystemColumnName = sysName);
        }
    }
}
