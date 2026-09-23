using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Sound Effect Data")]
public class SO_SoundEffectData : ScriptableObject
{
    public AudioClip audioClip;
    [Range(0, 100)] public float volume;
}
