using System.Windows.Controls;

namespace UI_Interface.Contracts.Views
{
    /// <summary>
    /// Интерфейс представления для главного окна приложения.
    /// </summary>
    public interface IShellWindow
    {
        /// <summary>
        /// Получает фрейм навигации для главного окна.
        /// </summary>
        /// <returns>Фрейм навигации.</returns>
        Frame GetNavigationFrame();

        /// <summary>
        /// Отображает главное окно.
        /// </summary>
        void ShowWindow();

        /// <summary>
        /// Закрывает главное окно.
        /// </summary>
        void CloseWindow();
    }
}
