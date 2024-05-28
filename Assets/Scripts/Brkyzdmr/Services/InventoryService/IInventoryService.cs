using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.UIService;
using UnityEngine;

namespace Brkyzdmr.Services.InventoryService
{
    public interface IInventoryService
    {
        Dictionary<string, InventoryItem> items { set; get; }
        Dictionary<string, int> itemTracker { set; get; }

        void InitializeInventoryData();
        bool IsInInventory(string id);
        void UpdateItem(GameObject itemObj, ItemConfig item, int itemCount, bool hasAnimation = false);
    }
}