using Brkyzdmr.Services.EventService;
using UnityEngine.UI;

namespace Brkyzdmr.Services.UIService
{
    public abstract class ButtonHandler
    {
        protected Button Button;
        protected readonly IEventService EventService;

        protected ButtonHandler(IEventService eventService)
        {
            EventService = eventService;
        }

        public abstract void HandleButtonClicked();

        public void SetButton(Button button)
        {
            Button = button;
        }

        public void SetButtonStatus(bool status)
        {
            Button.interactable = status;
        }
    }
}