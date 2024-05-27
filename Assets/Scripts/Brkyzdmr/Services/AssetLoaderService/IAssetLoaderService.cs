using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brkyzdmr.Services.AssetLoaderService
{
    public interface IAssetLoaderService 
    {
        Task<T> LoadAsset<T>(string assetId) where T : UnityEngine.Object;
        Task<IEnumerable<string>> LoadAllAssetPaths<T>(string basePath)  where T : UnityEngine.Object;

    }
}