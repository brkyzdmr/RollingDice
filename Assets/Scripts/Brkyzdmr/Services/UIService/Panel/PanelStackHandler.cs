using System.Collections.Generic;

namespace Brkyzdmr.Services.UIService
{
    public class PanelStackHandler : IPanelStackHandler
    {
        private readonly Dictionary<Panel.PanelType, Panel> _panels = new Dictionary<Panel.PanelType, Panel>();
        private Stack<Panel> _previousPanels = new Stack<Panel>();

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

        public void RemoveFromPreviousPanels(Panel.PanelType panelType)
        {
            var tempStack = new Stack<Panel>();
            while (_previousPanels.TryPop(out var panel))
            {
                if (panel.panelType != panelType || panel.isNeverRemove)
                    tempStack.Push(panel);
            }

            _previousPanels = tempStack;
        }

        public Panel ReturnToPreviousPanel()
        {
            if (_previousPanels.Count == 0)
            {
                return null;
            }

            Panel previousPanel;
            do
            {
                previousPanel = _previousPanels.Pop();
            } while (previousPanel.isNeverRemove && _previousPanels.Count > 0);

            return previousPanel;
        }

        private Panel GetPanel(Panel.PanelType panelType)
        {
            if (_panels.ContainsKey(panelType))
            {
                return _panels[panelType];
            }
            return null; // Panel not found
        }

        public void AddPanel(Panel.PanelType panelType, Panel panel)
        {
            _panels[panelType] = panel;
        }
    }
}