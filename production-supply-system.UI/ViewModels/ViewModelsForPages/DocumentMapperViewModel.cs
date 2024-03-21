using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DAL.Models.Document;

using MahApps.Metro.Controls;

using Microsoft.Extensions.Logging;

using NavigationManager.Frame.Extension.WPF;

using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о списке документов для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// </summary>
    public partial class DocumentMapperViewModel(
        IDocumentService documentService,
        INavigationManager navigationManager, 
        ILogger<DocumentMapperViewModel> logger) : ControlledViewModel(logger), INavigationAware
    {
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<Docmapper> _source = [];

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

                logger.LogInformation(string.Format(Resources.LogDocmapperFilter, SearchText));

                IEnumerable<Docmapper> filteredDocuments = (await documentService.GetAllDocumentsAsync())
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

                logger.LogInformation($"{string.Format(Resources.LogDocmapperFilter, SearchText)} {Resources.Completed}");
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

            foreach (Docmapper item in await documentService.GetAllDocumentsAsync())
            {
                Source.Add(item);
            }
        }

        /// <summary>
        /// Перенаправляет к детальной информации документа
        /// </summary>
        [RelayCommand]
        private void NavigateToDetail(object parameter)
        {
            if(parameter is null)
            {
                _ = navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                null);
            }else if (parameter is Docmapper document)
            {
                _ = navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                document.Id);
            }
        }
    }
}
