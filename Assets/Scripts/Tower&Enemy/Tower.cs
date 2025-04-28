using System.Collections.Generic;
using UnityEngine;
using static ManagerGame;

public class Tower : MonoBehaviour
{
    [Header("Tower Components")]
    public Transform target;    //Closest enemy targetted first
    [SerializeField] private float rotationSpeed = 360f;
    public float visionRange;
    public TowerType towerType;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private int damage = 1;
    public float fireRate = 1f;
    private float fireCooldown = 0f;

    [Header("Vision Range")] 
    [SerializeField] private GameObject rangeCircle;   
    public GameObject towerNozzle;

    [Header("Wait to be Spawned in Scene")]
    [SerializeField] private ManagerGame gameManager;
    [SerializeField] private List<Transform> enemiesInRange = new List<Transform>();

    public void Start()
    {
        gameManager = FindFirstObjectByType<ManagerGame>();   
        rangeCircle.transform.localScale = new Vector3(visionRange, visionRange, 1f);  
    }
    void Update()
    {    
        UpdateTarget();

        if (target != null)
        {
            SpriteRenderer disableCircle = rangeCircle.GetComponent<SpriteRenderer>();
            disableCircle.enabled = false;
            RotateTowardsTarget();
            ShootingMechanic();
        } 
    }

    void UpdateTarget()
    {
        if (enemiesInRange.Count == 0)
        {
            target = null;
            return;
        }

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy;
    }

    void RotateTowardsTarget()
    {
        if (target == null) return;

        // Get direction from the nozzle to the target
        Vector2 direction = target.position - towerNozzle.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f); // Still adding the angle offset

        // Smoothly rotate nozzle toward target at rotationSpeed degrees per second
        towerNozzle.transform.rotation = Quaternion.RotateTowards(towerNozzle.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }   

    void ShootingMechanic()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();

            //Audio effect for towerType here.
            switch(towerType)
            {
                case TowerType.Vacuum:
                    audioManager.PlaySFX(SFXTypes.VACUUMshoot);
                    break;
                case TowerType.Sniper:
                    audioManager.PlaySFX(SFXTypes.SNIPERshoot);
                    break;
            }

            bullet.SetTarget(target);
            bullet.damage = damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.transform);
        }
    }

    public enum TowerType       //For sound effects for towers
    {
        Vacuum,
        Sniper
        //May add more towers in the future...
    }

    private void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.red;
        // Draw a wire sphere to represent the radius
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

}
