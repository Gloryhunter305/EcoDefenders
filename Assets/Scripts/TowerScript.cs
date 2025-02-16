using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false, beenPlaced = false, hoveringOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)     //Moving the tower using the mouse
        {
            // Update the position of the object based on mouse movement
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.WorldToScreenPoint(transform.position).z; // Get the z position
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(mousePosition) + offset;
            newPosition.z = 0; // Keep the z position at 0 for 2D
            transform.position = newPosition; // Set the new position
        }
    }

    private void OnMouseEnter()
    {
        if (!beenPlaced)    //When tower is instantiated from tower spawn
        {
            isDragging = true;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
            offset = transform.position - mainCamera.ScreenToWorldPoint(mousePosition);
        }
        else
        {
            hoveringOver = true;
            Debug.Log("Tower is placed.");
        }
        
    }

    private void OnMouseExit()
    {
        Transform textBox = transform.GetChild(0);
        if (textBox.gameObject.activeSelf)
        {
            textBox.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (isDragging)     //Placing the tower down into the scene
        {
            isDragging = false;
            beenPlaced = true;
        }
        if (hoveringOver)   //Hovering over tower over stats (information)
        {
            Transform textBox = transform.GetChild(0);

            textBox.gameObject.SetActive(true);
            Debug.Log("Tower is selected");
        }
    }

    private void OnMouseUp()
    {
        // Vector3 mousePosition = Input.mousePosition;
        // Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // mousePosition.z = 0;

        // gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, mousePosition.z);
    }
}
