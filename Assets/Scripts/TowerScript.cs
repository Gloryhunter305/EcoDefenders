using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false, beenPlaced = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)
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
        if (!beenPlaced)
        {
            isDragging = true;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
            offset = transform.position - mainCamera.ScreenToWorldPoint(mousePosition);
        }
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse is down.");
        isDragging = false;
        beenPlaced = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse is up.");
        // Vector3 mousePosition = Input.mousePosition;
        // Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // mousePosition.z = 0;

        // gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, mousePosition.z);
    }
}
