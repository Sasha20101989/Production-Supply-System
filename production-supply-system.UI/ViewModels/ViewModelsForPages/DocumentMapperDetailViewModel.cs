using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DAL.Models.Document;

using MahApps.Metro.Controls;

using Microsoft.Extensions.Logging;

using NavigationManager.Frame.Extension.WPF;

using Newtonsoft.Json;

using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о детальной информации документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ControlledViewModel для уведомлений об изменении свойств и уведомлений.
    /// </summary>
    public partial class DocumentMapperDetailViewModel(
        ILogger<DocumentMapperDetailViewModel> logger, 
        IDocumentService documentService, 
        INavigationManager navigationManager) : ControlledViewModel(logger), INavigationAware
    {
        [ObservableProperty]
        private bool _hasErrors;

        [ObservableProperty]
        private bool _isNewDocument;

        [ObservableProperty]
        private bool _addNewFlyoutIsOpen = false;

        [ObservableProperty]
        private ObservableCollection<DocmapperColumn> _documentColumns = [];

        [ObservableProperty]
        private ObservableCollection<DocumentContentViewModel> _documentContent;

        [ObservableProperty]
        private DocumentContentViewModel _documentContentItem;

        [ObservableProperty]
        private DocumentColumnViewModel _documentColumnViewModel;

        [ObservableProperty]
        private DocumentViewModel _documentViewModel;

        [ObservableProperty]
        private string _pageTitle;

        /// <summary>
        /// получает или задает список валидируемых моделей
        /// </summary>
        private List<Type> ValidatedModels { get; set; } =
        [
            typeof(Docmapper),
            typeof(DocmapperContent),
            typeof(DocmapperColumn)
        ];

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo(object parameter)
        {
            IsNewDocument = parameter is null;

            PageTitle = IsNewDocument ? Resources.ShellDocumentMapperNewPage : Resources.ShellDocumentMapperEditPage;

            await CreateDocument(parameter);

            await CreateColumns();

            await TryToCreateDocumentContent(parameter);
        }

        /// <summary>
        /// Переключает состояние активности документа
        /// </summary>
        /// <param name="parameter"></param>
        [RelayCommand]
        private void SwitchIsActive(object parameter)
        {
            logger.LogInformation(string.Format(Resources.LogDocumentSwitchIsActive, DocumentViewModel.IsActive));
        }

        /// <summary>
        /// Добавляет новую колонку
        /// </summary>
        [RelayCommand]
        private async Task AddNewColumnAsync()
        {
            try
            {
                await CreateController(Resources.BllAddNewDocumentColumn);

                DocmapperColumn documentColumn = new()
                {
                    ElementName = DocumentColumnViewModel.ElementName,
                    SystemColumnName = DocumentColumnViewModel.SystemColumnName
                };

                logger.LogInformation(string.Format(Resources.LogDocmapperColumnAdd, JsonConvert.SerializeObject(documentColumn)));

                documentColumn = await documentService.AddDocumentColumnAsync(documentColumn);

                DocumentColumns.Add(documentColumn);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperColumnAdd, JsonConvert.SerializeObject(documentColumn))} {Resources.Completed}");

                SortOrderColumns();

                CloseAddNewFlyout();
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// удаляет контент из документа
        /// </summary>
        /// <param name="item"></param>
        [RelayCommand]
        private void DeleteDocumentContentItem(DocumentContentViewModel item)
        {
            if (item is DocumentContentViewModel selectedItem)
            {
                _ = DocumentContent.Remove(selectedItem);
            }

            OnHasErrorsUpdated(null, false);
        }

        /// <summary>
        /// Открывает панель добьавления новой колонки
        /// </summary>
        [RelayCommand]
        private void OpenAddNewFlyout()
        {
            AddNewFlyoutIsOpen = true;
            DocumentColumnViewModel = new(ValidatedModels, logger);
        }

        /// <summary>
        /// Закрывает панель добьавления новой колонки
        /// </summary>
        [RelayCommand]
        private void CloseAddNewFlyout()
        {
            AddNewFlyoutIsOpen = !AddNewFlyoutIsOpen;
            DocumentColumnViewModel = null;
        }

        /// <summary>
        /// Сохраняет документ
        /// </summary>
        [RelayCommand]
        private async Task SaveDocumentAsync()
        {
            try
            {
                List<DocmapperContent> content = [];

                foreach (DocumentContentViewModel documentContentItem in DocumentContent)
                {
                    DocmapperContent item = new()
                    {
                        DocmapperId = DocumentViewModel.Document.Id,
                        ColumnNr = documentContentItem.ColumnNumber,
                        DocmapperColumnId = documentContentItem.DocumentColumn.Id,
                        Id = documentContentItem.DocmapperContentId,
                        Docmapper = documentContentItem.Document,
                        RowNr = documentContentItem.RowNumber,
                        DocmapperColumn = new()
                        {
                            Id = documentContentItem.DocumentColumn.Id,
                            ElementName = documentContentItem.DocumentColumn.ElementName,
                            SystemColumnName = documentContentItem.DocumentColumn.SystemColumnName,
                        }
                    };

                    content.Add(item);
                }

                if (IsNewDocument)
                {
                    await CreateController(Resources.BllAddNewDocument);

                    logger.LogInformation(Resources.LogAddNewDocument);

                    DocumentViewModel.Document = await documentService.CreateDocumentAsync(DocumentViewModel.Document, content);

                    await WaitForMessageUnlock(Resources.BllAddNewDocument, Resources.BllAddNewDocumentSuccess, Brushes.IndianRed);

                    logger.LogInformation($"{Resources.LogAddNewDocument} {Resources.Completed}");
                }
                else
                {
                    await CreateController(Resources.BllUpdateDocument);

                    logger.LogInformation(Resources.LogUpdateDocument);

                    await documentService.UpdateDocumentAsync(DocumentViewModel.Document, content);

                    await WaitForMessageUnlock(Resources.BllUpdateDocument, Resources.BllUpdateDocumentSuccess, Brushes.IndianRed);

                    logger.LogInformation($"{string.Format(Resources.LogUpdateDocument, DocumentViewModel.Document.Id)} {Resources.Completed}");
                }

                _ = navigationManager.NavigateTo(typeof(DocumentMapperViewModel).FullName);
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Добавляет новый контент в документ
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanAddNewDocumentContentItem))]
        private void AddNewDocumentContentItem(DocmapperColumn item)
        {
            if (item is not null)
            {
                DocumentContentItem = new(ValidatedModels, logger)
                {
                    ColumnNumber = 0,
                    RowNumber = null,
                    DocumentColumn = item,
                };

                DocumentContentItem.HasErrorsUpdated += OnHasErrorsUpdated;

                DocumentContent.Add(DocumentContentItem);
            }
        }

        private bool CanAddNewDocumentContentItem(DocmapperColumn item)
        {
            return DocumentContent is null || !DocumentContent.Any(dc => dc.DocumentColumn.Id == item.Id);
        }

        /// <summary>
        /// Сортирует коллекцию колонок по имени
        /// </summary>
        private void SortOrderColumns()
        {
            DocumentColumns = new ObservableCollection<DocmapperColumn>(DocumentColumns.OrderBy(dc => dc.ElementName));
        }

        /// <summary>
        /// Создает модель представления документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private DocumentViewModel CreateNewDocumentViewModel(Docmapper document = null)
        {

            return document is null
                ? (new(ValidatedModels, logger) {
                    DefaultFolder = string.Empty,
                    DocmapperName = string.Empty,
                    SheetName = string.Empty,
                })
                : (new(ValidatedModels, logger)
                {
                    DefaultFolder = document.DefaultFolder,
                    DocmapperName = document.DocmapperName,
                    SheetName = document.SheetName,
                    FirstDataRow = document.FirstDataRow,
                    DocmapperId = document.Id,
                    IsActive = document.IsActive
                }
            );
        }

        /// <summary>
        /// Обработчик события изменения состояния ошибок в документа и контенте документа
        /// </summary>
        /// <param name="_"></param>
        /// <param name="result"></param>
        private void OnHasErrorsUpdated(object _, bool result)
        {
            bool hasErrorsInDocument = DocumentViewModel.HasErrors;
            bool hasErrorsInContent = DocumentContent is null || !(DocumentContent.Count > 0) || DocumentContent.Any(dc => dc.HasErrors);
            HasErrors = hasErrorsInDocument || hasErrorsInContent || result;
        }

        /// <summary>
        /// Пробует создать модель представления длокумента если есть переданный уникальный идентификатор документа
        /// </summary>
        /// <param name="parameter">уникальный идентификатор документа</param>
        private async Task TryToCreateDocumentContent(object parameter)
        {
            DocumentContent = [];

            if (parameter is int mapId)
            {
                logger.LogInformation(Resources.LogDocmapperContentAdd);

                foreach (DocmapperContent item in await documentService.GetAllDocumentContentItemsByIdAsync(mapId))
                {
                    DocumentContentViewModel documentContentViewModel = new(ValidatedModels, logger)
                    {
                        ColumnNumber = item.ColumnNr,
                        RowNumber = item.RowNr,
                        DocmapperColumnId = item.DocmapperColumnId,
                        DocmapperContentId = item.Id,
                        DocmapperId = item.DocmapperId,
                        Document = item.Docmapper,
                        DocumentColumn = item.DocmapperColumn
                    };

                    documentContentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;

                    DocumentContent.Add(documentContentViewModel);
                }

                logger.LogInformation($"{Resources.LogDocmapperContentAdd} {Resources.Completed}");
            }
        }

        /// <summary>
        /// Создает коллекцию колонок
        /// </summary>
        /// <returns></returns>
        private async Task CreateColumns()
        {
            try
            {
                await CreateController(Resources.BllGetAllDocumentColumns);

                logger.LogInformation(Resources.LogDocmapperColumnGet);

                foreach (DocmapperColumn column in await documentService.GetAllColumnsAsync())
                {
                    DocmapperColumn columnViewModel = new()
                    {
                        Id = column.Id,
                        ElementName = column.ElementName,
                        SystemColumnName = column.SystemColumnName
                    };

                    DocumentColumns.Add(columnViewModel);
                }

                logger.LogInformation($"{Resources.LogDocmapperColumnGet} {Resources.Completed}");

                SortOrderColumns();
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Создает документ
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task CreateDocument(object parameter)
        {
            if (parameter is int mapId)
            {
                try
                {
                    await CreateController(Resources.BllGetDocument);

                    logger.LogInformation(string.Format(Resources.LogDocmapperGetById, mapId));

                    Docmapper document = await documentService.GetDocumentByIdAsync(mapId);

                    logger.LogInformation($"{string.Format(Resources.LogDocmapperGetById, mapId)} {Resources.Completed}");

                    DocumentViewModel = CreateNewDocumentViewModel(document);

                    DocumentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;
                }
                catch (Exception ex)
                {
                    await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                    return;
                }
                finally
                {
                    await ControllerPostProcess();
                }
            }
            else
            {
                DocumentViewModel = CreateNewDocumentViewModel();
            }
        }
    }
}
