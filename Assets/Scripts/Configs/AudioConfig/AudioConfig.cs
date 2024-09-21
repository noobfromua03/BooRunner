
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioConfig : AbstractConfig<AudioConfig>
{
    [field: SerializeField] public List<AudioData> Sounds { get; private set; }

    public AudioData GetAudioDataByType(SoundType type)
        => Sounds.FindAll(s => s.Type == type).OrderBy(s => UnityEngine.Random.value).First();
}

[Serializable]

public class AudioData
{
    [field: SerializeField] public SoundType Type { get; private set; }
    [field: SerializeField] public SubType SubType { get; private set; }
    [field: SerializeField] public AudioClip Clip { get; private set; }
}

public enum SoundType
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

public enum SubType
{
    Sound = 0,
    Music = 1
}