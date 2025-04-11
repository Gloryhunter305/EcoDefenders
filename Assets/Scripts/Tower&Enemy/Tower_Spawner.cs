using UnityEngine;

public class Tower_Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;    //prefab you want to spawn
    private GameObject currentObject;
    private Camera mainCamera;
    [SerializeField] private ManagerGame managerGame;
    // [SerializeField] private TowerPlacement managerTowerPlacer;
    [SerializeField] private LayerMask pathLayer;

    //Grab path's position
    //public GameObject path;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerGame = FindFirstObjectByType<ManagerGame>();
        //managerTowerPlacer = FindFirstObjectByType<TowerPlacement>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (currentObject != null)  //3. Since tower isn't null, it starts following the mouse
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0))    //4. Wait for user to place the tower down in the Scene
            {
                Debug.Log(currentObject.transform.position);        //What if I saved this position somewhere? Then use distance in Tower
                Debug.Log("Placing object...");
                PlaceObject();
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(currentObject);
            }
        }   
    }

    public void StartPlacing()  //2. If wave isn't active, spawn the Tower
    {
        if (!managerGame.isWaveActive)
            currentObject = Instantiate(objectToSpawn);
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        
        currentObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }

    private void PlaceObject() //5. If the object can be placed in the right spot, then currentObject is null because it's placed and doesn't follow the mouse anymore
    {
        Vector3 position = currentObject.transform.position;

        // // Check if the position is valid
        if (OnTopOfPath(position))
        {
            Tower namedTower = objectToSpawn.GetComponent<Tower>();
            if (namedTower != null)
            {
                Debug.Log("Tower is found");
            }
            else
            {
                Debug.LogWarning("Tower isn't found");
            }
            currentObject = null; // Tower is placed, stop following the mouse
        }
        else
        {
            Debug.Log("Invalid placement! Cannot place tower here.");
        }
    }

    private bool IsPositionValid(Vector3 position)      //MIGHT USE DISTANCE METHOD 
    {
        // Vector3 pathPos = path.transform.position;

        // float distance = Vector3.Distance(position, pathPos);

        // Debug.Log("Distance: " + distance);

        // if (distance < 1f)
        // {
        //     Destroy(currentObject);
        // }
        // else
        // {
        //     currentObject = null;
        // }
        
        return !Physics.CheckSphere(position, 10f, LayerMask.GetMask("Tower"));
    }

    private bool OnTopOfPath(Vector3 position)      //Path works becuase pathways have a box collider
    {
        return !Physics2D.OverlapCircle(position, 1f, pathLayer);
    }

    public void OnButtonClick()     //1. Player clicks the button
    {
        Debug.Log("Dragging tower.");
        StartPlacing();
    }
}
