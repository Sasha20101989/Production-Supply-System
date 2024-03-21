using System.Windows.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;

using DAL.Models;
using UI_Interface.Helpers;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о пользователе для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// </summary>
    public partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _user = new();

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        public string Name
        {
            get => User.Name;
            set => SetProperty(User.Name, value, User, (model, name) => model.Name = name);
        }

        /// <summary>
        /// Получает или задает отчество пользователя.
        /// </summary>
        public string Patronymic
        {
            get => User.Patronymic;
            set => SetProperty(User.Patronymic, value, User, (model, patronymic) => model.Patronymic = patronymic);
        }

        /// <summary>
        /// Получает или задает информацию об учетной записи пользователя.
        /// </summary>
        public string Account
        {
            get => User.Account;
            set => SetProperty(User.Account, value, User, (model, account) => model.Account = account);
        }

        /// <summary>
        /// Получает или задает фотографию пользователя в виде BitmapImage.
        /// </summary>
        public BitmapImage Photo => string.IsNullOrEmpty(User.Photo)
               ? ImageHelper.ImageFromAssetsFile(Resources.DefaultAvatar)
               : ImageHelper.ImageFromString(User.Photo);
    }
}
