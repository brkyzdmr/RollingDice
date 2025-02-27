﻿
namespace Brkyzdmr.Services.UIService
{
    public interface IUIService
    {
        Panel.PanelType GetCurrentPanelType();
        Panel.PanelType GetTopPanelOrPopupType();
        Panel.PanelType GetLastPanelType();
        void ShowPanel(Panel.PanelType panelPanelType, bool isPopup = false, bool removePreviousPanels = false);
        void AddPanel(Panel.PanelType panelType, Panel panel);

        void ActivatePanel(Panel panel);
        void DeactivatePanel(Panel panel);
    }
}