using System.Collections.Generic;
using System.Threading.Tasks;
using Brkyzdmr.Services.ConfigService;

namespace Brkyzdmr.Services.ConfigService
{
    public interface IConfigService
    {
        GameConfig gameConfig { get; }
        Dictionary<string, LevelConfig> levelConfigs { get; } // level id, level config
        Dictionary<string, ItemConfig> itemConfigs { get; } // item id, item config
        Dictionary<string, BoardConfig> boardConfigs { get; } // board id, board config
        Dictionary<string, AvatarConfig> avatarConfigs { get; } // avatar id, avatar config
        LevelConfig currentLevelConfig { get; }
        void SetCurrentLevelConfig(string id);
        void SetNextLevelConfig();
        Task LoadConfigs(string gameConfigPath, string boardConfigsBasePath,
            string itemConfigsBasePath, string avatarConfigsBasePath);
    }
}