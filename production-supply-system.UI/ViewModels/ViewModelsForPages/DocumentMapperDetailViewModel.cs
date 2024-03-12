using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using BLL.Contracts;

using DAL.Models.Document;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;

using NavigationManager.Frame.Extension.WPF;

using Newtonsoft.Json;

using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о детальной информации документа для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ControlledViewModel для уведомлений об изменении свойств и уведомлений.
    /// </summary>
    public class DocumentMapperDetailViewModel : ControlledViewModel, INavigationAware
    {
        private readonly ILogger<DocumentMapperDetailViewModel> _logger;

        private readonly INavigationManager _navigationManager;

        private readonly IDocumentService _documentService;

        private ObservableCollection<DocumentContentViewModel> _documentContent;

        private ObservableCollection<DocmapperColumn> _documentColumns;

        private bool _hasErrors;

        private bool _isNewDocument;

        private bool _addNewFlyoutIsOpen;

        private DocumentContentViewModel _documentContentItem;

        private DocumentColumnViewModel _documentColumnViewModel;

        private DocumentViewModel _documentViewModel;

        public DocumentMapperDetailViewModel(ILogger<DocumentMapperDetailViewModel> logger, IDocumentService documentService, INavigationManager navigationManager)
        {
            _logger = logger;

            _documentService = documentService;

            _navigationManager = navigationManager;

            AddNewDocumentContentItemCommand = new RelayCommand<DocmapperColumn>(AddNewDocumentContentItem, CanAddNewDocumentContentItem);

            DeleteDocumentContentItemCommand = new RelayCommand<DocumentContentViewModel>(DeleteDocumentContentItem);

            SaveDocumentCommand = new AsyncRelayCommand(SaveDocumentAsync);

            OpenAddNewFlyoutCommand = new RelayCommand(OpenAddNewFlyout);

            CloseAddNewFlyoutCommand = new RelayCommand(CloseAddNewFlyout);

            AddNewColumnCommand = new AsyncRelayCommand(AddNewColumnAsync);

            IsActiveCommand = new RelayCommand<object>(SwitchIsActive);

            DocumentColumns = new();

            AddNewFlyoutIsOpen = false;

            _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);
        }

        /// <summary>
        /// Команда для добавления контента в документ
        /// </summary>
        public RelayCommand<DocmapperColumn> AddNewDocumentContentItemCommand { get; }

        /// <summary>
        /// Команда удаления контента из документа
        /// </summary>
        public RelayCommand<DocumentContentViewModel> DeleteDocumentContentItemCommand { get; }

        /// <summary>
        /// Команда открывающая панель для добавления нового типа контента
        /// </summary>
        public RelayCommand OpenAddNewFlyoutCommand { get; }

        /// <summary>
        /// Команда закрывающая панель для добавления нового типа контента
        /// </summary>
        public RelayCommand CloseAddNewFlyoutCommand { get; }

        /// <summary>
        /// Асинхронная команда добавления нового типа контента
        /// </summary>
        public AsyncRelayCommand AddNewColumnCommand { get; }

        /// <summary>
        /// Асинхронная команда сохранения документа
        /// </summary>
        public AsyncRelayCommand SaveDocumentCommand { get; }

        /// <summary>
        /// Команда установки активности документа
        /// </summary>
        public RelayCommand<object> IsActiveCommand { get; }

        /// <summary>
        /// получает или задает заголовок страницы в зависимости от того, это новый документ или редактируемый
        /// </summary>
        public string PageTitle { get; private set; }

        /// <summary>
        /// получает или задает список валидируемых моделей
        /// </summary>
        public List<Type> ValidatedModels { get; set; } = new()
        {
            typeof(Docmapper),
            typeof(DocmapperContent),
            typeof(DocmapperColumn)
        };

        /// <summary>
        /// получает или задает модель представления документа
        /// </summary>
        public DocumentViewModel DocumentViewModel
        {
            get => _documentViewModel;
            set => _ = SetProperty(ref _documentViewModel, value);
        }

        /// <summary>
        /// получает или задает модель представления колонки
        /// </summary>
        public DocumentColumnViewModel DocumentColumnViewModel
        {
            get => _documentColumnViewModel;
            set => _ = SetProperty(ref _documentColumnViewModel, value);
        }

        /// <summary>
        /// получает или задает список моделей представления контента документа
        /// </summary>
        public ObservableCollection<DocumentContentViewModel> DocumentContent
        {
            get => _documentContent;
            set => _ = SetProperty(ref _documentContent, value);
        }

        /// <summary>
        /// получает или задает коллекцию моделей колонок
        /// </summary>
        public ObservableCollection<DocmapperColumn> DocumentColumns
        {
            get => _documentColumns;
            set => _ = SetProperty(ref _documentColumns, value);
        }

        /// <summary>
        /// флаг отображающий наличие ошибок
        /// </summary>
        public bool HasErrors
        {
            get => _hasErrors;
            set => _ = SetProperty(ref _hasErrors, value);
        }

        /// <summary>
        /// Получает или задает значение отображающее, открыта ли панель добавления новой колонки или нет
        /// </summary>
        public bool AddNewFlyoutIsOpen
        {
            get => _addNewFlyoutIsOpen;
            set => _ = SetProperty(ref _addNewFlyoutIsOpen, value);
        }

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo(object parameter)
        {
            _isNewDocument = parameter is null;

            PageTitle = _isNewDocument ? Resources.ShellDocumentMapperNewPage : Resources.ShellDocumentMapperEditPage;

            await CreateDocument(parameter);

            await CreateColumns();

            await TryToCreateDocumentContent(parameter);
        }

        /// <summary>
        /// Переключает состояние активности документа
        /// </summary>
        /// <param name="parameter"></param>
        private void SwitchIsActive(object parameter)
        {
            bool isActive = DocumentViewModel.IsActive;

            _logger.LogInformation($"The activity status of the document has been transferred to '{isActive}'.");
        }

        /// <summary>
        /// Добавляет новую колонку
        /// </summary>
        /// <returns></returns>
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

                _logger.LogInformation($"Start adding a column '{JsonConvert.SerializeObject(documentColumn)}'.");

                documentColumn = await _documentService.AddDocumentColumnAsync(documentColumn);

                DocumentColumns.Add(documentColumn);

                _logger.LogInformation($"Adding a column completed.");

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
        /// Сортирует коллекцию колонок по имени
        /// </summary>
        private void SortOrderColumns()
        {
            _logger.LogInformation($"Start sorting a column collection.");

            DocumentColumns = new ObservableCollection<DocmapperColumn>(DocumentColumns.OrderBy(dc => dc.ElementName));

            _logger.LogInformation($"Sorting a column collection completed.");

        }

        /// <summary>
        /// Создает модель представления документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private DocumentViewModel CreateNewDocumentViewModel(Docmapper document = null)
        {

            return document is null
                ? (new(ValidatedModels, _logger) {
                    DefaultFolder = string.Empty,
                    DocmapperName = string.Empty,
                    SheetName = string.Empty,
                })
                : (new(ValidatedModels, _logger)
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

        private bool CanAddNewDocumentContentItem(DocmapperColumn item)
        {
            return !DocumentContent.Any(dc => dc.DocumentColumn.Id == item.Id);
        }

        /// <summary>
        /// Пробует создать модель представления длокумента если есть переданный уникальный идентификатор документа
        /// </summary>
        /// <param name="parameter">уникальный идентификатор документа</param>
        private async Task TryToCreateDocumentContent(object parameter)
        {
            DocumentContent = new();

            if (parameter is int mapId)
            {
                _logger.LogInformation($"The beginning of the formation of the document content.");

                foreach (DocmapperContent item in await _documentService.GetAllDocumentContentItemsByIdAsync(mapId))
                {
                    DocumentContentViewModel documentContentViewModel = new(ValidatedModels, _logger)
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

                _logger.LogInformation($"The formation of the document content has been completed: '{JsonConvert.SerializeObject(DocumentContent)}'");
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

                _logger.LogInformation($"The beginning of getting columns that can be added to the document.");

                foreach (DocmapperColumn column in await _documentService.GetAllColumnsAsync())
                {
                    DocmapperColumn columnViewModel = new()
                    {
                        Id = column.Id,
                        ElementName = column.ElementName,
                        SystemColumnName = column.SystemColumnName
                    };

                    DocumentColumns.Add(columnViewModel);
                }

                _logger.LogInformation($"Getting columns that can be added to the document is completed.");

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

                    _logger.LogInformation($"The beginning of getting the document.");

                    Docmapper document = await _documentService.GetDocumentByIdAsync(mapId);

                    _logger.LogInformation($"Getting the document is completed.");

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

        /// <summary>
        /// Добавляет новый контент в документ
        /// </summary>
        private void AddNewDocumentContentItem(DocmapperColumn item)
        {
            if (item is not null)
            {
                _documentContentItem = new(ValidatedModels, _logger)
                {
                    ColumnNumber = 0,
                    RowNumber = null,
                    DocumentColumn = item,
                };

                _documentContentItem.HasErrorsUpdated += OnHasErrorsUpdated;

                DocumentContent.Add(_documentContentItem);
            }
        }

        /// <summary>
        /// удаляет контент из документа
        /// </summary>
        /// <param name="item"></param>
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
        private void OpenAddNewFlyout()
        {
            AddNewFlyoutIsOpen = true;
            DocumentColumnViewModel = new(ValidatedModels, _logger);
        }

        /// <summary>
        /// Закрывает панель добьавления новой колонки
        /// </summary>
        private void CloseAddNewFlyout()
        {
            AddNewFlyoutIsOpen = !AddNewFlyoutIsOpen;
            DocumentColumnViewModel = null;
        }

        /// <summary>
        /// Сохраняет документ
        /// </summary>
        /// <returns></returns>
        private async Task SaveDocumentAsync()
        {
            try
            {
                List<DocmapperContent> content = new();

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

                if (_isNewDocument)
                {
                    await CreateController(Resources.BllAddNewDocument);

                    _logger.LogInformation($"Start saving a document.");

                    DocumentViewModel.Document = await _documentService.CreateDocumentAsync(DocumentViewModel.Document, content);

                    await WaitForMessageUnlock(Resources.BllAddNewDocument, Resources.BllAddNewDocumentSuccess, Brushes.IndianRed);

                    _logger.LogInformation($"Saving a document completed.");
                }
                else
                {
                    await CreateController(Resources.BllUpdateDocument);

                    _logger.LogInformation($"Start updating a document.");

                    await _documentService.UpdateDocumentAsync(DocumentViewModel.Document, content);

                    await WaitForMessageUnlock(Resources.BllUpdateDocument, Resources.BllUpdateDocumentSuccess, Brushes.IndianRed);

                    _logger.LogInformation($"Updating a document completed");
                }

                _ = _navigationManager.NavigateTo(typeof(DocumentMapperViewModel).FullName);
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
    }
}
