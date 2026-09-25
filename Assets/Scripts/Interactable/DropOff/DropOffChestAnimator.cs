using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DropOffChestAnimator : MonoBehaviour
{
    private static readonly int DepositTrigger = Animator.StringToHash("Deposit");
    private Animator chestAnimator;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
    }

    public void PlayDepositAnimation()
    {
        chestAnimator.SetTrigger(DepositTrigger);

        SoundFXManager soundFXManager = SoundFXManager.Instance;
        SO_SoundEffectData soundEffect = soundFXManager?.SoundEffects?.chestOpenSound;

        if (soundFXManager == null || soundEffect == null || soundEffect.audioClip == null)
        {
            Debug.LogWarning("The chest deposit sound is not configured.", this);
            return;
        }

        soundFXManager.PlaySoundFXClip(soundEffect.audioClip, transform, soundEffect.volume);
    }
}
