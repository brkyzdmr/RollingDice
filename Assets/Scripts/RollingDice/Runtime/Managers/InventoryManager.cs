using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brkyzdmr.Attributes;
using Brkyzdmr.Services;
using Brkyzdmr.Services.AssetLoaderService;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.InventoryService;
using Brkyzdmr.Services.ObjectPoolService;
using RollingDice.Runtime.Event;
using UnityEngine;
using UnityEngine.Serialization;

namespace RollingDice.Runtime.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItem;
        [SerializeField] private Transform inventoryTransform;
        
        
        private IInventoryService _inventoryService;
        private IConfigService _configService;
        private IEventService _eventService;
        private IAssetLoaderService _assetLoaderService;
        private IObjectPoolService _objectPoolService;

        private void Awake()
        {
            _inventoryService = Services.GetService<IInventoryService>();
            _configService = Services.GetService<IConfigService>();
            _eventService = Services.GetService<IEventService>();
            _assetLoaderService = Services.GetService<IAssetLoaderService>();
            _objectPoolService = Services.GetService<IObjectPoolService>();
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
            _inventoryService.InitializeInventoryData();
            CreateInventory();
        }

        private async Task CreateInventory()
        {
            foreach (var itemConfigPair in _configService.itemConfigs)
            {
                var id = itemConfigPair.Key;
                var itemConfig = itemConfigPair.Value;
                
                await CreateItem(itemConfig);
            }
            Debug.Log("Inventory has created!");
            _eventService.Get<OnInventoryCreated>().Execute();
        }
        
        private async Task CreateItem(ItemConfig itemConfig)
        {
            var spritePath = _configService.itemConfigs[itemConfig.id].iconPath;
            var sprite = await _assetLoaderService.LoadAsset<Sprite>(spritePath);

            GameObject obj = Instantiate(inventoryItem.gameObject, inventoryTransform, true);
            obj.transform.localScale = Vector3.one;
            var goalTile = obj.GetComponent<InventoryItem>();
            goalTile.itemImage.sprite = sprite;
            goalTile.itemCountText.text = "0";

            _inventoryService.items[itemConfig.id] = goalTile;
        }

        [Button]
        private void AddItemToInventory()
        {
            var itemObj = _objectPoolService.Spawn("strawberry").Result;
            _inventoryService.UpdateItem(itemObj, _configService.itemConfigs["0"], 5);
        }
    }
}