using System;
using System.Collections.Generic;
using System.Linq;
using Brkyzdmr.Services;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RollingDice.Runtime.Board
{
    public class Board : MonoBehaviour
    {
        public List<Tile> Tiles;
        
        private IConfigService _configService;
        private IEventService _eventService;

        private void Awake()
        {
            _configService = Services.GetService<IConfigService>();
            _eventService = Services.GetService<IEventService>();
        }

        private void OnEnable()
        {
            _eventService.Get<OnGameConfigLoaded>().AddListener(OnGameConfigLoaded);
        }

        private void OnDisable()
        {
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(OnGameConfigLoaded);
        }

        private void OnGameConfigLoaded()
        {
            SetTiles();
        }

        private Tile GetTileAtIndex(int index)
        {
            if (Tiles == null || index < 0 || index >= Tiles.Count)
            {
                return null;
            }
            return Tiles[index];
        }

        private void SetTiles()
        {
            for (var i = 0; i < Tiles.Count; i++)
            {
                var tile = Tiles[i];
                var tileId = _configService.currentLevelConfig.tiles[i];
                
                if (tileId == "-")
                {
                    continue;
                }

                var itemData = new ItemData
                {
                    config = _configService.itemConfigs[tileId],
                    count = _configService.currentLevelConfig.rewards[i]
                };

                tile.ItemData = itemData;
                tile.SetItemVisual();
            }
        }
    }
}