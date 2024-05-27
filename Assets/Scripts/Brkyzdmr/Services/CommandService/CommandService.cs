using System.Collections.Generic;

namespace Brkyzdmr.Services.CommandService
{
    public class CommandService : Service, ICommandService
    {
        private CommandExecutor _commandExecutor = new CommandExecutor();
        private Stack<ICommand> _undoStack = new Stack<ICommand>(); 

        public void ExecuteCommand(ICommand command)
        {
            _commandExecutor.EnqueueCommand(command);

            _undoStack.Push(command);
            StartProcessingCommands();
        }

        public async void StartProcessingCommands()
        {
            await _commandExecutor.ExecuteCommands();
        }
        
        public async void Undo() 
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();

                await command.Execute();  
            }
        }
    }
}