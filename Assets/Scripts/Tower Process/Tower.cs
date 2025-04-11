using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Components")]
    public Transform target;    //Closest enemy targetted first
    public float rotationSpeed = 5f;
    public float visionRange;
    public int storageCost;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireCooldown = 0f;

    [Header("Vision Range")] 
    [SerializeField] private GameObject rangeCircle;   

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
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0,0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);   
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
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletScript>().SetTarget(target);
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

    private void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.red;
        // Draw a wire sphere to represent the radius
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

}
