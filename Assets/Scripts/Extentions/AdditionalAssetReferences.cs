using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceRoadPart : AssetReferenceT<RoadPart>
{
    public AssetReferenceRoadPart(string guid) : base(guid) { }
}
