using UnityEngine;

public static class AudioUtils
{
    const float MINIMUM_VOLUME_DB = -80f;
    public static SO_SoundFXSettingsData SoundEffects => SoundFXManager.Instance.SoundEffects;
    public static float PercentToDecibel(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);

        if (percent <= 0f)
        {
            return MINIMUM_VOLUME_DB;
        }

        float linear = percent / 100f;
        return Mathf.Log10(linear) * 20f;
    }

    public static float DecibelToPercent(float db)
    {
        if (db <= MINIMUM_VOLUME_DB)
        {
            return 0f;
        }

        float linear = Mathf.Pow(10f, db / 20f);
        return linear * 100f;
    }
}
