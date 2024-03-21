using Windows.UI.Notifications;

namespace UI_Interface.Contracts
{
    public interface IToastNotificationsService
    {
        public abstract void ShowToastNotification(ToastNotification toastNotification);

        public abstract void ShowToastNotificationMessage(string header, string message, string inputFilePath = null, string ngFilePath = null, string outputPath = null);

    }
}
