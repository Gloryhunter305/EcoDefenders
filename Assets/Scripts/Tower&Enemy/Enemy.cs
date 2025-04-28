using System;
using UnityEngine;
using static ManagerGame;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 10;
    private int currentHealth;
    public float speed = 2f;
    public EnemyType enemyType;
    private Transform target;

    public Transform[] path;
    private int currentWaypointIndex = 0;

    public event Action OnEnemyDestroyed;

    void Start()
    {
        switch (enemyType)      //Set Enemy Type to corresponding location
        {
            case EnemyType.PowerPlant:
                target = GameObject.FindGameObjectWithTag("PowerPlant").transform;
                break;
            case EnemyType.Field:
                target = GameObject.FindGameObjectWithTag("Field").transform;
                break;
            case EnemyType.River:
                target = GameObject.FindGameObjectWithTag("River").transform;
                break;
        }
        currentHealth = maxHealth;
        if (path == null || path.Length == 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypointIndex < path.Length)
        {
            MoveTowardsWayPoint();
        }
        else
        {
                //If the gameObejct makes its way to the LAB
            DoDamage();
            DestroyItself();
        }
    }

    private void MoveTowardsWayPoint()
    {
        Transform targetWayPoint = path[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWayPoint.position) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    public void TakeDamage(int damage)      //Bullet Script damages gameObject
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DoDamage()      //Does Damage to Submeters
    {
        SubMeters subMeter = null;

        switch (enemyType)
        {
            case EnemyType.PowerPlant:
                subMeter = GameObject.FindGameObjectWithTag("PowerPlant").GetComponent<SubMeters>();
                break;
            case EnemyType.Field:
                subMeter = GameObject.FindGameObjectWithTag("Field").GetComponent<SubMeters>();
                break;
            case EnemyType.River:
                subMeter = GameObject.FindGameObjectWithTag("River").GetComponent<SubMeters>();
                break;
        }
        if (subMeter != null)
        {
            subMeter.HitByEnemy();
        }
    }

    private void Die()      //Let's make this function useful by adding the sound effects here.
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        switch (enemyType)
        {
            case EnemyType.PowerPlant:
                audioManager.PlaySFX(SFXTypes.CO2death);
                break;
            case EnemyType.River:
                audioManager.PlaySFX(SFXTypes.WATERdeath);
                break;
            case EnemyType.Field:
                audioManager.PlaySFX(SFXTypes.WHEATdeath);
                break;

        }
        DestroyItself();
    }

    public void DestroyItself()
    {
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }

    public enum EnemyType
    {
        PowerPlant, 
        Field,
        River
    }
}
