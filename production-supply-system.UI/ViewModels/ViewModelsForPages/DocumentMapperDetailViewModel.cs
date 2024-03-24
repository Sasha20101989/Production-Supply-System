using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using NavigationManager.Frame.Extension.WPF;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

using UI_Interface.Extensions;
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
        private Docmapper _docmapper;

        [ObservableProperty]
        private bool _hasErrors;

        [ObservableProperty]
        private bool _isNewDocument;

        [ObservableProperty]
        private bool _addNewFlyoutIsOpen = false;

        [ObservableProperty]
        private ObservableCollection<DocmapperColumn> _documentColumns = [];

        [ObservableProperty]
        private ObservableCollection<DocumentContentViewModel> _documentContentViewModels = [];

        [ObservableProperty]
        private DocumentContentViewModel _documentContentItemViewModel;

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

            if (parameter is int docmapperId)
            {
                Docmapper = await documentService.GetDocumentByIdAsync(docmapperId);

                DocumentViewModel = CreateNewDocumentViewModel(Docmapper);

                DocumentContentViewModels = [];

                foreach (DocmapperContent content in Docmapper.DocmapperContents)
                {
                    DocumentContentViewModel documentContentViewModel = new(ValidatedModels, logger)
                    {
                        DocmapperContentId = content.Id,
                        DocmapperColumnId = content.DocmapperColumnId,
                        DocmapperId = content.DocmapperId,
                        ElementName = content.DocmapperColumn.ElementName,
                        SystemColumnName = content.DocmapperColumn.SystemColumnName,
                        ColumnNr = content.ColumnNr,
                        RowNr = content.RowNr
                    };

                    documentContentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;

                    DocumentContentViewModels.Add(documentContentViewModel);
                }

                DocumentViewModel.HasErrorsUpdated += OnHasErrorsUpdated;
            }
            else
            {
                Docmapper = new();

                DocumentViewModel = CreateNewDocumentViewModel();
            }

            await CreateColumns();
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

                await documentService.AddDocumentColumnAsync(documentColumn);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperColumnAdd, JsonConvert.SerializeObject(documentColumn))} {Resources.Completed}");

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

                await CreateColumns();
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
                _ = DocumentContentViewModels.Remove(selectedItem);
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
                Docmapper.DocmapperName = DocumentViewModel.DocmapperName;
                Docmapper.DefaultFolder = DocumentViewModel.DefaultFolder;
                Docmapper.SheetName = DocumentViewModel.SheetName;
                Docmapper.FirstDataRow = DocumentViewModel.FirstDataRow;
                Docmapper.IsActive = DocumentViewModel.IsActive;
                Docmapper.DocmapperContents = [];

                foreach (DocumentContentViewModel documentContentItem in DocumentContentViewModels)
                {
                    DocmapperContent item = new()
                    {
                        Id = documentContentItem.DocmapperContentId,
                        DocmapperId = documentContentItem.DocmapperId,
                        DocmapperColumnId = documentContentItem.DocmapperColumnId,
                        ColumnNr = documentContentItem.ColumnNr,
                        RowNr = documentContentItem.RowNr,
                        DocmapperColumn = new()
                        {
                            Id = documentContentItem.DocmapperColumnId,
                            ElementName = documentContentItem.ElementName,
                            SystemColumnName = documentContentItem.SystemColumnName,
                        }
                    };

                    Docmapper.DocmapperContents.Add(item);
                }

                if (IsNewDocument)
                {
                    await CreateController(Resources.BllAddNewDocument);

                    logger.LogInformation(Resources.LogAddNewDocument);

                    await documentService.CreateDocumentAsync(Docmapper);

                    await WaitForMessageUnlock(Resources.BllAddNewDocument, Resources.BllAddNewDocumentSuccess, Brushes.IndianRed);

                    logger.LogInformation($"{Resources.LogAddNewDocument} {Resources.Completed}");
                }
                else
                {
                    await CreateController(Resources.BllUpdateDocument);

                    logger.LogInformation(Resources.LogUpdateDocument);

                    await documentService.UpdateDocumentContentsAsync(Docmapper.DocmapperContents, Docmapper.Id);

                    await documentService.UpdateDocumentAsync(Docmapper);

                    await WaitForMessageUnlock(Resources.BllUpdateDocument, Resources.BllUpdateDocumentSuccess, Brushes.IndianRed);

                    logger.LogInformation($"{string.Format(Resources.LogUpdateDocument, Docmapper.Id)} {Resources.Completed}");
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
                DocumentContentItemViewModel = new(ValidatedModels, logger)
                {
                    DocmapperColumnId = item.Id,
                    DocmapperId = Docmapper.Id,
                    ElementName = item.ElementName,
                    SystemColumnName = item.SystemColumnName
                };

                DocumentContentItemViewModel.HasErrorsUpdated += OnHasErrorsUpdated;

                DocumentContentViewModels.Add(DocumentContentItemViewModel);
            }
        }

        private bool CanAddNewDocumentContentItem(DocmapperColumn item)
        {
            return DocumentContentViewModels is null || !DocumentContentViewModels.Any(dc => dc.DocmapperColumnId == item.Id);
        }

        /// <summary>
        /// Создает модель представления документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private DocumentViewModel CreateNewDocumentViewModel(Docmapper document = null)
        {
            return document is null
                ? (new(ValidatedModels, logger))
                : (new(ValidatedModels, logger)
                {
                    Id = document.Id,
                    DefaultFolder = document.DefaultFolder,
                    DocmapperName = document.DocmapperName,
                    SheetName = document.SheetName,
                    FirstDataRow = document.FirstDataRow,
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
            bool hasErrorsInContent = DocumentContentViewModels is null || !(DocumentContentViewModels.Count > 0) || DocumentContentViewModels.Any(dc => dc.HasErrors);
            HasErrors = hasErrorsInDocument || hasErrorsInContent || result;
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

                DocumentColumns.Clear();

                logger.LogInformation(Resources.LogDocmapperColumnGet);

                DocumentColumns.AddRange(await documentService.GetAllColumnsAsync());

                logger.LogInformation($"{Resources.LogDocmapperColumnGet} {Resources.Completed}");
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
