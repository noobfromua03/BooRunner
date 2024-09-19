using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EffectConfig : AbstractConfig<EffectConfig>
{
    [field: SerializeField] public List<EffectData> Effects { get; private set; }

    public GameObject GetEffectByType(EffectType type)
        => Effects.Find(e => e.Type == type).GetPrefab();
}

[Serializable]
public class EffectData
{
    [field: SerializeField] public EffectType Type { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject Effect { get; private set; }

    public GameObject GetPrefab()
        => AddressableExtensions.GetAsset(Effect);
}

public enum EffectType
{
    Damaged = 0,
    Scared = 1,
    Congratulate = 2,
    Immateriality = 3,
    SlowMotion = 4,
    DarkCloud = 5,
    ChillingTouch = 6,
    PhantomOfTheOpera = 7,
    TownLegend = 8
}