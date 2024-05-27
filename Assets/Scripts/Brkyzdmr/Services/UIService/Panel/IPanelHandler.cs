namespace Brkyzdmr.Services.UIService
{
    public interface IPanelHandler
    {
        Panel.PanelType GetCurrentPanelType();
        Panel.PanelType GetTopPanelOrPopupType();
        Panel.PanelType GetLastPanelType();
        void ShowPanel(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false);
        void AddPanel(Panel.PanelType panelType, Panel panel);
        void DeactivatePanel(Panel panel);
        void ActivatePanel(Panel panel);
    }
}