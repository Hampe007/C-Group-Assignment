using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class DropOffChestAnimator : MonoBehaviour
{
    private static readonly int DepositTrigger = Animator.StringToHash("Deposit");

    [SerializeField] private AudioClip openSound;
    
    private AudioSource chestAudioSource;
    private Animator chestAnimator;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
        chestAudioSource = GetComponent<AudioSource>();
    }
    
    public void PlayDepositAnimation()
    {
        chestAnimator.SetTrigger(DepositTrigger);

        if (openSound != null)
        {
            chestAudioSource.PlayOneShot(openSound);
        }
    }
}