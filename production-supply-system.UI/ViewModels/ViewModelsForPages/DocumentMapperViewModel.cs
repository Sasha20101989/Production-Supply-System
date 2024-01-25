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

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class DocumentMapperViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly INavigationManager _navigationManager;

        private readonly IDocumentService _documentService;

        private string _searchText;

        public DocumentMapperViewModel(IDocumentService documentService, INavigationManager navigationManager, IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            _navigationManager = navigationManager;

            _documentService = documentService;

            NavigateToDetailCommand = new RelayCommand<object>(NavigateToDetail);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _ = SetProperty(ref _searchText, value);

                ApplySearchFilter();
            }
        }

        public ObservableCollection<Document> Source { get; } = new ObservableCollection<Document>();

        public RelayCommand<object> NavigateToDetailCommand { get; }

        public void OnNavigatedFrom()
        {
            //TODO: Не используется
        }

        private async void ApplySearchFilter()
        {
            try
            {
                IEnumerable<Document> filteredDocuments = (await _documentService.GetAllDocumentsAsync())
                        .ToList()
                        .Where(document =>
                              document.DocmapperName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                Source.Clear();

                Source.Add(new());

                foreach (Document item in filteredDocuments)
                {
                    Source.Add(item);
                }
            }
            catch (Exception)
            {
                _ = _dialogCoordinator.ShowMessageAsync(this, $"Ошибка фильтрации", "Фильтр не применён.");
                return;
            }
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            Source.Add(new() { });

            foreach (Document item in await _documentService.GetAllDocumentsAsync())
            {
                Source.Add(item);
            }
        }

        private void NavigateToDetail(object parameter)
        {
            if(parameter is null)
            {
                _ = _navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                null);
            }else if (parameter is Document document)
            {
                _ = _navigationManager.NavigateTo(
                typeof(DocumentMapperDetailViewModel).FullName,
                document.DocmapperId);
            }
        }
    }
}
