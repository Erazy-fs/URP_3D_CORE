using UnityEngine;

public class GunSounds : MonoBehaviour
{
    
    public AudioSource audioSource;
    public AudioClip singleShotSounds;
    public AudioClip burstShotSounds;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySingleShotSound()
    {
        audioSource.PlayOneShot(singleShotSounds);
    }

    public void PlayBurstShotSound()
    {
        audioSource.PlayOneShot(burstShotSounds);
    }
}
