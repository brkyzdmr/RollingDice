namespace Brkyzdmr.Tools.State
{
    public interface IState
    {
        string StateID { get; }
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
    }
}