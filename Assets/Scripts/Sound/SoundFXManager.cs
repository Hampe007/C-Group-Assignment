using UnityEngine;

/// <summary>
/// Use this as instead of AudioClip to associate a specific volume with an AudioClip
/// </summary>
[System.Serializable]
public struct SoundFXClip
{
    public AudioClip audioClip;
    [Range(0, 100)] public float volume;
}

public class SoundFXManager : Singleton<SoundFXManager>
{
    [SerializeField] private AudioSource soundFXObject;
    private AudioSource repeatSource = null;
    private float _timer = 0f;
    private float _repeatDelay = 0f;
    private int _iterationsLeft = 0;

    /* PUBLIC METHODS */

    /// <summary>
    /// Play a sound effect once.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="spawnTransform">The transform which will be used to determine where the SoundFXObject is spawned.</param>
    /// <param name="volume">The relative volume at which to play the AudioClip(s). Expected to be a value between 0f and 100f.</param>
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = Mathf.Log10(volume / 100 + 1);
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        DontDestroyOnLoad(audioSource.gameObject);
        Destroy(audioSource.gameObject, clipLength);
    }

    /// <summary>
    /// Play a random sound effect from an array of AudioClips.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="spawnTransform">The transform which will be used to determine where the SoundFXObject is spawned.</param>
    /// <param name="volume">The relative volume at which to play the AudioClip(s). Expected to be a value between 0f and 100f.</param>
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];
        audioSource.volume = Mathf.Log10(volume / 100 + 1);
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    /// <summary>
    /// Play a sound effect a given amount of times. SoundFXManager can currently only keep track of one repeating sound effect.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="spawnTransform">The transform which will be used to determine where the SoundFXObject is spawned.</param>
    /// <param name="volume">The relative volume at which to play the AudioClip(s). Expected to be a value between 0f and 100f.</param>
    /// <param name="iterations"></param>
    /// <param name="delay">The interval between invokes.</param>
    public void PlaySoundFXtimes(AudioClip audioClip, Transform spawnTransform, float volume, int iterations, float delay)
    {
        _iterationsLeft = iterations;
        _repeatDelay = delay;
        repeatSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        repeatSource.clip = audioClip;
        repeatSource.volume = Mathf.Log10(volume / 100 + 1);

        float clipLength = repeatSource.clip.length;

        Destroy(repeatSource.gameObject, iterations * (clipLength + delay));
    }

    /* MONOBEHAVIOR LIFECYCLE METHODS */

    private void Update()
    {
        if (_timer <= 0 && _iterationsLeft > 0)
        {
            repeatSource.Play();
            _timer = repeatSource.clip.length + _repeatDelay;
            _iterationsLeft--;
        }

        _timer -= Time.deltaTime;
    }
}
