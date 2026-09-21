using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BackgroundSongSettings
{
    public string name;
    public int sceneIndex;
    public SO_MusicData data;
    [Range(0f, 5f)]
    public float timeAfterSceneLoadPlay;
    [Range(0f, 5f)]
    public float fadeInDuration;
    [Range(0f, 5f)]
    public float fadeOutDuration;

}

[CreateAssetMenu(menuName = "Scriptable Objects/Music/Music Settings Data")]
public class SO_MusicSettingsData : ScriptableObject
{
    public List<BackgroundSongSettings> BackgroundSongs;
}
