using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brkyzdmr.Services.CommandService
{
    public class CommandExecutor 
    {
        private readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();

        public void EnqueueCommand(ICommand command) 
        {
            _commandQueue.Enqueue(command);
        }

        public async Task ExecuteCommands()
        {
            while (_commandQueue.Count > 0)
            {
                var command = _commandQueue.Dequeue();
                await command.Execute(); 
            }
        }
    }
}