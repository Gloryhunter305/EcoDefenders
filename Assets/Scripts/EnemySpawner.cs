using System.Collections;
using UnityEngine;
using static WaveManager;

public class EnemySpawner : MonoBehaviour
{
    //Spawner Components
    
    public Transform[] wayPoints;
    public float spawnInterval = 2f;

    //Class Components
    private WaveManager waveManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
        
        if (waveManager == null)
        {
            Debug.LogError("WaveManager isn't found in scene");
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Wave currentWaveData = waveManager.GetEnemyCountForCurrentWave();
            StartCoroutine(SpawnEnemies(currentWaveData));
        }
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        foreach (EnemySpawnData enemyData in wave.enemiesToSpawn)
        {
            
            for (int i = 0; i < enemyData.count; i++)
            {
                GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPoint.position, Quaternion.identity);
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                enemyScript.SetWaypoints(wayPoints);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        waveManager.AdvancetoNextWave();
    }
}
