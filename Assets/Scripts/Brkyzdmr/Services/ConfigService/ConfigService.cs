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
        
        public async Task LoadConfigs(string gameConfigPath, string boardConfigsBasePath, 
            string itemConfigsBasePath, string avatarConfigsBasePath)
        {
            var gameConfigText = await _assetLoaderService.LoadAsset<TextAsset>(gameConfigPath);
            gameConfig = JsonConvert.DeserializeObject<GameConfig>(gameConfigText.text);

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

            await LoadBoardConfigs(boardConfigsBasePath);
            await LoadItemConfigs(itemConfigsBasePath);
            await LoadAvatarConfigs(avatarConfigsBasePath);
             

            await Task.Yield();
        }

        private async Task LoadBoardConfigs(string boardConfigsBasePath)
        {
            await LoadConfigs(boardConfigsBasePath, boardConfigs);
        }

        private async Task LoadItemConfigs(string itemConfigsBasePath)
        {
            await LoadConfigs(itemConfigsBasePath, itemConfigs);
        }

        private async Task LoadAvatarConfigs(string avatarConfigsBasePath)
        {
            await LoadConfigs(avatarConfigsBasePath, avatarConfigs);
        }

        private async Task LoadConfigs<T>(string basePath, Dictionary<string, T> configDictionary)
        {
            try
            {
                var configPaths = await _assetLoaderService.LoadAllAssetPaths<TextAsset>(basePath);

                foreach (var configPath in configPaths)
                {
                    try
                    {
                        var configText = await _assetLoaderService.LoadAsset<TextAsset>(configPath);
                        var config = JsonConvert.DeserializeObject<T>(configText.text);
                        var idProperty = typeof(T).GetProperty("id");
                        if (idProperty != null)
                        {
                            var id = (string)idProperty.GetValue(config);
                            configDictionary[id] = config;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error loading config from path {configPath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading config paths from base path {basePath}: {ex.Message}");
            }
        }
    }
}