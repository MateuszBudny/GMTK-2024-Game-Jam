using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleBehaviour<SoundManager>
{
    public AudioSource ambientSource;
    public List<AudioSource> soundsSources;
    [SerializeField]
    private float audioFadeInDuration = 3f;

    [Header("Other sounds")]
    [SerializeField]
    private List<AudioRecord> sounds;
    [SerializeField]
    private List<AudioRecord> satanTalking;

    private Dictionary<AudioSource, float> audioSourcesDefaultVolumes = new Dictionary<AudioSource, float>();

    protected override void Awake()
    {
        base.Awake();
        DoOnAllSoundsSources(source => audioSourcesDefaultVolumes.Add(source, source.volume));
    }

    private void Start()
    {
        AudioFadeIn();
    }

    public void Play(Audio audioEnum)
    {
        AudioRecord audioRecordToPlay = sounds.Find(sound => sound.audioEnum == audioEnum);
        if(audioRecordToPlay.randomPitch)
        {
            soundsSources.GetRandomElement().PlayOneShot(audioRecordToPlay.audioClip, audioRecordToPlay.volume);
        }
        else
        {
            soundsSources[0].PlayOneShot(audioRecordToPlay.audioClip, audioRecordToPlay.volume);
        }
    }

    public void PlaySatanTalking()
    {
        AudioRecord satanTalkingChosen = satanTalking.GetRandomElement();
        soundsSources.GetRandomElement().PlayOneShot(satanTalkingChosen.audioClip, satanTalkingChosen.volume);
    }

    private void AudioFadeIn()
    {
        DoOnAllSoundsSources(source =>
        {
            float targetDefaultVolume = source.volume;
            source.volume = 0f;
            AdjustVolumeByTweening(source, targetDefaultVolume, audioFadeInDuration);
        });
    }

    public void StopAllMusicAndSounds() => DoOnAllSoundsSources(source => source.Stop());

    public void AdjustVolumeByTweening(AudioSource source, float targetVolume, float duration)
    {
        DOTween.To(() => source.volume, value => source.volume = value, targetVolume, duration).SetEase(Ease.Linear);
    }

    private void DoOnAllSoundsSources(Action<AudioSource> action)
    {
        action(ambientSource);
        soundsSources.ForEach(source => action(source));
    }
}

public enum Audio
{
    PickUp,
    PutDownOnProperPlace,
    Drop,
    Sell,
    EnterStation,

    Click,
    Success,
    SinglePatternPutOnGrid,
    MultiplyPattern,
    RemoveOrRevert,
}

[Serializable]
public class AudioRecord
{
    public AudioClip audioClip;
    public Audio audioEnum;
    public bool randomPitch = false;
    // TODO:
    public float minPitch = 0.5f;
    public float maxPitch = 1.5f;
    public float volume = 1f;
}