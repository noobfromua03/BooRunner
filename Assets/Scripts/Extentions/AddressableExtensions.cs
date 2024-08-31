using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public static class AddressableExtensions
{
    public static T GetAsset<T>(this AssetReferenceT<T> assetReference) where T : Object
    {
        AddressablesAssetsHandler.AddReference(assetReference);

        if (assetReference.IsValid())
        {
            return assetReference.Asset as T;
        }
        return assetReference.LoadAssetAsync<T>().WaitForCompletion();
    }

    public static async UniTask<T> GetAssetAsync<T>(this AssetReferenceT<T> assetReference) where T : Object
    {
        AddressablesAssetsHandler.AddReference(assetReference);

        if (!assetReference.IsValid())
        {
            await assetReference.LoadAssetAsync<T>().ToUniTask();
        }

        return assetReference.Asset as T;
    }
}
