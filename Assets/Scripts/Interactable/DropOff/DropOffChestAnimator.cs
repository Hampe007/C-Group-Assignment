using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class DropOffChestAnimator : MonoBehaviour
{
    private static readonly int DepositTrigger = Animator.StringToHash("Deposit");

    [SerializeField] private AudioClip openSound;

    private Animator chestAnimator;
    private SO_SoundEffectData soundEffect;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
        soundEffect = AudioUtils.SoundEffects.chestOpenSound;
    }

    public void PlayDepositAnimation()
    {
        chestAnimator.SetTrigger(DepositTrigger);

        if (openSound != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(soundEffect.audioClip, transform, soundEffect.volume);
        }
    }
}
