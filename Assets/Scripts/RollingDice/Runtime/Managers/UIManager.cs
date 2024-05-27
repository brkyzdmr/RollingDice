using System.Collections.Generic;
using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RollingDice.Runtime.Managers
{
        public class UIManager : PersistentSingleton<SaveManager>
    {
        [SerializeField] private List<Panel> panels;
        
        // [SerializeField] private CanvasGroup loadingCanvasGroup;
        // [SerializeField] private Slider loadingSlider;

        private IUIService _uiService;
        private IEventService _eventService;

        protected override void Awake()
        {
            _uiService = Services.GetService<IUIService>();
            _eventService = Services.GetService<IEventService>();

            AddPanels();
            _uiService.ShowPanel(Panel.PanelType.Game);
        }

        private void Start()
        {
            // loadingCanvasGroup.gameObject.SetActive(true);
            // loadingCanvasGroup.alpha = 1;
        }

        private void OnEnable()
        {
            _eventService.Get<OnUIPanelChangeRequested>().AddListener(OnUIPanelChanged);
            _eventService.Get<OnGameConfigLoaded>().AddListener(OnGameConfigLoaded);
            _eventService.Get<OnGameSceneLoaded>().AddListener(OnGameSceneLoaded);
        }

        private void OnDisable()
        {
            _eventService.Get<OnUIPanelChangeRequested>().RemoveListener(OnUIPanelChanged);
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(OnGameConfigLoaded);
            _eventService.Get<OnGameSceneLoaded>().RemoveListener(OnGameSceneLoaded);
        }

        private void OnUIPanelChanged(Panel.PanelType panelType)
        {
            _uiService.ShowPanel(panelType);
        }

        private void OnSceneChange()
        {
            // loadingSlider.gameObject.SetActive(false);
            // loadingCanvasGroup.gameObject.SetActive(true);
            // loadingCanvasGroup.alpha = 0;
            // loadingCanvasGroup.DOFade(1, 0.8f)
            //     .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

        private void AddPanels()
        {
            foreach (var panel in panels)
            {
                _uiService.AddPanel(panel.panelType, panel);
            }
        }

        private void OnGameSceneLoaded()
        {
            // loadingSlider.DOValue(0.2f, 0.2f);
        }

        private void OnGameConfigLoaded()
        {
            // loadingSlider.DOValue(0.8f, 0.2f);
            // loadingSlider.DOValue(1f, 0.2f).SetDelay(0.6f);
            // loadingCanvasGroup.DOFade(0, 0.8f).OnComplete(() => loadingCanvasGroup.gameObject.SetActive(false));
        }
    }
}