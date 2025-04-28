using UnityEngine;

public class HeadTower : MonoBehaviour
{
    [Header("Frontend Tower Components")]
    public SpriteRenderer towerSpirte;
    public Color originalColor;
    public int storageCost;

    private MouseCursorScript mouseScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerSpirte = GetComponent<SpriteRenderer>();
        mouseScript = FindFirstObjectByType<MouseCursorScript>();
    }

    public void Remove()
    {
        StorageManager storageManager = FindFirstObjectByType<StorageManager>();
        
        if (!mouseScript.getTowerState())
        {  //If isPlacing == false, means tower's already placed
            storageManager.RemoveStorage(storageCost);
        }
        Destroy(gameObject);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) )     
        {
            Remove();
        }
    }
}
