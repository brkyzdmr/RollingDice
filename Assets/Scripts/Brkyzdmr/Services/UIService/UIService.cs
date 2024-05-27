
using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.UIService
{
    public class UIService : Service, IUIService
    {
        private readonly Dictionary<Panel.PanelType, Panel> _panels;
        private Stack<Panel> _previousPanels = new Stack<Panel>();
        private Panel _topPanelOrPopup;
        private Panel _currentPanel;
        private Panel _lastShownPanel;
        public Panel.PanelType GetCurrentPanelType() => _currentPanel?.panelType ?? Panel.PanelType.None;
        public Panel.PanelType GetTopPanelOrPopupType() => _topPanelOrPopup?.panelType ?? Panel.PanelType.None;
        public Panel.PanelType GetLastPanelType() => _lastShownPanel?.panelType ?? Panel.PanelType.None;

        public UIService()
        {
            _panels = new Dictionary<Panel.PanelType, Panel>();
        }

        public void ShowPanel(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false)
        {
            if (removePreviousPanels) _previousPanels?.Clear();

            Panel panelToShow = GetPanel(panelType);
            if (panelToShow.isNeverRemove) return;

            panelToShow.isCurrentlyPopup = panelToShow.isAlwaysPopup || isPopup;

            HandleCurrentPanel(panelToShow);
            HandleTopPanel(panelToShow, removePreviousPanels);
            ActivatePanel(panelToShow);
        }


        public void AddToPreviousPanels(Panel.PanelType panelType, bool isPopup = false, bool removePreviousPanels = false)
        {
            if (removePreviousPanels)
            {
                _previousPanels.Clear();
            }

            Panel panelToAdd = GetPanel(panelType);
            if (panelToAdd == null || panelToAdd.isNeverRemove) return;

            panelToAdd.isCurrentlyPopup = panelToAdd.isAlwaysPopup || isPopup;
            _previousPanels.Push(panelToAdd);
        }


        public void RemoveFromPreviousPanels(Panel.PanelType panelTypeToRemove)
        {
            var tempStack = new Stack<Panel>();
            while (_previousPanels.TryPop(out var panel))
            {
                if (panel.panelType != panelTypeToRemove || panel.isNeverRemove)
                    tempStack.Push(panel);
            }

            _previousPanels = tempStack;
        }


        public Panel.PanelType ReturnToPreviousPanel()
        {
            if (_previousPanels.Count == 0)
            {
                return Panel.PanelType.None;
            }

            Panel previousPanel;
            do
            {
                DeactivatePanel(_topPanelOrPopup);
                _lastShownPanel = _topPanelOrPopup;
                previousPanel = _previousPanels.Pop();
            } while (previousPanel.isNeverRemove && _previousPanels.Count > 0);

            ActivatePanel(previousPanel);
            _currentPanel = previousPanel.isCurrentlyPopup ? _currentPanel : previousPanel;
            _topPanelOrPopup = previousPanel;

            return previousPanel.panelType;
        }


        private Panel GetPanel(Panel.PanelType panelType) => _panels.TryGetValue(panelType, out var panel) ? panel : null;

        public void AddPanel(Panel.PanelType panelType, Panel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            _panels[panelType] = panel;
        }

        private void DeactivatePanel(Panel panel)
        {
            if (panel.isNeverRemove) return;

            panel.gameObject.SetActive(false);
            panel.isCurrentlyOpen = false;
            if (panel.filterImage != null) panel.filterImage.SetActive(false);
        }

        private void ActivatePanel(Panel panel)
        {
            panel.gameObject.SetActive(true);
            panel.isCurrentlyOpen = true;
            if (panel.filterImage != null) panel.filterImage.SetActive(true);
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
            // Deactivate the previous panel only if it's not marked as isNeverRemove
            if ((previousPanel.isCurrentlyPopup || !newPanel.isCurrentlyPopup) && !previousPanel.isNeverRemove)
            {
                DeactivatePanel(previousPanel);
            }

            // Add the previous panel back to the stack if it's not to be removed
            if (!removePreviousPanels && !previousPanel.isNeverRemove)
            {
                _previousPanels.Push(previousPanel);
            }
        }
    }
}