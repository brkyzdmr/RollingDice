using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.UIService
{
    public class PanelHandler : IPanelHandler
    {
        private readonly Dictionary<Panel.PanelType, Panel> _panels = new Dictionary<Panel.PanelType, Panel>();
        private readonly IPanelStackHandler _panelStackHandler;
        private Panel _currentPanel;
        private Panel _topPanelOrPopup;
        private Panel _lastShownPanel;

        public PanelHandler(IPanelStackHandler panelStackHandler)
        {
            _panelStackHandler = panelStackHandler;
        }

        public Panel.PanelType GetCurrentPanelType()
        {
            return _currentPanel?.panelType ?? Panel.PanelType.None;
        }

        public Panel.PanelType GetTopPanelOrPopupType()
        {
            return _topPanelOrPopup?.panelType ?? Panel.PanelType.None;
        }

        public Panel.PanelType GetLastPanelType()
        {
            return _lastShownPanel?.panelType ?? Panel.PanelType.None;
        }

        public void ShowPanel(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false)
        {
            if (removePreviousPanels) _panelStackHandler.AddToPreviousPanels(_topPanelOrPopup.panelType, isPopup, removePreviousPanels);

            var panelToShow = GetPanel(panelType);
            if (panelToShow == null || panelToShow.isNeverRemove) return;

            panelToShow.isCurrentlyPopup = panelToShow.isAlwaysPopup || isPopup;

            HandleCurrentPanel(panelToShow);
            HandleTopPanel(panelToShow, removePreviousPanels);
            ActivatePanel(panelToShow);
        }

        public void AddPanel(Panel.PanelType panelType, Panel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            _panels[panelType] = panel;
        }

        public void DeactivatePanel(Panel panel)
        {
            if (panel == null || panel.isNeverRemove) return;
            panel.gameObject.SetActive(false);
            panel.isCurrentlyOpen = false;
            if (panel.filterImage != null) panel.filterImage.SetActive(false);
        }

        public void ActivatePanel(Panel panel)
        {
            if (panel == null) return;
            panel.gameObject.SetActive(true);
            panel.isCurrentlyOpen = true;
            if (panel.filterImage != null) panel.filterImage.SetActive(true);
        }

        public Panel GetPanel(Panel.PanelType panelType)
        {
            _panels.TryGetValue(panelType, out var panel);
            return panel;
        }

        private void HandleCurrentPanel(Panel panelToShow)
        {
            if (_currentPanel == null || !panelToShow.isCurrentlyPopup)
            {
                _currentPanel = panelToShow;
            }
        }

        private void HandleTopPanel(Panel panelToShow, bool removePreviousPanels)
        {
            if (_topPanelOrPopup != null)
            {
                HandlePanelVisibility(_topPanelOrPopup, panelToShow, removePreviousPanels);
            }

            _lastShownPanel = _topPanelOrPopup;
            _topPanelOrPopup = panelToShow;
        }

        private void HandlePanelVisibility(Panel previousPanel, Panel newPanel, bool removePreviousPanels)
        {
            if ((previousPanel.isCurrentlyPopup || !newPanel.isCurrentlyPopup) && !previousPanel.isNeverRemove)
            {
                DeactivatePanel(previousPanel);
            }

            if (!removePreviousPanels && !previousPanel.isNeverRemove)
            {
                _panelStackHandler.AddToPreviousPanels(previousPanel.panelType, previousPanel.isCurrentlyPopup);
            }
        }
    }
}
