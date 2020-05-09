using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClient.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
