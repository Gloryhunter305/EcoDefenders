using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicate
        }
    }

    public void ResetGame()
    {
        var manager = FindFirstObjectByType<ManagerGame>();
        if (manager != null)
            manager.ResetGameState();

        var storage = FindFirstObjectByType<StorageManager>();
        if (storage != null)
            storage.ResetStorage();

        var audio = FindFirstObjectByType<AudioManager>();
        if (audio != null)
            audio.ResetAudio();

        var meter = FindFirstObjectByType<SustainabilityMeter>();
        if (meter != null)
            meter.ResetMeter(); 
        var secret = FindFirstObjectByType<SecretInputHandler>();
            if (secret != null)
                secret.ResetKonami();
    }
}
