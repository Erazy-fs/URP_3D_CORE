using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] stepSounds;
    private Animator animator;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        animator = GetComponent<Animator>();
    }

    public void PlayStepSound()
    {
        if (!IsGrounded()) return;

        if (animator != null && (animator.GetCurrentAnimatorStateInfo(0).IsName("walk") is true || animator?.GetCurrentAnimatorStateInfo(0).IsName("walkAndHold") is true))
        {
            if (stepSounds.Length > 0)
            {
                AudioClip randomStepSound = stepSounds[Random.Range(0, stepSounds.Length)];
                audioSource.PlayOneShot(randomStepSound);
            }
        }
    }

    private bool IsGrounded()
{
    return Physics.Raycast(transform.position, Vector3.down, 1.1f);
}
}
