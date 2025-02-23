using UnityEngine;

public class HomeBase : MonoBehaviour
{

    //Laboratory Variables
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Home Base took about: " + damage);
    }
}
