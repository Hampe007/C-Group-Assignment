using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Sound Effect Settings Data")]
public class SO_SoundFXSettingsData : ScriptableObject
{
    public SO_SoundEffectData timerBeepSound;
    public SO_SoundEffectData buttonPressSound;
    public SO_SoundEffectData gameOverSound;
    public SO_SoundEffectData lvlStartSound;
    public SO_SoundEffectData pickupSound;
    public SO_SoundEffectData chestOpenSound;
    public SO_SoundEffectData jumpLandSound;
}
