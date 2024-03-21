using System.Globalization;
using System.Threading;

namespace UI_Interface.Multilang
{
    public class TemporaryThreadCulture
    {
        public TemporaryThreadCulture(CultureInfo newCulture)
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }
    }
}
