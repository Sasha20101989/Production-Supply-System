using System;
using DAL.Models.Master;
using System.Threading.Tasks;
using System.Linq;
using BLL.Contracts;
using MahApps.Metro.Controls;
using System.Windows.Media;
using UI_Interface.Properties;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAPICodePack.Dialogs;
using DAL.Enums;
using System.Collections.Generic;
using DAL.Models.Document;
using UI_Interface.Contracts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая шаг (Step) в мастере загрузки файлов.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="StepViewModel"/>.
    /// </remarks>
    /// <param name="documentService">Сервис для работы с документами.</param>
    /// <param name="excelService">Сервис для работы с Excel-файлами.</param>
    public partial class StepViewModel(
        IExcelService excelService, 
        ILogger logger, 
        IExportProceduresService exportProcedures,
        IToastNotificationsService toastNotificationsService, 
        ProcessStep masterItem) : ControlledViewModel(logger)
    {
        [ObservableProperty]
        private bool? _hasError;

        [ObservableProperty]
        private ProcessStep _processStep = masterItem;

        /// <summary>
        /// Обработчик команды экспорта в файл.
        /// </summary>
        /// <param name="masterItem">Экземпляр StepViewModel.</param>
        [RelayCommand]
        private async Task ExportFileAsync(StepViewModel masterItem)
        {
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ProcessStep.Docmapper.Folder = dialog.FileName;

                try
                {
                    await CreateController(Resources.ShellExportFiles);

                    await ExportToExcelFileAsync(
                                ProcessStep.Process.ProcessName,
                                ProcessStep.StepName,
                                ProcessStep.Docmapper.Folder,
                                ProcessStep.Docmapper.SheetName,
                                ProcessStep.Docmapper.DocmapperContents);
                }
                catch (Exception ex)
                {
                    await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);
                }
                finally
                {
                    await ControllerPostProcess();
                }
            }
        }

        /// <summary>
        /// Асинхронная задача экспорта данных в файл.
        /// </summary>
        /// <param name="processName">Значение перечисления процесса</param>
        /// <param name="step">Значение перечисления шага для поцесса</param>
        /// <param name="folder">Путь с названием файла и расширением</param>
        /// <param name="sheetName">Имя листа</param>
        /// <param name="content">Колонки из докмаппера</param>
        private async Task ExportToExcelFileAsync(AppProcess processName, Steps step, string folder, string sheetName, List<DocmapperContent> content)
        {
            if (processName == AppProcess.ExportFileToExcelPartner2 && step == Steps.ExportTracing)
            {
                string filePath = $"{folder}\\{Steps.ExportTracing}_{DateTime.Now.ToFileTimeUtc()}{Resources.ShellXLSX}";

                await exportProcedures.ExportTracingForPartner2(processName, step, filePath, sheetName, content);

                toastNotificationsService.ShowToastNotificationMessage(Resources.ShellExportFiles, $"{Resources.ShellExportFileCompleted}", null, null, filePath);
            }
        }

        /// <summary>
        /// Обработчик команды выбора файла.
        /// </summary>
        /// <param name="masterItem">Экземпляр StepViewModel.</param>
        [RelayCommand]
        private async Task SelectFileAsync(StepViewModel masterItem)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                ProcessStep.ValidationErrors.Clear();

                ProcessStep.Docmapper.Folder = openFileDialog.FileName;

                ProcessStep.Docmapper.NgFolder = GetNgFolder(openFileDialog.FileName);

                try
                {
                    await CreateController(Resources.BllValidationFileColumns);

                    ReadAndValidateFileAsync(
                                ProcessStep.Docmapper.Folder,
                                ProcessStep.Docmapper.SheetName,
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
        /// Формирует путь к файлу с ошибками
        /// </summary>
        /// <param name="originalFilePath">Путь выбранного файла</param>
        /// <returns></returns>
        private string GetNgFolder(string originalFilePath)
        {
            logger.LogInformation(string.Format(Resources.LogFormationNgFolder, originalFilePath));

            string directory = Path.GetDirectoryName(originalFilePath);

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);

            string extension = Path.GetExtension(originalFilePath);

            string ngFileName = string.Format(Resources.NgFileName, fileNameWithoutExtension, $"{DateTime.Now:yyyyMMddHHmmss}", extension);

            string ngPath = Path.Combine(directory, ngFileName);

            logger.LogInformation($"{string.Format(Resources.LogFormationNgFolder, originalFilePath)} {Resources.Completed} {string.Format(Resources.LogWithResult, ngPath)}");

            return ngPath;
        }

        /// <summary>
        /// Асинхронный метод чтения и валидации файла.
        /// </summary>
        /// <param name="folder">Путь к файлу.</param>
        /// <param name="sheetName">Имя листа в Excel.</param>
        /// <param name="firstDataRow">Номер первой строки с данными.</param>
        private void ReadAndValidateFileAsync(string folder, string sheetName, int firstDataRow)
        {
            try
            {
                logger.LogInformation(string.Format(Resources.LogValidatingHeaders, folder));

                ProcessStep.Docmapper.Data = excelService.ValidateExcelDataHeaders(firstDataRow, [.. ProcessStep.Docmapper.DocmapperContents], folder, sheetName);

                HasError = ProcessStep.Docmapper.Data is null;

                logger.LogInformation($"{string.Format(Resources.LogValidatingHeaders, folder)} {Resources.Completed} {string.Format(Resources.LogWithResult, HasError)}");
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
