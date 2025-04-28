using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public float spawnInterval;
    public List<SpawnRoute> spawnRoutes;
}

[System.Serializable]
public class SpawnRoute
{
    public Transform spawnPoint;
    public Transform[] path;
}

[System.Serializable]
public class Wave
{
    public int enemyType1Count;
    public int enemyType2Count;
    public int enemyType3Count;
    public float waveInterval;      //?
}
public class ManagerGame : MonoBehaviour
{
    public EnemyType[] enemyTypes;
    public Wave[] waves;
    public SubMeters[] subMeters;

    [Header("Wave Manager")]
    public TextMeshProUGUI nextWaveText;
    public TextMeshProUGUI enemiesInWavesText;
    private int currentWaveIndex = 0;
    public bool isWaveActive = false;

    [Header("Game And Tower Cameras")]
    [SerializeField] private Camera gameCamera;     //Game Camera zooms in the scene and displays the UI stuff for the player to see
    [SerializeField] private Camera towerCamera;

    [Header("Canvas Transition")]
    public GameObject[] canvases;

    private List<GameObject> activeEnemies = new List<GameObject>();

    [Header("Enemy Water Sprites")]
    [SerializeField] private Sprite[] riverSprites;

    //Audio Manager stuff
    public GamePhase currentPhase;
    void Start()
    {
        UpdateWaveText();   
        UpdateEnemiesInWaveText();

        foreach(GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }

        if (canvases.Length > 0)
        {
            canvases[0].SetActive(true);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isWaveActive && currentWaveIndex < waves.Length)
        {
            if (activeEnemies.Count == 0)
            {
                //Start of the game begins here...
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }   
        else if (currentWaveIndex >= waves.Length)
        {
            FindAnyObjectByType<SustainabilityMeter>().PlayerWins();
        }
    }

    // private IEnumerator SpawnWaves()
    // {
    //     isWaveActive = true;
        
    //     while (currentWaveIndex < waves.Length)
    //     {
    //         yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
    //         currentWaveIndex++;
    //         yield return new WaitForSeconds(waves[currentWaveIndex - 1].waveInterval);
    //     }
    //     isWaveActive = false;
    // }

    private IEnumerator SpawnWave(Wave wave)
    {
        StartingGamePhase();    //Camera transitioning stuff

        // Spawn enemyType1     CO2
        for (int i = 0; i < wave.enemyType1Count; i++)
        {
            SpawnEnemy(0); // Assuming enemyType1 is at index 0 
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }

        // Spawn enemyType2     RIVER 
        for (int i = 0; i < wave.enemyType2Count; i++)
        {
            SpawnEnemy(1); // Assuming enemyType2 is at index 1 
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }

        // Spawn enemyType3     WHEAT
        for (int i = 0; i < wave.enemyType3Count; i++)
        {
            SpawnEnemy(2); // Assuming enemyType3 is at index 2
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }
    
        yield return new WaitUntil(() => activeEnemies.Count == 0);     //Wait until all enemies are gone from game
        yield return new WaitForSeconds(2f);    //Wait for two seconds to show user what they lost in the battle

        GettingReadyForNextWave();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));  //Press space to start wave
    }
    private List<GameObject> SpawnEnemy(int enemyTypeIndex)
    {
        List<GameObject> spawned = new List<GameObject>();
        EnemyType type = enemyTypes[enemyTypeIndex];

        foreach (var route in type.spawnRoutes)
        {
            // Instantiate the enemy at the spawn point
            GameObject enemy = Instantiate(type.enemyPrefab, route.spawnPoint.position, Quaternion.identity);

            if (enemyTypeIndex == 1 && riverSprites.Length > 0)        //River enemy
            {
                SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = riverSprites[Random.Range(0, riverSprites.Length)];
                }
            }

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.path = route.path;
            //enemyScript.path = enemyTypes[enemyTypeIndex].path; // Assign the path for this enemy type
            
            enemyScript.OnEnemyDestroyed += () => activeEnemies.Remove(enemy);
            activeEnemies.Add(enemy);   
            spawned.Add(enemy);
        }
        return spawned;
    }

    public void ResetSubMeters()
    {
        foreach (SubMeters subMeter in subMeters)
        {
            if (subMeter != null)
            {
                subMeter.ResetSubMeter();
            }
        }
    }

    private void UpdateWaveText()
    {
        nextWaveText.text = "Wave " + (currentWaveIndex + 1);       //Instead of Wave 0, Wave 1
    }

    private void UpdateEnemiesInWaveText()
    {
        int co2 = waves[currentWaveIndex].enemyType1Count;
        int water = waves[currentWaveIndex].enemyType2Count;
        int wheat = waves[currentWaveIndex].enemyType3Count;
        
        enemiesInWavesText.text = " CO2: " + co2 + " Water: " + water + " Crop: " + wheat;
    }

    private void GettingReadyForNextWave()
    {
        StorageManager storageManager = FindFirstObjectByType<StorageManager>();
        isWaveActive = false;
        towerCamera.enabled = true;     //Make sure that it switches to towerCamera to display tower panel
        gameCamera.enabled = false;
        canvases[0].SetActive(true);    //Tower UI
        canvases[1].SetActive(false);   //Game UI

        
        storageManager.IncreaseMaxStorage(1);  //Increasing Max Storage
        ResetSubMeters();
        currentWaveIndex++;
        if (currentWaveIndex < waves.Length)
        {
            UpdateWaveText();
            UpdateEnemiesInWaveText();
        } 

        SetGamePhase(GamePhase.Building);
    }

    private void StartingGamePhase()
    {
        isWaveActive = true;
        towerCamera.enabled = false;
        gameCamera.enabled = true;
        canvases[0].SetActive(false);    //Tower UI
        canvases[1].SetActive(true);   //Game UI

        //Audio background music
        SetGamePhase(GamePhase.Playing);
    }

    public void SetGamePhase(GamePhase phase)
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        currentPhase = phase;

        if (phase == GamePhase.Building)
        {
            audioManager.PlayMusic(audioManager.buildBackgroundMusic);
        }
        else if (phase == GamePhase.Playing)
        {
            audioManager.PlayMusic(audioManager.waveBackgroundMusic);
        }
    }

    /*      Audio Stuff GOES HERE       */
    public enum GamePhase
    {
        Building,
        Playing
    }

    public enum SFXTypes
    {
        CO2damage,
        CO2death,
        WATERdamage,
        WATERdeath,
        WHEATdamage,
        WHEATdeath,
        METERbreak,
        LABdamage,
        VACUUMshoot,
        SNIPERshoot
    }
}
