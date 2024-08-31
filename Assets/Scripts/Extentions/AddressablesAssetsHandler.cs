using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public static class AddressablesAssetsHandler
{
    private static List<AssetReference> assetReferences = new List<AssetReference>();

    public static void AddReference(AssetReference assetReference)
    {
        if (!assetReferences.Contains(assetReference))
            assetReferences.Add(assetReference);
    }

    public static void ReleaseReferences()
    {
        foreach (var item in assetReferences)
        {
            if (!item.IsValid())
                item.ReleaseAsset();
        }

        assetReferences.Clear();
    }
}
