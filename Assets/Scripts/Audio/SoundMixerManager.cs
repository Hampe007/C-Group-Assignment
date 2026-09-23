using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;

    /// <summary>
    /// Returns the volume value from the main mixer master channel mapped to a value between 0f and 100f.
    /// </summary>
    public float MasterVolume
    {
        get
        {
            audioMixer.GetFloat("masterVolume", out float volume);
            return AudioUtils.DecibelToPercent(volume);
        }
    }

    /// <summary>
    /// Returns the volume value from the main mixer soundFX channel mapped to a value between 0f and 100f.
    /// </summary>
    public float SoundFXVolume
    {
        get
        {
            audioMixer.GetFloat("soundFXVolume", out float volume);
            return AudioUtils.DecibelToPercent(volume);
        }
    }

    /// <summary>
    /// Returns the volume value from the main mixer music channel mapped to a value between 0f and 100f.
    /// </summary>
    public float MusicVolume
    {
        get
        {
            audioMixer.GetFloat("musicVolume", out float volume);
            return AudioUtils.DecibelToPercent(volume);
        }
    }

    /// <summary>
    /// Set the master volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="percent"></param>
    public void SetMasterVolume(float percent)
    {
        audioMixer.SetFloat("masterVolume", AudioUtils.PercentToDecibel(percent));
    }

    /// <summary>
    /// Set the soundFX volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="percent"></param>
    public void SetSoundFXVolume(float percent)
    {
        audioMixer.SetFloat("soundFXVolume", AudioUtils.PercentToDecibel(percent));
    }

    /// <summary>
    /// Set the music volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="percent"></param>
    public void SetMusicVolume(float percent)
    {
        audioMixer.SetFloat("musicVolume", AudioUtils.PercentToDecibel(percent));
    }
}
