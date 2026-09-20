using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioMixerSnapshot unpausedSnapshot;
    [SerializeField] AudioMixerSnapshot pausedSnapshot;

    private void Start()
    {
        GameState.Instance.GamePausedEvent.AddListener(SetLowPassFilterOn);
        GameState.Instance.GameUnPausedEvent.AddListener(SetLowPassFilterOff);
        GameState.Instance.GameResetEvent.AddListener(SetLowPassFilterOff);
    }

    private void SetLowPassFilterOn()
    {
        // TODO: Transition to Mixer Snapshots instead
        audioMixer.SetFloat("lowPassCutoffFrequency", 500f);
    }
    private void SetLowPassFilterOff()
    {
        // TODO: Transition to Mixer Snapshots instead
        audioMixer.SetFloat("lowPassCutoffFrequency", 5000f);
    }
}
