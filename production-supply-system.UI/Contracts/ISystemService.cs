using System.Threading.Tasks;

namespace UI_Interface.Contracts
{
    public interface ISystemService
    {
        Task OpenExcelFile(string filePath);
        Task OpenInWebBrowser(string url);
    }
}
