namespace Brkyzdmr.Services.UIService
{
    public interface IPanelStackHandler
    {
        void AddToPreviousPanels(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false);
        void RemoveFromPreviousPanels(Panel.PanelType panelType);
        Panel ReturnToPreviousPanel();
    }
}