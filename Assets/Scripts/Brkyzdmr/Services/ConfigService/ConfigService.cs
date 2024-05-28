using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brkyzdmr.Services.AssetLoaderService;
using Newtonsoft.Json;
using RollingDice.Runtime.Global;
using UnityEngine;

namespace Brkyzdmr.Services.ConfigService
{
    public class ConfigService : Service, IConfigService
    {
        public GameConfig gameConfig { get; private set; } = new();
        public Dictionary<string, LevelConfig> levelConfigs { get; private set; } = new();
        public Dictionary<string, ItemConfig> itemConfigs { get; private set; } = new();
        public Dictionary<string, BoardConfig> boardConfigs { get; private set; } = new();
        public Dictionary<string, AvatarConfig> avatarConfigs { get; private set; } = new();
        public LevelConfig currentLevelConfig { get; private set; }
        
        private GameConfig _gameConfig;
        private readonly IAssetLoaderService _assetLoaderService = Services.GetService<IAssetLoaderService>();

        public void SetCurrentLevelConfig(string id)
        {
            currentLevelConfig = levelConfigs[id];
        }

        public void SetNextLevelConfig()
        {
            Debug.Log("ConfigService: SetNextLevelConfig!");
            if (levelConfigs.Count == 0) return; // No levels loaded

            string nextLevelId = DetermineNextLevelId(); 

            if (levelConfigs.TryGetValue(nextLevelId, out var config)) 
            {
                currentLevelConfig = config;
            }
        }

        private string DetermineNextLevelId()
        {
            if (currentLevelConfig == null) return levelConfigs.Keys.First();

            var nextLevelId = levelConfigs.Keys
                .SkipWhile(id => id != currentLevelConfig.id)
                .Skip(1) // Skip tutorial level
                .FirstOrDefault();

            Debug.Log("ConfigService: DetermineNextLevelId! - " + nextLevelId);
            return nextLevelId ?? levelConfigs.Keys.First();
        }
        
        public async Task LoadConfigs(string gameConfigPath)
        {
            var gameConfigText = await _assetLoaderService.LoadAsset<TextAsset>(gameConfigPath);
            gameConfig = JsonConvert.DeserializeObject<GameConfig>(gameConfigText.text);
            
            await LoadLevelConfigs();
            await LoadBoardConfigs();
            await LoadItemConfigs();
            await LoadAvatarConfigs();
             

            await Task.Yield();
        }
        
        private async Task LoadLevelConfigs()
        {
            foreach (var levelPath in gameConfig.levelOrder)
            {
                try
                {
                    var levelConfigText = await _assetLoaderService.LoadAsset<TextAsset>(levelPath);
                    var levelConfig = JsonConvert.DeserializeObject<LevelConfig>(levelConfigText.text); 
                    levelConfigs[levelConfig.id] = levelConfig;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading level from path {levelPath}: {ex.Message}");
                }
            }
            
            await Task.Yield();
        }

        private async Task LoadBoardConfigs()
        {
            foreach (var boardPath in gameConfig.boards)
            {
                try
                {
                    var boardConfigText = await _assetLoaderService.LoadAsset<TextAsset>(boardPath);
                    var boardConfig = JsonConvert.DeserializeObject<BoardConfig>(boardConfigText.text); 
                    boardConfigs[boardConfig.id] = boardConfig;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading board from path {boardPath}: {ex.Message}");
                }
            }
            
            await Task.Yield();
        }

        private async Task LoadItemConfigs()
        {
            foreach (var itemPath in gameConfig.items)
            {
                try
                {
                    var itemConfigText = await _assetLoaderService.LoadAsset<TextAsset>(itemPath);
                    var itemConfig = JsonConvert.DeserializeObject<ItemConfig>(itemConfigText.text); 
                    itemConfigs[itemConfig.id] = itemConfig;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading item from path {itemPath}: {ex.Message}");
                }
            }
            
            await Task.Yield();
        }

        private async Task LoadAvatarConfigs()
        {
            foreach (var avatarPath in gameConfig.avatars)
            {
                try
                {
                    var avatarConfigText = await _assetLoaderService.LoadAsset<TextAsset>(avatarPath);
                    var avatarConfig = JsonConvert.DeserializeObject<ItemConfig>(avatarConfigText.text); 
                    itemConfigs[avatarConfig.id] = avatarConfig;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading avatar from path {avatarPath}: {ex.Message}");
                }
            }
            
            await Task.Yield();
        }
    }
}