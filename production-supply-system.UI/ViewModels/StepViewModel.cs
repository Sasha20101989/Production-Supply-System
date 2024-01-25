using System;
using DAL.Models.Docmapper;
using System.Collections.Generic;
using DAL.Models.Master;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Linq;
using BLL.Contracts;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая шаг (Step) в мастере загрузки файлов.
    /// </summary>
    public class StepViewModel : ObservableObject
    {
        private readonly IExcelService _excelService;

        private readonly IDocumentService _documentService;

        private bool? _hasError;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StepViewModel"/>.
        /// </summary>
        /// <param name="documentService">Сервис для работы с документами.</param>
        /// <param name="excelService">Сервис для работы с Excel-файлами.</param>
        public StepViewModel(IExcelService excelService, IDocumentService documentService, ProcessStep masterItem)
        {
            _excelService = excelService;

            _documentService = documentService;

            ProcessStep = masterItem;

            SelectFileCommand = new AsyncRelayCommand<StepViewModel>(SelectFileAsync);
        }

        /// <summary>
        /// Команда выбора файла.
        /// </summary>
        public AsyncRelayCommand<StepViewModel> SelectFileCommand { get; }

        /// <summary>
        /// Объект, представляющий шаг (Step) в мастере загрузки файлов.
        /// </summary>
        public ProcessStep ProcessStep { get; set; } = new();

        /// <summary>
        /// Флаг наличия ошибки для текущего шага.
        /// </summary>
        public bool? HasError
        {
            get => _hasError;
            set => _ = SetProperty(ref _hasError, value);
        }

        /// <summary>
        /// Обработчик команды выбора файла.
        /// </summary>
        /// <param name="masterItem">Экземпляр StepViewModel.</param>
        private async Task SelectFileAsync(StepViewModel masterItem)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                ProcessStep.Document.Folder = openFileDialog.FileName;

                await ReadAndValidateFileAsync(
                    ProcessStep.Document.Folder,
                    ProcessStep.Document.SheetName,
                    ProcessStep.Document.DocmapperId,
                    ProcessStep.Document.FirstDataRow);

                HasStepViewModelUpdated?.Invoke(this, masterItem);
            }
        }

        /// <summary>
        /// Асинхронный метод чтения и валидации файла.
        /// </summary>
        /// <param name="folder">Путь к файлу.</param>
        /// <param name="sheetName">Имя листа в Excel.</param>
        /// <param name="mapId">Идентификатор карты документа.</param>
        /// <param name="firstDataRow">Номер первой строки с данными.</param>
        private async Task ReadAndValidateFileAsync(string folder, string sheetName, int mapId, int firstDataRow)
        {
            try
            {
                ProcessStep.Document.Data = _excelService.ReadExcelFile(folder, sheetName);

                if (ProcessStep.Document.Data is null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                HasErrorUpdated?.Invoke(this, $"Ошибка чтения файла: {ex.Message}");
                return;
            }

            IEnumerable<DocumentContent> content;

            try
            {
                content = await _documentService.GetAllDocumentContentItemsByIdAsync(mapId);

                if (content is null)
                {
                    return;
                }

                ProcessStep.Document.Content = content.ToList();
            }
            catch (Exception ex)
            {
                HasErrorUpdated?.Invoke(this, $"Ошибка получения карты документа: {ex.Message}");
                return;
            }

            HasError = _excelService.ValidateExcelDataHeaders(ProcessStep.Document.Data, firstDataRow, ProcessStep.Document.Content);
        }

        /// <summary>
        /// Событие, уведомляющее об обновлении состояния выбранного документа.
        /// </summary>
        public event EventHandler<StepViewModel> HasStepViewModelUpdated;

        /// <summary>
        /// Событие, уведомляющее о наличии ошибки.
        /// </summary>
        public event EventHandler<string> HasErrorUpdated;
    }
}
