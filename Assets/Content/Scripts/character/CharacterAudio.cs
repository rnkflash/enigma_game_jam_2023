using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] AudioSource footstepsAudioSource = null;
    [SerializeField] AudioSource jumpingAudioSource = null;

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] softSteps = null;
    [SerializeField] AudioClip[] concreteSteps = null;
    [SerializeField] AudioClip softLanding = null;
    [SerializeField] AudioClip hardLanding = null;
    [SerializeField] AudioClip jump = null;

    private float stepsTimer;

    public void PlaySteps(GroundType groundType, Vector3 position)
    {
        if (groundType == GroundType.None)
            return;

        var steps = groundType == GroundType.Concrete ? concreteSteps : softSteps;
        int index = Random.Range(0, steps.Length);
        footstepsAudioSource.PlayOneShot(steps[index]);
    }

    public void PlayJump()
    {
        jumpingAudioSource.PlayOneShot(jump);
    }

    public void PlayLanding(GroundType groundType, Vector3 position)
    {
        if (groundType == GroundType.Concrete)
            jumpingAudioSource.PlayOneShot(hardLanding);
        else
            jumpingAudioSource.PlayOneShot(softLanding);
    }
}