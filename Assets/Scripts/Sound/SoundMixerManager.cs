using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    const float MINIMUM_VOLUME_DB = -80f;
    [SerializeField] private AudioMixer audioMixer;

    /// <summary>
    /// Returns the volume value from the main mixer master channel mapped to a value between 0f and 100f.
    /// </summary>
    public float MasterVolume
    {
        get
        {
            audioMixer.GetFloat("masterVolume", out float volume);
            return DecibelToPercent(volume);
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
            return DecibelToPercent(volume);
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
            return DecibelToPercent(volume);
        }
    }

    /// <summary>
    /// Set the master volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="level"></param>
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", PercentToDecibel(level));
    }

    /// <summary>
    /// Set the soundFX volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="level"></param>
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", PercentToDecibel(level));
    }

    /// <summary>
    /// Set the music volume. Expects a value between 0f and 100f.
    /// </summary>
    /// <param name="level"></param>
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", PercentToDecibel(level));
    }

    private float PercentToDecibel(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);

        if (percent <= 0f)
        {
            return MINIMUM_VOLUME_DB;
        }

        float linear = percent / 100f;
        return Mathf.Log10(linear) * 20f;
    }

    private float DecibelToPercent(float db)
    {
        if (db <= MINIMUM_VOLUME_DB)
        {
            return 0f;
        }

        float linear = Mathf.Pow(10f, db / 20f);
        return linear * 100f;
    }
}
