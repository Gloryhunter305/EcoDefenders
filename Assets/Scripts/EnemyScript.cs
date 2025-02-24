using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    /*  GameObject Components   */
    private Rigidbody2D RB;
    public SpriteRenderer SR;
    public Transform[] wayPoints;

    //Enemy Variables
    public float speed;
    public int health;
    public int damage;

    /*  Miscellaneous variables */
    private int currentWaypointIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.bodyType = RigidbodyType2D.Dynamic;

        if (wayPoints == null || wayPoints.Length == 0)
        {
            Debug.LogError("Waypoints not assigned to enemy");
            return;
        }

        // transform.position = wayPoints[0].position; //First waypoint initialize
    }

    void FixedUpdate()
    {
        if (wayPoints != null && currentWaypointIndex < wayPoints.Length)
        {
            MoveToWayPoint();
        }
    }

    public void SetWaypoints(Transform[] newWayPoints)
    {
        //I need to divide these waypoints by the enemyType/ locations 
        wayPoints = newWayPoints;
        currentWaypointIndex = 0;
    }

    void MoveToWayPoint()
    {
        if (wayPoints == null || wayPoints.Length == 0) return;

        Vector2 targetPosition = wayPoints[currentWaypointIndex].position;
        Vector2 newPosition = Vector2.MoveTowards(RB.position, targetPosition, speed * Time.fixedDeltaTime);   
        RB.MovePosition(newPosition);

        if (Vector2.Distance(RB.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HomeBase"))
        {
            HomeBase baseScript = collision.gameObject.GetComponent<HomeBase>();

            if (baseScript != null)
            {
                baseScript.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }   
    }
}
