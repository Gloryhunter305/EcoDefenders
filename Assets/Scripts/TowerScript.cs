using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false; 
    public bool beenPlaced = false;

    private LayerMask pathLayer;
    [SerializeField] private Tower tower;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tower = GetComponent<Tower>();
        mainCamera = Camera.main;
        pathLayer = LayerMask.GetMask("Path");
        tower.towerRadius.transform.localScale = new Vector3(tower.visionRange * 2f, tower.visionRange * 2f, 1f);  
    }

    void Update()
    {
        if (isDragging)     //Moving the tower using the mouse
        {
            FollowMouse();
        }
    }

    private void OnMouseEnter()
    {
        if (!beenPlaced)    //When tower is instantiated from tower spawn
        {
            isDragging = true;
        }
        else
        {
            Debug.Log("Mouse entered tower's range");
        }
        
    }

    // private void OnMouseExit()
    // {
    //     Transform textBox = transform.GetChild(0);
    //     if (textBox.gameObject.activeSelf)
    //     {
    //         textBox.gameObject.SetActive(false);
    //     }
    // }

    private void OnMouseDown()
    {
        if (isDragging)     //Placing the tower down into the scene
        {
            if (IsValidPlacement())
            {
                isDragging = false;
                beenPlaced = true;
                tower.towerRadius.SetActive(false);
            }
            
        }

        // if (hoveringOver)   //Hovering over tower over stats (information)
        // {
        //     Transform textBox = transform.GetChild(0);

        //     textBox.gameObject.SetActive(true);
        //     Debug.Log("Tower is selected");
        // }
    }

    private bool IsValidPlacement()
    {
        // Check if the current position is on the path layer
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, pathLayer);
        return hit == null; // Return true if no collider is found on the path layer
    }

    // private void ResetTowerPosition()
    // {
    //     transform.position = originalPosition; // Uncomment and set originalPosition accordingly
    // }

    private void OnMouseUp()
    {
        // Vector3 mousePosition = Input.mousePosition;
        // Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // mousePosition.z = 0;

        // gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, mousePosition.z);
    }

    private void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
