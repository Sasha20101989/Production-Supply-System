using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Models.Docmapper;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NavigationManager.Frame.Extension.WPF;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class DocumentMapperDetailViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly INavigationManager _navigationManager;

        private readonly IDocumentService _documentService;

        private ObservableCollection<DocumentContentViewModel> _documentContent;

        private ObservableCollection<DocumentColumn> _documentColumns;

        private bool _hasErrors;

        private bool _isNewDocument;

        private bool _addNewFlyoutIsOpen;

        private DocumentContentViewModel _documentContentItem;

        private DocumentColumnViewModel _documentColumnViewModel;

        private DocumentViewModel _documentViewModel;

        public DocumentMapperDetailViewModel(IDocumentService documentService, INavigationManager navigationManager, IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            _documentService = documentService;

            _navigationManager = navigationManager;

            AddNewDocumentContentItemCommand = new RelayCommand<DocumentColumn>(AddNewDocumentContentItem, CanAddNewDocumentContentItem);

            DeleteDocumentContentItemCommand = new RelayCommand<DocumentContentViewModel>(DeleteDocumentContentItem);

            SaveDocumentCommand = new AsyncRelayCommand(SaveDocumentAsync);

            OpenAddNewFlyoutCommand = new RelayCommand(OpenAddNewFlyout);

            CloseAddNewFlyoutCommand = new RelayCommand(CloseAddNewFlyout);

            AddNewColumnCommand = new AsyncRelayCommand(AddNewColumnAsync);

            IsActiveCommand = new RelayCommand<object>(SwitchIsActive);

            DocumentColumns = new();

            AddNewFlyoutIsOpen = false;

            ValidatedModels.Add(typeof(Document));
            ValidatedModels.Add(typeof(DocumentContent));
            ValidatedModels.Add(typeof(DocumentColumn));
        }

        public List<Type> ValidatedModels { get; set; } = new();

        public RelayCommand<DocumentColumn> AddNewDocumentContentItemCommand { get; }

        public RelayCommand<DocumentContentViewModel> DeleteDocumentContentItemCommand { get; }

        public RelayCommand OpenAddNewFlyoutCommand { get; }

        public RelayCommand CloseAddNewFlyoutCommand { get; }

        public AsyncRelayCommand AddNewColumnCommand { get; }

        public AsyncRelayCommand SaveDocumentCommand { get; }

        public RelayCommand<object> IsActiveCommand { get; }

        public string PageTitle { get; private set; }

        public DocumentViewModel DocumentViewModel
        {
            get => _documentViewModel;
            set => _ = SetProperty(ref _documentViewModel, value);
        }

        public DocumentColumnViewModel DocumentColumnViewModel
        {
            get => _documentColumnViewModel;
            set => _ = SetProperty(ref _documentColumnViewModel, value);
        }

        public ObservableCollection<DocumentContentViewModel> DocumentContent
        {
            get => _documentContent;
            set => _ = SetProperty(ref _documentContent, value);
        }

        public ObservableCollection<DocumentColumn> DocumentColumns
        {
            get => _documentColumns;
            set => _ = SetProperty(ref _documentColumns, value);
        }

        public bool HasErrors
        {
            get => _hasErrors;
            set => _ = SetProperty(ref _hasErrors, value);
        }

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

        private void SwitchIsActive(object parameter)
        {
            bool isActive = DocumentViewModel.IsActive;
            DocumentViewModel.IsActive = isActive;
        }

        private async Task AddNewColumnAsync()
        {
            try
            {
                DocumentColumn documentColumn = new()
                {
                    ElementName = DocumentColumnViewModel.ElementName,
                    SystemColumnName = DocumentColumnViewModel.SystemColumnName
                };

                documentColumn = await _documentService.AddDocumentColumnAsync(documentColumn);

                DocumentColumns.Add(documentColumn);

                SortOrderColumns();

                CloseAddNewFlyout();
            }
            catch (Exception)
            {
                _ = _dialogCoordinator.ShowMessageAsync(this, $"Ошибка добавления колонки", "Колонка не добавлена в базу данных.");
                return;
            }
        }

        private void SortOrderColumns()
        {
            DocumentColumns = new ObservableCollection<DocumentColumn>(DocumentColumns.OrderBy(dc => dc.ElementName));
        }

        private DocumentViewModel CreateNewDocumentViewModel(Document document)
        {

            return document is null
                ? (new(ValidatedModels) {
                    DefaultFolder = string.Empty,
                    DocmapperName = string.Empty,
                    SheetName = string.Empty,
                })
                : (new(ValidatedModels)
                {
                    DefaultFolder = document.DefaultFolder,
                    DocmapperName = document.DocmapperName,
                    SheetName = document.SheetName,
                    FirstDataRow = document.FirstDataRow,
                    DocmapperId = document.DocmapperId,
                    IsActive = document.IsActive
                }
            );
        }

        private void OnHasErrorsUpdated(object _, bool result)
        {
            bool hasErrorsInDocument = DocumentViewModel.HasErrors;
            bool hasErrorsInContent = DocumentContent is null || !(DocumentContent.Count > 0) || DocumentContent.Any(dc => dc.HasErrors);
            HasErrors = hasErrorsInDocument || hasErrorsInContent || result;
        }

        private bool CanAddNewDocumentContentItem(DocumentColumn item)
        {
            return !DocumentContent.Any(dc => dc.DocumentColumn.DocmapperColumnId == item.DocmapperColumnId);
        }

        private async Task TryToCreateDocumentContent(object parameter)
        {
            DocumentContent = new();

            if (parameter is int mapId)
            {
                foreach (DocumentContent item in await _documentService.GetAllDocumentContentItemsByIdAsync(mapId))
                {
                    DocumentContentViewModel documentContentViewModel = new(ValidatedModels)
                    {
                        ColumnNumber = item.ColumnNumber,
                        RowNumber = item.RowNumber,
                        DocmapperColumnId = item.DocmapperColumnId,
                        DocmapperContentId = item.DocmapperContentId,
                        DocmapperId = item.DocmapperId,
                        Document = item.Document,
                        DocumentColumn = item.DocumentColumn
                    };

                    documentContentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;

                    DocumentContent.Add(documentContentViewModel);
                }
            }
        }

        private async Task CreateColumns()
        {
            try
            {
                foreach (DocumentColumn column in await _documentService.GetAllColumnsAsync())
                {
                    DocumentColumn columnViewModel = new()
                    {
                        DocmapperColumnId = column.DocmapperColumnId,
                        ElementName = column.ElementName,
                        SystemColumnName = column.SystemColumnName
                    };

                    DocumentColumns.Add(columnViewModel);
                }

                SortOrderColumns();
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task CreateDocument(object parameter)
        {
            Document document = null;

            if (parameter is int mapId)
            {
                document = await _documentService.GetDocumentByIdAsync(mapId);
            }

            DocumentViewModel = CreateNewDocumentViewModel(document);

            DocumentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;
        }

        private void AddNewDocumentContentItem(DocumentColumn item)
        {
            if (item is not null)
            {
                _documentContentItem = new(ValidatedModels)
                {
                    ColumnNumber = 0,
                    RowNumber = null,
                    DocumentColumn = item,
                };

                _documentContentItem.HasErrorsUpdated += OnHasErrorsUpdated;

                DocumentContent.Add(_documentContentItem);
            }
        }

        private void DeleteDocumentContentItem(DocumentContentViewModel item)
        {
            if (item is DocumentContentViewModel selectedItem)
            {
                _ = DocumentContent.Remove(selectedItem);
            }

            OnHasErrorsUpdated(null, false);
        }

        private void OpenAddNewFlyout()
        {
            AddNewFlyoutIsOpen = true;
            DocumentColumnViewModel = new(ValidatedModels);
        }

        private void CloseAddNewFlyout()
        {
            AddNewFlyoutIsOpen = !AddNewFlyoutIsOpen;
            DocumentColumnViewModel = null;
        }

        private async Task SaveDocumentAsync()
        {
            List<DocumentContent> content = new();

            foreach (DocumentContentViewModel documentContentItem in DocumentContent)
            {
                DocumentContent item = new()
                {
                    DocmapperId = DocumentViewModel.Document.DocmapperId,
                    ColumnNumber = documentContentItem.ColumnNumber,
                    DocmapperColumnId = documentContentItem.DocumentColumn.DocmapperColumnId,
                    DocmapperContentId = documentContentItem.DocmapperContentId,
                    Document = documentContentItem.Document,
                    RowNumber = documentContentItem.RowNumber,
                    DocumentColumn = new()
                    {
                        DocmapperColumnId = documentContentItem.DocumentColumn.DocmapperColumnId,
                        ElementName = documentContentItem.DocumentColumn.ElementName,
                        SystemColumnName = documentContentItem.DocumentColumn.SystemColumnName,
                    }
                };

                content.Add(item);
            }

            try
            {
                if (_isNewDocument)
                {
                    DocumentViewModel.Document = await _documentService.CreateDocumentAsync(DocumentViewModel.Document, content);
                }
                else
                {
                    await _documentService.UpdateDocumentAsync(DocumentViewModel.Document, content);
                }

                _ = _dialogCoordinator.ShowMessageAsync(this, $"Сохранение документа", "Документ успешно сохранён в базу данных.");
                _ = _navigationManager.NavigateTo(typeof(DocumentMapperViewModel).FullName);
            }
            catch (Exception)
            {
                _ = _dialogCoordinator.ShowMessageAsync(this, $"Ошибка сохранения документа", "Документ не сохранён.");
                return;
            }
        }
    }
}
