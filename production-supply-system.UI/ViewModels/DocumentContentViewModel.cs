using System;
using System.Collections.Generic;
using DAL.Models.Docmapper;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о содержимом документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public class DocumentContentViewModel : ValidatedViewModel<DocumentContentViewModel, List<Type>>
    {
        private Document _document;

        private DocumentColumn _documentColumn;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentContentViewModel"/>.
        /// </summary>
        public DocumentContentViewModel(List<Type> models) : base(models)
        {

        }

        /// <summary>
        /// Возвращает или задает контент документа.
        /// </summary>
        public DocumentContent DocumentContent { get; set; } = new();

        /// <summary>
        /// Возвращает или задает уникальный идентификатор контента документа.
        /// </summary>
        public int DocmapperContentId
        {
            get => DocumentContent.DocmapperContentId;
            set => _ = SetProperty(DocumentContent.DocmapperContentId, value, DocumentContent, (model, docmapperContentId) => model.DocmapperContentId = docmapperContentId);
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
            get => DocumentContent.RowNumber;
            set => _ = SetProperty(DocumentContent.RowNumber, value, DocumentContent, (model, row) => model.RowNumber = row);
        }

        /// <summary>
        /// Возвращает или задает значение колонки.
        /// </summary>
        public int ColumnNumber
        {
            get => DocumentContent.ColumnNumber;
            set => _ = SetProperty(DocumentContent.ColumnNumber, value, DocumentContent, (model, col) => model.ColumnNumber = col);
        }

        /// <summary>
        /// Возвращает или задает значение документа.
        /// </summary>
        public Document Document
        {
            get => _document;
            set => _ = SetProperty(ref _document, value);
        }

        /// <summary>
        /// Возвращает или задает значение колонки.
        /// </summary>
        public DocumentColumn DocumentColumn
        {
            get => _documentColumn;
            set => _ = SetProperty(ref _documentColumn, value);
        }
    }
}
