using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Brkyzdmr.Services.AssetLoaderService
{
    public class ResourcesLoaderService : Service, IAssetLoaderService
    {
        public async Task<T> LoadAsset<T>(string assetId) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(assetId);
            await request; 

            if (request.isDone)
            {
                return request.asset as T;
            }
            else
            {
                Debug.LogError($"Failed to load asset from Resources: {assetId}"); 
                return null;
            }
        }
        
        public async Task<IEnumerable<string>> LoadAllAssetPaths<T>(string basePath) where T : UnityEngine.Object
        {
            return await Task.Run(() =>
            {
                var assets = Resources.LoadAll<T>(basePath);
                return assets.Select(asset => $"{basePath}/{asset.name}").ToList();
            });
        }
    }
}