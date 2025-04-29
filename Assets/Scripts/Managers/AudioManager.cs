using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Building / Wave Background Music")]
    public AudioClip buildBackgroundMusic;
    public AudioClip waveBackgroundMusic;

    [Header("Sound Effects")]
    public AudioClip[] soundEffects;

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

    public void PlaySFX(ManagerGame.SFXTypes sfx)       //ManagerGame holds the soundEffects
    {
        int index = (int) sfx;
        
        if (index >= 0 && index < soundEffects.Length)
        {
            sfxSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("No SFX, index out of range" + sfx);
        }
    }

    public void ResetAudio()
    {
        PlayMusic(buildBackgroundMusic);
    }

}