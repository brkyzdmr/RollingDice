using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Brkyzdmr.Services.AssetLoaderService
{
    public class AddressablesLoaderService : Service, IAssetLoaderService
    {
        public async Task<T> LoadAsset<T>(string assetId) where T : UnityEngine.Object
        {
            var asyncOp = Addressables.LoadAssetAsync<T>(assetId);
        
            // Await the completion of the async operation
            await asyncOp.Task;

            if (asyncOp.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load asset with ID: {assetId}");
                return null; 
            }

            return asyncOp.Result;
        }

        public async Task<IEnumerable<string>> LoadAllAssetPaths<T>(string basePath) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadResourceLocationsAsync(basePath, typeof(T));
            await handle.Task;

            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                return handle.Result.Select(location => location.PrimaryKey);
            }
            else
            {
                Debug.LogError($"Error loading asset paths from base path {basePath}");
                return Enumerable.Empty<string>();
            }
        }
    }
}