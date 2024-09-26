using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    private Dictionary<AudioType, List<AudioClip>> audioClips = new();

    private List<AudioSource> audioSources = new();

    private const float SOUND_VOLUME = 0f;
    private const float MUSIC_VOLUME = -4f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayAudioByType(AudioType.MenuMusic, AudioSubType.Music);
        ChangeMusicVolume();
        ChangeSoundVolume();
    }

    public AudioSource PlayAudioByType(AudioType type, AudioSubType subType)
    {
        if (audioClips.ContainsKey(type) == false)
            GetAudioData(type);

        var source = GetFreeAudioSource(type, subType);
        source.clip = GetClip(type);
        source.Play();
        return source;
    }
    private AudioSource GetFreeAudioSource(AudioType type, AudioSubType subType)
    {
        AudioSource freeSource = audioSources.Find(s => s.isPlaying == false);

        if (audioSources.Count == 0 || freeSource == null)
        {
            freeSource = this.AddComponent<AudioSource>();
            audioSources.Add(freeSource);
        }


        switch (subType)
        {
            case AudioSubType.Music:
                freeSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music").First();
                break;
            case AudioSubType.Sound:
                freeSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound").First();
                break;
        }

        audioSources.Add(freeSource);
        freeSource.loop = subType == AudioSubType.Music;
        freeSource.playOnAwake = false;

        return freeSource;
    }

    private void GetAudioData(AudioType type)
    {
        var data = AudioConfig.Instance.GetAudioDataByType(type);
        var clipList = new List<AudioClip>();

        foreach (var clip in data)
        {
            var loadedClip = clip.Clip;
            clipList.Add(loadedClip);
        }
        audioClips.Add(type, clipList);
    }

    private AudioClip GetClip(AudioType type)
        => audioClips[type].OrderBy(c => UnityEngine.Random.value).First();

    public void Reload()
    {
        foreach (var source in audioSources)
            Destroy(source);
        audioSources.Clear();
    }

    public void ChangeSoundVolume()
    {
        if (Progress.Options.SoundMute)
            audioMixer.SetFloat("SoundVolume", SOUND_VOLUME);
        else
            audioMixer.SetFloat("SoundVolume", -80f);
    }

    public void ChangeMusicVolume()
    {
        if (Progress.Options.MusicMute)
            audioMixer.SetFloat("MusicVolume", MUSIC_VOLUME);
        else
            audioMixer.SetFloat("MusicVolume", -80f);
    }
}