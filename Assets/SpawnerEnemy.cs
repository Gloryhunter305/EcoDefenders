using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    //Waypoints for each spawnPoint
    public Transform[] wayPointsForSpawnPoint1;
    public Transform[] wayPointsForSpawnPoint2;
    public Transform[] wayPointsForSpawnPoint3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.path = GetPathForSpawnPoint(spawnIndex);
    }

    Transform[] GetPathForSpawnPoint(int spawnIndex)
    {
        if (spawnIndex == 0)
        {
            return wayPointsForSpawnPoint1;
        }
        else if (spawnIndex == 1)
        {
            return wayPointsForSpawnPoint2;
        }
        else
        {
            return wayPointsForSpawnPoint3;
        }
    }
}
