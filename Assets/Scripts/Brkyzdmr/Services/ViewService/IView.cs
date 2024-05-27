namespace Brkyzdmr.Services.ViewService
{
    public interface IView
    {
        string Id { get; set; }
        void OnSelect();
        void OnMatch();
        void OnDeselect();
        void OnDestroyed();
    }
}