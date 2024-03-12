using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using BLL.Contracts;

using DAL.Models.Document;

using MahApps.Metro.Controls;

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;

using NavigationManager.Frame.Extension.WPF;

using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о списке документов для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// </summary>
    public class DocumentMapperViewModel : ControlledViewModel, INavigationAware
    {
        private readonly ILogger<DocumentMapperViewModel> _logger;

        private readonly INavigationManager _navigationManager;

        private readonly IDocumentService _documentService;

        private string _searchText;

        public DocumentMapperViewModel(IDocumentService documentService, INavigationManager navigationManager, ILogger<DocumentMapperViewModel> logger)
        {
            _logger = logger;

            _navigationManager = navigationManager;

            _documentService = documentService;

            NavigateToDetailCommand = new RelayCommand<object>(NavigateToDetail);

            _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);
        }

        /// <summary>
        /// Команда перенаправления к детальной информации документа
        /// </summary>
        public RelayCommand<object> NavigateToDetailCommand { get; }

        /// <summary>
        /// Получает или задает текст фильтра
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _ = SetProperty(ref _searchText, value);

                ApplySearchFilter();
            }
        }

        /// <summary>
        /// Получает или задает коллекцию документов
        /// </summary>
        public ObservableCollection<Docmapper> Source { get; } = new ObservableCollection<Docmapper>();

        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// Применяет фильтр поиска
        /// </summary>
        private async void ApplySearchFilter()
        {
            try
            {
                await CreateController(Resources.BllFilterDocuments);

                _logger.LogInformation($"Start filtering documents by search text '{SearchText}'.");

                IEnumerable<Docmapper> filteredDocuments = (await _documentService.GetAllAsync())
                        .ToList()
                        .Where(document =>
                              document.DocmapperName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                Source.Clear();

                Source.Add(new());

                foreach (Docmapper item in filteredDocuments)
                {
                    Source.Add(item);
                }

                _logger.LogInformation($"Filtering documents by search text '{SearchText}' completed.");
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

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            Source.Add(new() { });

            foreach (Docmapper item in await _documentService.GetAllAsync())
            {
                Source.Add(item);
            }
        }

        /// <summary>
        /// Перенаправляет к детальной информации документа
        /// </summary>
        private void NavigateToDetail(object parameter)
        {
            if(parameter is null)
            {
                _ = _navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                null);
            }else if (parameter is Docmapper document)
            {
                _ = _navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                document.Id);
            }
        }
    }
}
