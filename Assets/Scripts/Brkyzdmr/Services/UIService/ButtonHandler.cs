using Brkyzdmr.Services.EventService;

namespace Brkyzdmr.Services.UIService
{
    public abstract class ButtonHandler
    {
        protected readonly IEventService EventService;

        protected ButtonHandler(IEventService eventService)
        {
            EventService = eventService;
        }

        public abstract void HandleButtonClicked();
    }
}