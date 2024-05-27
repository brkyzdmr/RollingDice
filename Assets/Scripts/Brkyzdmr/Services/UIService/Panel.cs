using UnityEngine;
using UnityEngine.Serialization;

namespace Brkyzdmr.Services.UIService
{
    public class Panel : MonoBehaviour
    {
        public enum PanelType
        {
            None = 0,
            Fail = 1,
            Win = 2,
            Pause = 3,
            Game = 4,
            Lobby = 5,
            Loading = 6
        }

        public PanelType panelType;
        public bool isAlwaysPopup;
        public bool isNeverRemove;
        [HideInInspector] public bool isCurrentlyPopup;
        [HideInInspector] public bool isCurrentlyOpen;
        public GameObject filterImage;
    }
}