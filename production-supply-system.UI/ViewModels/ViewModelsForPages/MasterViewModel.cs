using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;
using DAL.Models.Master;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NavigationManager.Frame.Extension.WPF;
using UI_Interface.Models;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel для страницы "MasterPage"
    /// </summary>
    public class MasterViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly INavigationManager _navigationManager;

        private readonly IDocumentService _documentService;

        private readonly IExcelService _excelService;

        private bool _hasErrorsInCollection;

        private ObservableCollection<StepViewModel> _masterCollection;

        private object _calledViewModel;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MasterViewModel"/>.
        /// </summary>
        /// <param name="navigationManager">Менеджер навигации для перехода между страницами.</param>
        /// <param name="documentService">Сервис для работы с документами.</param>
        /// <param name="excelService">Сервис для работы с Excel-файлами.</param>
        /// <param name="dialogCoordinator">Координатор диалогов для отображения всплывающих сообщений.</param>
        public MasterViewModel(INavigationManager navigationManager, IDocumentService documentService, IExcelService excelService, IDialogCoordinator dialogCoordinator)
        {
            _navigationManager = navigationManager;

            _documentService = documentService;

            _excelService = excelService;

            _dialogCoordinator = dialogCoordinator;

            StartCommand = new(Start);

            MasterCollection = new();
        }

        /// <summary>
        /// Коллекция шагов процесса, связанная с пользовательским интерфейсом.
        /// </summary>
        public ObservableCollection<StepViewModel> MasterCollection
        {
            get => _masterCollection;
            set => _ = SetProperty(ref _masterCollection, value);
        }

        /// <summary>
        /// Флаг, указывающий, есть ли ошибки в коллекции шагов.
        /// </summary>
        public bool HasErrorsInCollection
        {
            get => _hasErrorsInCollection = HasErrors();
            set => _ = SetProperty(ref _hasErrorsInCollection, value);
        }

        /// <summary>
        /// Команда для запуска определенного действия при нажатии на кнопку "Start".
        /// </summary>
        public RelayCommand StartCommand { get; }

        /// <summary>
        /// Вызывается при переходе с этой страницы.
        /// </summary>
        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// Вызывается при переходе на эту страницу.
        /// </summary>
        /// <param name="parameter">Параметр, переданный при переходе.</param>
        public void OnNavigatedTo(object parameter)
        {
            if (parameter is MasterTransfer masterTransfer)
            {
                if (masterTransfer.Object is IEnumerable<ProcessStep> masterContent)
                {
                    _calledViewModel = masterTransfer.ViewModel;

                    foreach (ProcessStep masterItem in masterContent)
                    {
                        StepViewModel stepViewModel = new(_excelService, _documentService, masterItem);

                        stepViewModel.HasStepViewModelUpdated += OnStepViewModelUpdated;
                        stepViewModel.HasErrorUpdated += OnHasErrorUpdated;

                        MasterCollection.Add(stepViewModel);
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события, вызываемого при наличии ошибки в объекте StepViewModel.
        /// </summary>
        private void OnHasErrorUpdated(object sender, string message)
        {
            _ = _dialogCoordinator.ShowModalMessageExternal(this, "Ошибка", message);
        }

        /// <summary>
        /// Обработчик события, вызываемого при обновлении данных в объекте StepViewModel.
        /// </summary>
        private void OnStepViewModelUpdated(object sender, StepViewModel stepViewModel)
        {
            HasErrorsInCollection = HasErrors();
        }

        /// <summary>
        /// Проверяет, есть ли ошибки в коллекции шагов.
        /// </summary>
        /// <returns>True, если есть ошибки, иначе False.</returns>
        private bool HasErrors()
        {
            return !MasterCollection.All(item => item.HasError is false);
        }

        /// <summary>
        /// Запускает переход на предыдущую страницу.
        /// </summary>
        private void Start()
        {
            Type calledViewModelType = _calledViewModel?.GetType();

            if (calledViewModelType is not null)
            {
                _ = _navigationManager.NavigateTo(
                calledViewModelType.FullName,
                MasterCollection.ToList());
            }
        }
    }
}
