
using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.UIService
{
    public class UIService : Service, IUIService
    {
        private readonly IPanelHandler _panelHandler;
        private readonly IPanelStackHandler _panelStackHandler;

        public UIService()
        {
            _panelStackHandler = new PanelStackHandler();
            _panelHandler = new PanelHandler(_panelStackHandler);
        }

        public Panel.PanelType GetCurrentPanelType() => _panelHandler.GetCurrentPanelType();
        public Panel.PanelType GetTopPanelOrPopupType() => _panelHandler.GetTopPanelOrPopupType();
        public Panel.PanelType GetLastPanelType() => _panelHandler.GetLastPanelType();

        public void ShowPanel(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false)
        {
            _panelHandler.ShowPanel(panelType, isPopup, removePreviousPanels);
        }

        public void AddPanel(Panel.PanelType panelType, Panel panel)
        {
            _panelHandler.AddPanel(panelType, panel);
        }

        public void DeactivatePanel(Panel panel)
        {
            _panelHandler.DeactivatePanel(panel);
        }

        public void ActivatePanel(Panel panel)
        {
            _panelHandler.ActivatePanel(panel);
        }
    }
}