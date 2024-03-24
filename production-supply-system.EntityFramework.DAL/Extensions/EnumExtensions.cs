using System.ComponentModel;
using System.Reflection;

namespace production_supply_system.EntityFramework.DAL.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Получает значение атрибута Description для элемента перечисления
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>Значение атрибута Description, или пустая строка, если атрибут отсутствует.</returns>
        public static string GetDescription(Enum e)
        {
            FieldInfo field = e.GetType().GetField(e.ToString());

            DescriptionAttribute[] descriptionAttributes =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : string.Empty;
        }
    }
}
