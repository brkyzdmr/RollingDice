using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;

namespace RollingDice.Runtime.Event
{
    // Scene Load Events
    public class OnGameSceneLoaded : GameEvent {}
    public class OnLobbySceneLoaded : GameEvent {}
    public class OnApplicationStart : GameEvent {}

    // Configs Events
    public class OnGameConfigLoaded : GameEvent {}

    // Pool Events
    public class OnPoolsInitialized : GameEvent {}
    
    // Level Events
    public class OnLevelRestart : GameEvent { }
    public class OnNextLevel : GameEvent { }

    // UI Events
    public class OnUIPanelChangeRequested : GameEvent<Panel.PanelType> { }
    public class OnRollDiceButtonClicked : GameEvent {}

    // Game Events
    public class OnDiceRolled : GameEvent<List<int>> { }
    public class OnDiceCountChanged : GameEvent<int> { }
    public class OnAvatarMoveCompleted : GameEvent { }
    public class OnInventoryCreated : GameEvent { }
}