using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip gameOverMusic;
    public AudioClip marioDiesMusic;
    public AudioClip victoryMusic;

    public AudioClip jumpSound;
    public AudioClip bigJumpSound;    
    public AudioClip bumpSound;
    public AudioClip stompSound;
    public AudioClip coinSound;
    public AudioClip powerUpAppearSound;
    public AudioClip brickBreakSound;
    public AudioClip powerUpSound;
    public AudioClip powerDownSound;
    public AudioClip flagSound;
    public AudioClip pauseSound;



    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = false;
        musicSource.Play();
    }
}
