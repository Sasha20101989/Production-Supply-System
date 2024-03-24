using System.Threading.Tasks;

namespace UI_Interface.Contracts.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle();

        Task HandleAsync();
    }
}


