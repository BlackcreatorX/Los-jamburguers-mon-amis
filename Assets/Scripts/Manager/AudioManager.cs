using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Fonts")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Clips de Audio")]
    public AudioClip cannonExplosion;
    public AudioClip pinguinLaunch;
    public AudioClip idleSound;
    public AudioClip cowMoo;
    public AudioClip cowBell;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Playidle()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = idleSound;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopIdle()
    {
        musicSource.Stop();
    }

    public void PlayShoot()
    {
        sfxSource.PlayOneShot(cannonExplosion);
        sfxSource.PlayOneShot(pinguinLaunch);
    }

    public void PlayCowBell()
    {
        sfxSource.PlayOneShot(cowBell);
    }

    public void PlayCowMoo()
    {
        sfxSource.PlayOneShot(cowMoo);
    }
}
