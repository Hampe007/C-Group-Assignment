using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptable Objects/Music/Music Data")]
public class SO_MusicData : ScriptableObject
{
    public AudioMixerGroup audioMixerGroup;
    public AudioClip audioClip;
    [Range(0, 100)] public float volume;
}
