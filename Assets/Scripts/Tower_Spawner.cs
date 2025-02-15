using UnityEngine;

public class Tower_Spawner : MonoBehaviour
{

    public GameObject objectToSpawn;    //prefab you want to spawn
    private GameObject currentObject;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    
    [SerializeField] private Color originalColor;
    [SerializeField] private Color hoveredColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (currentObject != null)  //If there are no objects in the scene
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Placing object...");
                PlaceObject();
            }
        }   
    }

    public void StartPlacing()
    {
        currentObject = Instantiate(objectToSpawn);
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        
        currentObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }

    private void PlaceObject()
    {
        currentObject = null;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = hoveredColor;
    }
    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    void OnMouseDown()
    {
        StartPlacing();
    }
}
