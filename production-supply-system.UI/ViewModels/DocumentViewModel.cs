using System;
using System.Collections.Generic;
using DAL.Models.Docmapper;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о документе для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public class DocumentViewModel : ValidatedViewModel<DocumentViewModel, List<Type>>
    {
        public DocumentViewModel(List<Type> models) : base(models)
        {

        }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        public Document Document { get; set; } = new();

        /// <summary>
        /// Получает или задает уникальный идентификатор шаблона документа.
        /// </summary>
        public int DocmapperId
        {
            get => Document.DocmapperId;
            set => _ = SetProperty(Document.DocmapperId, value, Document, (model, id) => model.DocmapperId = id);
        }

        /// <summary>
        /// Получает или задает имя шаблона документа.
        /// </summary>
        public string DocmapperName
        {
            get => Document.DocmapperName;
            set => _ = SetProperty(Document.DocmapperName, value, Document, (model, name) => model.DocmapperName = name);
        }

        /// <summary>
        /// Получает или задает путь документа.
        /// </summary>
        public string DefaultFolder
        {
            get => Document.DefaultFolder;
            set => _ = SetProperty(Document.DefaultFolder, value, Document, (model, defFolder) => model.DefaultFolder = defFolder);
        }

        /// <summary>
        /// Получает или задает имя листа с которого необходимо извлекать данные.
        /// </summary>
        public string SheetName
        {
            get => Document.SheetName;
            set => SetProperty(Document.SheetName, value, Document, (model, sheetName) => model.SheetName = sheetName);
        }

        /// <summary>
        /// Получает или задает строку с которой начинаются данные.
        /// </summary>
        public int FirstDataRow
        {
            get => Document.FirstDataRow;
            set => SetProperty(Document.FirstDataRow, value, Document, (model, firstDataRow) => model.FirstDataRow = firstDataRow);
        }

        /// <summary>
        /// Получает или задает активность документа.
        /// </summary>
        public bool IsActive
        {
            get => Document.IsActive;
            set => _ = SetProperty(Document.IsActive, value, Document, (model, isActive) => model.IsActive = isActive);
        }
    }
}
