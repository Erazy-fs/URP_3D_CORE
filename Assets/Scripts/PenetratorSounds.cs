using UnityEngine;

public class PenetratorSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip landingSound;
    public AudioClip pumpUpSound;
    public AudioClip pumpDownSound;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayLandingSound()
    {
        audioSource.PlayOneShot(landingSound);
    }
    public void PlayPumpUpSound()
    {
        audioSource.PlayOneShot(pumpUpSound);
    }
    
    public void PlayPumpDownSound()
    {
        audioSource.PlayOneShot(pumpDownSound);
    }
}
