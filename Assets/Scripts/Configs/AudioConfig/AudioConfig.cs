
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
    Damaged = 0,
    Boo = 1,
    Scared = 2,
    CatchEssence = 3,
    CatchBooster = 4,
    ChillingTouch = 5,
    PhantomOfTheOpera = 6,
    SlowMotion = 7,
    ButtonClick = 10,
    MenuMusic = 11,
    LevelMusic = 12,
}

public enum AudioSubType
{
    Sound = 0,
    Music = 1
}