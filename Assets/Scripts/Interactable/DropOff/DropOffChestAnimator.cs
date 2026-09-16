using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class DropOffChestAnimator : MonoBehaviour
{
    private static readonly int DepositTrigger = Animator.StringToHash("Deposit");

    [SerializeField] private AudioClip openSound;

    private Animator chestAnimator;
    private AudioSource audioSource;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDepositAnimation()
    {
        chestAnimator.SetTrigger(DepositTrigger);

        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
}