using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Building / Wave Background Music")]
    public AudioClip buildBackgroundMusic;
    public AudioClip waveBackgroundMusic;

    [Header("Sound Effects")]
    public AudioClip shootSFX;
    public AudioClip enemyDeathSFX;

    public bool allowSFX = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(buildBackgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip)   return; //If clip is the original 
        musicSource.Stop();
        musicSource.clip = clip;    //Change Audio clip
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (allowSFX && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}