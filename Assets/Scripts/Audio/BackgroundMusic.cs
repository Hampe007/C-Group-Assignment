using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public float Volume => _audioSource.volume;
    public float InitialVolume => _initialVolume;
    public int SceneIndex => _sceneIndex;
    private AudioSource _audioSource;
    private int _sceneIndex;
    private float _initialVolume;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;

        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(SO_MusicData musicData, int sceneIndex)
    {
        _audioSource.outputAudioMixerGroup = musicData.audioMixerGroup;
        _audioSource.clip = musicData.audioClip;
        _initialVolume = musicData.volume;
        _audioSource.volume = musicData.volume / 100f;
        _sceneIndex = sceneIndex;
    }

    /// <summary>
    /// Play the BackgroundMusic from the beginning.
    /// </summary>
    /// <param name="delay"></param>
    public void Play(float delay)
    {
        StartCoroutine(PlayRoutine(delay));
    }

    /// <summary>
    /// Lerps the volume of the BackgroundMusic between startPercent and targetPercent.
    /// </summary>
    /// <param name="startPercent"></param>
    /// <param name="targetPercent"></param>
    /// <param name="duration"></param>
    /// <param name="delay"></param>
    public void TransitionVolume(float startPercent, float targetPercent, float duration, float delay)
    {
        StartCoroutine(TransitionVolumeRoutine(startPercent, targetPercent, duration, delay));
    }

    private void ResetVolume()
    {
        _audioSource.volume = _initialVolume / 100;
    }

    private IEnumerator PlayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.Play();
    }

    private IEnumerator TransitionVolumeRoutine(float startPercent, float targetPercent, float duration, float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float currentPercent = Mathf.Lerp(startPercent, targetPercent, t);

            _audioSource.volume = currentPercent / 100;

            elapsed += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = targetPercent / 100;

        if (targetPercent <= 0f)
        {
            _audioSource.Stop();
            ResetVolume();
        }
    }
}
