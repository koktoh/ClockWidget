using System.Threading.Tasks;

namespace ClockWidget.Models.Initialization
{
    public interface IAsyncInitializable
    {
        bool IsInitialized { get; }

        Task InitializeAsync(); 
    }
}
