using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed = 10f;
    
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (Vector2) target.position - (Vector2) transform.position;
        transform.position += (Vector3) direction.normalized * bulletSpeed * Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
