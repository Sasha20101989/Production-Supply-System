using System;
using System.Collections.Generic;
using DAL.Models.Master;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Linq;
using BLL.Contracts;
using DAL.Models.Document;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Media;
using UI_Interface.Properties;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая шаг (Step) в мастере загрузки файлов.
    /// </summary>
    public class StepViewModel : ControlledViewModel
    {
        private readonly ILogger _logger;

        private readonly IExcelService _excelService;

        private readonly IDocumentService _documentService;

        private bool? _hasError;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StepViewModel"/>.
        /// </summary>
        /// <param name="documentService">Сервис для работы с документами.</param>
        /// <param name="excelService">Сервис для работы с Excel-файлами.</param>
        public StepViewModel(IExcelService excelService, IDocumentService documentService, ILogger logger, ProcessStep masterItem)
        {
            _logger = logger;

            _excelService = excelService;

            _documentService = documentService;

            ProcessStep = masterItem;

            SelectFileCommand = new AsyncRelayCommand<StepViewModel>(SelectFileAsync);

            _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);
        }

        /// <summary>
        /// Формирует путь к файлу с ошибками
        /// </summary>
        /// <param name="originalFilePath">Путь выбранного файла</param>
        /// <returns></returns>
        private string GetNgFolder(string originalFilePath)
        {
            _logger.LogInformation($"The beginning of the formation of the path to the file with errors based on the path: '{originalFilePath}'");

            string directory = Path.GetDirectoryName(originalFilePath);

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);

            string extension = Path.GetExtension(originalFilePath);

            string ngFileName = $"NG-{fileNameWithoutExtension}-{DateTime.Now:yyyyMMddHHmmss}{extension}";

            string ngPath = Path.Combine(directory, ngFileName);

            _logger.LogInformation($"The formation of the path to the file with errors based on the path: '{originalFilePath}' is completed. Result path: '{ngPath}' ");

            return ngPath;
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
            _logger.LogInformation($"Start of file selection.");

            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                ProcessStep.ValidationErrors.Clear();

                ProcessStep.Docmapper.Folder = openFileDialog.FileName;

                _logger.LogInformation($"The file is selected: {openFileDialog.FileName}");

                ProcessStep.Docmapper.NgFolder = GetNgFolder(openFileDialog.FileName);

                try
                {
                    await CreateController(Resources.BllValidationFileColumns);

                    await ReadAndValidateFileAsync(
                                ProcessStep.Docmapper.Folder,
                                ProcessStep.Docmapper.SheetName,
                                ProcessStep.Docmapper.Id,
                                ProcessStep.Docmapper.FirstDataRow);
                }
                catch (Exception ex)
                {
                    await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);
                }
                finally
                {
                    await ControllerPostProcess();

                    HasStepViewModelUpdated?.Invoke(this, masterItem);
                }
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
                _logger.LogInformation($"Start reading a file {folder}");

                ProcessStep.Docmapper.Data = _excelService.ReadExcelFile(folder, sheetName);

                _logger.LogInformation($"Reading a file completed.");

                if (ProcessStep.Docmapper.Data is null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка чтения файла: {ex.Message}.");
            }

            IEnumerable<DocmapperContent> content;

            try
            {
                _logger.LogInformation($"Start loading document content with id '{mapId}'");

                content = await _documentService.GetAllDocumentContentItemsByIdAsync(mapId);

                _logger.LogInformation($"Loading document content with id '{mapId}' completed, result: '{JsonConvert.SerializeObject(content)}'");

                if (content is null)
                {
                    return;
                }

                ProcessStep.Docmapper.DocmapperContents = content.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения карты документа: {ex.Message}.");
            }

            try
            {
                _logger.LogInformation($"Start validating a file headers {folder}");

                HasError = _excelService.ValidateExcelDataHeaders(ProcessStep.Docmapper.Data, firstDataRow, ProcessStep.Docmapper.DocmapperContents.ToList());

                _logger.LogInformation($"Validating a file headers {folder} completed with result: '{HasError}'");
            }
            catch (Exception)
            {
                HasError = true;

                throw;
            }
        }

        /// <summary>
        /// Событие, уведомляющее об обновлении состояния выбранного документа.
        /// </summary>
        public event EventHandler<StepViewModel> HasStepViewModelUpdated;
    }
}
