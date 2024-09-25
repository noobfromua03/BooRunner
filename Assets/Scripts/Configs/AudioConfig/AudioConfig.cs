using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioConfig : AbstractConfig<AudioConfig>
{
    [field: SerializeField] public List<AudioData> Sounds { get; private set; }

    public List<AudioData> GetAudioDataByType(AudioType type)
        => Sounds.FindAll(s => s.Type == type).ToList();
}

[Serializable]

public class AudioData
{
    [field: SerializeField] public AudioType Type { get; private set; }
    [field: SerializeField] public AudioClip Clip { get; private set; }
}

public enum AudioType
{
    Boo = 0,
    Booster = 1,
    ButtonClick = 2,
    CastleMusic = 3,
    CemetryMusic = 4,
    ChillingTouch = 5,
    Damaged = 6,
    DarkCloud = 7,
    EssenceCatch = 8,
    FarmMusic = 9,
    FortuneWheel = 10,
    Immateriality = 11,
    LevelPassed = 12,
    MenuMusic = 13,
    ParkMusic = 14,
    PhantomOfTheOpera = 15,
    RewardOpen = 16,
    Salute = 17,
    Scared = 18,
    SlowMotion = 19,
    TownLegend = 20,
    TownMusic = 21
}

public enum AudioSubType
{
    Sound = 0,
    Music = 1
}