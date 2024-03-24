using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Logging;

using UI_Interface.Properties;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// Базовая ViewModel с общими методами управления прогрессом и диалогами.
    /// </summary>
    public class ControlledViewModel(ILogger logger) : ObservableObject
    {
        protected ProgressDialogController _progressController;

        protected MetroWindow _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);

        /// <summary>
        /// Ожидает разблокировки сообщения с указанным заголовком, текстом сообщения и цветом прогресс-бара.
        /// </summary>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="color">Цвет прогресс-бара.</param>
        protected async Task WaitForMessageUnlock(string title, string message, SolidColorBrush color)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _progressController.SetTitle(title);

                _progressController.SetMessage($"{message}. {string.Format(Resources.PleasePress, Resources.ShellClose)}.");

                logger.LogError(message);

                _progressController.SetCancelable(true);

                _progressController.SetProgressBarForegroundBrush(color);

                TaskCompletionSource<bool> tcs = new();

                _progressController.Canceled += (sender, args) => tcs.SetResult(true);

                _ = await tcs.Task;
            }
        }

        /// <summary>
        /// Завершает процесс контроллера, если он открыт.
        /// </summary>
        protected async Task ControllerPostProcess()
        {
            if (_progressController is not null && _progressController.IsOpen)
            {
                await _progressController.CloseAsync();
            }
        }

        /// <summary>
        /// Создает контроллер прогресса с указанным заголовком процесса.
        /// </summary>
        /// <param name="processTitle">Заголовок процесса.</param>
        protected async Task CreateController(string processTitle)
        {
            MetroDialogSettings mySettings = new()
            {
                NegativeButtonText = Resources.ShellClose,
                AnimateShow = true,
                AnimateHide = false
            };

            _progressController = await _metroWindow.ShowProgressAsync(processTitle, Resources.ShellWait, true, mySettings);
        }
    }
}
