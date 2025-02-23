using System.Collections.Specialized;
using NUnit.Framework.Internal;
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
    private int currentWaypointIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.bodyType = RigidbodyType2D.Dynamic;

        transform.position = wayPoints[0].position; //First waypoint initialize
    }

    void FixedUpdate()
    {
        if (currentWaypointIndex < wayPoints.Length)
        {
            MoveToWayPoint();
        }
    }

    void MoveToWayPoint()
    {
        Vector2 targetPosition = wayPoints[currentWaypointIndex].position;
        //Speed will be tweaked to 
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
            collision.gameObject.GetComponent<HomeBase>().TakeDamage(damage);
            Destroy(gameObject);
        }   
    }
}
