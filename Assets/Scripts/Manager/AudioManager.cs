using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource sfxLoopSource;

    [Header("Clips de Audio")]
    public AudioClip backgroundMusic;
    public AudioClip cannonExplosion;
    public AudioClip pinguinLaunch;
    public AudioClip idleSound;
    public AudioClip cowMoo;
    public AudioClip cowBell;

    private float savedMusicVol = 1f;
    private float savedSFXVol = 1f;
    private bool isMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        Playidle();
    }

    public void SetMusicVolume(float volume)
    {
        savedMusicVol = volume;
        if (!isMuted) musicSource.volume = savedMusicVol;
    }

    public void SetSFXVolume(float volume)
    {
        savedSFXVol = volume;
        if (!isMuted)
        {
            sfxSource.volume = savedSFXVol;
            sfxLoopSource.volume = savedSFXVol;
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            musicSource.volume = 0f;
            sfxSource.volume = 0f;
            sfxLoopSource.volume = 0f;
        }
        else
        {
            musicSource.volume = savedMusicVol;
            sfxSource.volume = savedSFXVol;
            sfxLoopSource.volume = savedSFXVol;
        }
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
    }

    public void PlayImpact()
    {
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
