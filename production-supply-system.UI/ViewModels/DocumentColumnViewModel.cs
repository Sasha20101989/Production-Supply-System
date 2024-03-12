using System;
using System.Collections.Generic;
using DAL.Models.Document;

using Microsoft.Extensions.Logging;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о колонке контента документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public class DocumentColumnViewModel : ValidatedViewModel<DocumentColumnViewModel, List<Type>>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentColumnViewModel"/>.
        /// </summary>
        public DocumentColumnViewModel(List<Type> models, ILogger logger) : base(models, logger)
        {

        }

        /// <summary>
        /// Получает или задает колонку контента документа.
        /// </summary>
        public DocmapperColumn DocumentColumn { get; set; } = new();

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
