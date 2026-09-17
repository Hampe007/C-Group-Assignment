using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropOffFeedback : MonoBehaviour
{
    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem depositParticles;
    [SerializeField] private GameObject rangeGlow;
    [SerializeField] private GameObject floatingMarker;

    [Header("Gamepad Rumble")]
    [SerializeField] private bool useRumble = true;
    [SerializeField, Range(0f, 1f)] private float rumbleStrength = 0.25f;
    [SerializeField] private float rumbleDuration = 0.12f;

    public void SetAvailable(bool available)
    {
        if (rangeGlow != null)
        {
            rangeGlow.SetActive(available);
        }
    }

    public void SetMarkerVisible(bool visible)
    {
        if (floatingMarker != null)
        {
            floatingMarker.SetActive(visible);
        }
    }
    
    public void PlayDeposit()
    {
        depositParticles?.Play();

        if (useRumble && Gamepad.current != null)
        {
            StartCoroutine(Rumble(Gamepad.current));
        }
    }

    private IEnumerator Rumble(Gamepad gamepad)
    {
        gamepad.SetMotorSpeeds(rumbleStrength, rumbleStrength);

        yield return new WaitForSecondsRealtime(rumbleDuration);

        gamepad.SetMotorSpeeds(0f, 0f);
    }
}