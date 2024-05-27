
using System.Threading.Tasks;

namespace Brkyzdmr.Services.CommandService
{
    public interface ICommand
    {
        Task Execute();
    }
}