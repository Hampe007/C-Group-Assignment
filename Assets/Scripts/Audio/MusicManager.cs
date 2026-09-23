using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// TODO: Add support for multiple songs per scene.
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [Space()]
    [SerializeField] private SO_MusicSettingsData musicSettings;
    Dictionary<int, BackgroundMusic> _sceneMusicLookUp = new();
    Dictionary<int, BackgroundSongSettings> _sceneMusicSettingsLookUp = new();

    private void Start()
    {
        musicSettings.BackgroundSongs.ForEach((musicSetting) =>
        {
            var music = new GameObject(musicSetting.name).AddComponent<BackgroundMusic>();
            music.Initialize(musicSetting.data, musicSetting.sceneIndex);

            _sceneMusicLookUp.Add(musicSetting.sceneIndex, music);
            _sceneMusicSettingsLookUp.Add(musicSetting.sceneIndex, musicSetting);
        });

        GameState.Instance.GamePausedEvent.AddListener(SetLowPassFilterOn);
        GameState.Instance.GameOverEvent.AddListener(SetLowPassFilterOn);
        GameState.Instance.GameUnPausedEvent.AddListener(SetLowPassFilterOff);
        GameState.Instance.GameResetEvent.AddListener(SetLowPassFilterOff);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        PlaySceneMusic(SceneManager.GetActiveScene());
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

    private void PlaySceneMusic(Scene scene)
    {
        if (
            !_sceneMusicLookUp.TryGetValue(scene.buildIndex, out BackgroundMusic targetMusic)
            || !_sceneMusicSettingsLookUp.TryGetValue(scene.buildIndex, out BackgroundSongSettings musicSettings)
        )
        {
            return;
        }

        targetMusic.Play(musicSettings.timeAfterSceneLoadPlay);
        targetMusic.TransitionVolume(0f, targetMusic.InitialVolume, musicSettings.fadeInDuration, musicSettings.timeAfterSceneLoadPlay);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // TODO: Fade out the volume for the entire music AudioMixerGroup for the purpose of avoiding sound artifacts when scene is unloaded.
        if (
            !_sceneMusicLookUp.TryGetValue(scene.buildIndex, out BackgroundMusic targetMusic)
            || !_sceneMusicSettingsLookUp.TryGetValue(scene.buildIndex, out BackgroundSongSettings musicSettings)
        )
        {
            return;
        }

        targetMusic.TransitionVolume(targetMusic.Volume, 0f, musicSettings.fadeOutDuration, 0f);
    }

}
