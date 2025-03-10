using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public Transform[] path;
    public float spawnInterval;
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
    public Transform[] spawnPoints;
    public Wave[] waves;
    public SubMeters[] subMeters;

    public int currentWaveIndex = 0;
    public bool isWaveActive = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isWaveActive && currentWaveIndex < waves.Length)
        {
            if (activeEnemies.Count == 0)
            {
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
        isWaveActive = true;
        // Spawn enemyType1
        for (int i = 0; i < wave.enemyType1Count; i++)
        {
            GameObject enemy = SpawnEnemy(0); // Assuming enemyType1 is at index 0
            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }

        // Spawn enemyType2
        for (int i = 0; i < wave.enemyType2Count; i++)
        {
            GameObject enemy = SpawnEnemy(1); // Assuming enemyType2 is at index 1
            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }

        // Spawn enemyType3
        for (int i = 0; i < wave.enemyType3Count; i++)
        {
            GameObject enemy = SpawnEnemy(2); // Assuming enemyType3 is at index 2
            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(1f); // Optional delay between spawns
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitUntil(() => activeEnemies.Count == 0);
        
        currentWaveIndex++;
        ResetSubMeters();
        isWaveActive = false;
    }
    private GameObject SpawnEnemy(int enemyTypeIndex)
    {
        // Get the spawn point for this enemy type
        Transform spawnPoint = spawnPoints[enemyTypeIndex];

        // Instantiate the enemy at the spawn point
        GameObject enemy = Instantiate(enemyTypes[enemyTypeIndex].enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.path = enemyTypes[enemyTypeIndex].path; // Assign the path for this enemy type

        enemyScript.OnEnemyDestroyed += () => activeEnemies.Remove(enemy);

        return enemy;
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
}
