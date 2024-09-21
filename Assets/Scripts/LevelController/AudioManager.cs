using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private Dictionary<SoundType, List<AudioSource>> audioSources = new();

    private AudioSource activeMusic;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayAudioByType(SoundType.MenuMusic);
    }

    public void PlayAudioByType(SoundType type)
    {
        if (audioSources.ContainsKey(type))
        {
            var notPlayingSource = audioSources[type].Find(s => s.isPlaying == false);
            if (notPlayingSource == null)
                notPlayingSource = CreateAudioSourceByType(type);

            notPlayingSource.Play();
        }
        else
        {
            var source = CreateAudioSourceByType(type);
            source.Play(); 
        }
    }
    private AudioSource CreateAudioSourceByType(SoundType type)
    {
        var source = this.AddComponent<AudioSource>();
        source.playOnAwake = false;

        var audioData = AudioConfig.Instance.GetAudioDataByType(type);
        source.loop = audioData.SubType == SubType.Music;
        source.clip = audioData.Clip;

        AddAudioSource(type, source);

        return source;
    }

    public void AddAudioSource(SoundType type, AudioSource source)
    {
        if (audioSources.ContainsKey(type))
            audioSources[type].Add(source);
        else
        {
            var audioSourceList = new List<AudioSource> { source };
            audioSources.Add(type, audioSourceList);
        }
    }

    public void Reload()
    {
        foreach (var key in audioSources.Keys)
        {
            foreach (var source in audioSources[key])
            {
                Destroy(source);
            }
            audioSources[key].Clear();
        }
        audioSources.Clear();
    }
}
