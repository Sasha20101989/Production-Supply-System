using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace UI_Interface.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с изображениями.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Создает объект BitmapImage из строки Base64.
        /// </summary>
        public static BitmapImage ImageFromString(string data)
        {
            BitmapImage image = new();
            byte[] binaryData = Convert.FromBase64String(data);
            image.BeginInit();
            image.StreamSource = new MemoryStream(binaryData);
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Создает объект BitmapImage из файла в ресурсах.
        /// </summary>
        public static BitmapImage ImageFromAssetsFile(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;

            Uri imageUri = new($"pack://application:,,,/{assemblyName};component/Assets/{fileName}");

            BitmapImage image = new(imageUri);

            return image;
        }
    }
}
