using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using DAL.Models.Document;

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
        private Docmapper _document;

        [ObservableProperty]
        private DocmapperColumn _documentColumn;

        [ObservableProperty]
        private DocmapperContent _documentContent = new();

        /// <summary>
        /// Возвращает или задает уникальный идентификатор контента документа.
        /// </summary>
        public int DocmapperContentId
        {
            get => DocumentContent.Id;
            set => _ = SetProperty(DocumentContent.Id, value, DocumentContent, (model, docmapperContentId) => model.Id = docmapperContentId);
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор документа.
        /// </summary>
        public int DocmapperId
        {
            get => DocumentContent.DocmapperId;
            set => _ = SetProperty(DocumentContent.DocmapperId, value, DocumentContent, (model, docmapperId) => model.DocmapperId = docmapperId);
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор колонки.
        /// </summary>
        public int DocmapperColumnId
        {
            get => DocumentContent.DocmapperColumnId;
            set => _ = SetProperty(DocumentContent.DocmapperColumnId, value, DocumentContent, (model, docmapperColumnId) => model.DocmapperColumnId = docmapperColumnId);
        }

        /// <summary>
        /// Возвращает или задает значение строки.
        /// </summary>
        public int? RowNumber
        {
            get => DocumentContent.RowNr;
            set => _ = SetProperty(DocumentContent.RowNr, value, DocumentContent, (model, row) => model.RowNr = row);
        }

        /// <summary>
        /// Возвращает или задает значение колонки.
        /// </summary>
        public int ColumnNumber
        {
            get => DocumentContent.ColumnNr;
            set => _ = SetProperty(DocumentContent.ColumnNr, value, DocumentContent, (model, col) => model.ColumnNr = col);
        }
    }
}
