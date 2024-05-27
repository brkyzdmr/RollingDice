
namespace Brkyzdmr.Services.UIService
{
    public interface IUIService
    {
        public void ShowPanel(Panel.PanelType panelPanelType, bool isPopup = false, bool removePreviousPanels = false);
        public void AddToPreviousPanels(Panel.PanelType panelPanelType, bool isPopup = false, bool removePreviousPanels = false);
        public void RemoveFromPreviousPanels(Panel.PanelType panelPanelTypeToRemove);
        public Panel.PanelType ReturnToPreviousPanel();
        public void AddPanel(Panel.PanelType panelPanelType, Panel panel);
    }
}