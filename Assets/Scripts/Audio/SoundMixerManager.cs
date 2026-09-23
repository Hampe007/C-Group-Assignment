using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("Channel Multipliers")]
    [Range(50f, 200f)]
    [SerializeField] private float masterVolumeMultiplier = 100f;
    [Range(50f, 200f)]
    [SerializeField] private float soundFXVolumeMultiplier = 100f;
    [Range(50f, 200f)]
    [SerializeField] private float musicVolumeMultiplier = 100f;

    public float MasterVolume
    {
        get
        {
            audioMixer.GetFloat("masterVolume", out float volume);
            return GetOriginalPercent(volume, masterVolumeMultiplier);
        }
    }

    public float SoundFXVolume
    {
        get
        {
            audioMixer.GetFloat("soundFXVolume", out float volume);
            return GetOriginalPercent(volume, soundFXVolumeMultiplier);
        }
    }

    public float MusicVolume
    {
        get
        {
            audioMixer.GetFloat("musicVolume", out float volume);
            return GetOriginalPercent(volume, musicVolumeMultiplier);
        }
    }

    public void SetMasterVolume(float percent)
    {
        SetMixerVolume("masterVolume", percent, masterVolumeMultiplier);
    }

    public void SetSoundFXVolume(float percent)
    {
        SetMixerVolume("soundFXVolume", percent, soundFXVolumeMultiplier);
    }

    public void SetMusicVolume(float percent)
    {
        SetMixerVolume("musicVolume", percent, musicVolumeMultiplier);
    }

    private void SetMixerVolume(string parameter, float percent, float multiplier)
    {
        float db = AudioUtils.PercentToDecibel(percent);
        db += MultiplierToDecibel(multiplier);
        audioMixer.SetFloat(parameter, db);
    }

    private float GetOriginalPercent(float mixerDb, float multiplier)
    {
        float multiplierDb = MultiplierToDecibel(multiplier);
        float originalDb = mixerDb - multiplierDb;
        return AudioUtils.DecibelToPercent(originalDb);
    }

    private float MultiplierToDecibel(float multiplier)
    {
        return Mathf.Log10(multiplier / 100f) * 20f;
    }
}
