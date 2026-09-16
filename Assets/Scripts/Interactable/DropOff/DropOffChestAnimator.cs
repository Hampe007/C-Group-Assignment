using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DropOffChestAnimator : MonoBehaviour
{
    private static readonly int DepositTrigger = Animator.StringToHash("Deposit");

    private Animator chestAnimator;

    private void Awake() => chestAnimator = GetComponent<Animator>();

    public void PlayDepositAnimation() => chestAnimator.SetTrigger(DepositTrigger);
}