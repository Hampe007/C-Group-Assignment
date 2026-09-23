using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Music Data")]
public class SO_MusicData : ScriptableObject
{
    public AudioMixerGroup audioMixerGroup;
    public AudioClip audioClip;
    [Range(0, 100)] public float volume;
}
