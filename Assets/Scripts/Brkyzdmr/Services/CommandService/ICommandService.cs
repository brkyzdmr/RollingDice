namespace Brkyzdmr.Services.CommandService
{
    public interface ICommandService
    {
        void ExecuteCommand(ICommand command);
        void StartProcessingCommands();
        void Undo();
    }
}