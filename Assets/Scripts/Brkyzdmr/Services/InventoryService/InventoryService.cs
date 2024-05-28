using System.Collections;
using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Tools.EzTween;
using UnityEngine;

namespace Brkyzdmr.Services.InventoryService
{
    public class InventoryService: Service, IInventoryService
    {
        public Dictionary<string, InventoryItem> items { set; get; } = new();
        public Dictionary<string, int> itemTracker { set; get; } = new();
        private readonly IConfigService _configService = Services.GetService<IConfigService>();
        private readonly IEventService _eventService = Services.GetService<IEventService>();
        
        public void InitializeInventoryData()
        {
            var itemConfigs = _configService.itemConfigs.Values;
            foreach (var itemData in itemConfigs)
            {
                itemTracker[itemData.id] = 0;
            }
        }

        public bool IsInInventory(string id)
        {
            return items.ContainsKey(id);
        }

        public void UpdateItem(GameObject itemObj, ItemConfig item, int itemCount, bool hasAnimation = false)
        {
            if (itemTracker.ContainsKey(item.id))
            {
                if (hasAnimation)
                {
                    var rectTransform = items[item.id].GetComponent<RectTransform>();
                    if (rectTransform == null)
                    {
                        Debug.LogError($"Item with id {item.id} does not have a RectTransform.");
                        return;
                    }

                    Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
                    
                    itemObj.transform.DoMove(screenPoint, 0.5f)
                        .SetRelative(true)
                        .OnComplete(() =>
                        {
                            itemTracker[item.id] += itemCount;
                            UpdateItemText(item.id);
                        });
                }
                else
                {
                    itemTracker[item.id] += itemCount;
                    UpdateItemText(item.id);
                }
                
                Debug.Log("Item updated!");
            }
        }

        private void UpdateItemText(string id)
        {
            if (items[id] != null)
            {
                items[id].itemCountText.text = Mathf.Max(itemTracker[id], 0).ToString();
            } 
        }
        
        private IEnumerator DoMove(GameObject itemObj, Vector3 targetPosition, float duration, System.Action onComplete)
        {
            Vector3 startPosition = itemObj.transform.position;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                itemObj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            itemObj.transform.position = targetPosition;

            onComplete?.Invoke();
        }
    }
}