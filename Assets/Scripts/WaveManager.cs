using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //This manager takes care of the waves that happen in the game
    //private const int endWave = 12;

    public class Wave
    {
        public List<EnemySpawnData> enemiesToSpawn;
        
        public Wave(List<EnemySpawnData> enemies)
        {
            enemiesToSpawn = enemies;
        }
        
    }
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int count;
        public Transform spawnPoint;
        public EnemySpawnData(GameObject enemyType, int spawnCount, Transform location)
        {
            enemyPrefab = enemyType;
            count = spawnCount;
            spawnPoint = location;
        }
    }

    [SerializeField] private int currentWave = 0;

    //Initializing Wave Data...
    List<Wave> waveData = new List<Wave>();

    [Header("Enemy Types")]
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;

    [Header("Spawn Locations")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    //Wave Data goes in here
    void Awake()
    {
        waveData = new List<Wave>
        {
            new Wave(new List<EnemySpawnData> {new EnemySpawnData(enemyType1, 2, spawnPoint1)}), 
            new Wave(new List<EnemySpawnData> {new EnemySpawnData(enemyType1, 4, spawnPoint1)}),
            new Wave(new List<EnemySpawnData> {new EnemySpawnData(enemyType1, 6, spawnPoint1), new EnemySpawnData(enemyType3, 3, spawnPoint3)}),
            new Wave(new List<EnemySpawnData> {new EnemySpawnData(enemyType3, 4, spawnPoint3), new EnemySpawnData(enemyType2, 2, spawnPoint2)}),
            new Wave(new List<EnemySpawnData> {new EnemySpawnData(enemyType1, 1, spawnPoint1), new EnemySpawnData(enemyType2, 1, spawnPoint2), new EnemySpawnData(enemyType3, 1, spawnPoint3)})
        
        };   
    }

    public Wave GetEnemyCountForCurrentWave()
    {
        if (currentWave < waveData.Count)
        {
            return waveData[currentWave];
        }
        return null;
    }

    public void AdvancetoNextWave()
    {
        if (currentWave < waveData.Count - 1)
        {
            currentWave++;
        }
        else
        {
            //This could be an bool statement that all enemies are 
            //spawned in the scene. Now, I need one bool to track if 
            //all enemies disappear from the scene
            Debug.Log("All waves completed.");  
        }
    }
}
