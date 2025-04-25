using System;
using UnityEngine;

public class MouseCursorScript : MonoBehaviour
{
    private GameObject objectFollowingMouse;
    private bool isPlacing = false;
    private bool onTopOfTower = false;

    public Camera towerCamera;

    //Mouse is always checking if mouse is on enemy's path
    [SerializeField] private LayerMask pathLayer;

    void Update()
    {
        if (objectFollowingMouse != null)
        {
            // Follow the mouse
            Vector3 mouseWorld = towerCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f; // 2D only
            objectFollowingMouse.transform.position = mouseWorld;

            SpriteRenderer SR = objectFollowingMouse.GetComponent<SpriteRenderer>();
            HeadTower headTower = objectFollowingMouse.GetComponent<HeadTower>();
            
            if (IsOverlappingTower(objectFollowingMouse))
            {
                SR.color = Color.red;   //Don't need to call tower, red is usually a good indicator
                onTopOfTower = true;
            }
            else
            {
                SR.color = headTower.originalColor;  //Tower has original color stored so calling tower
                onTopOfTower = false;
            }

            // Place it with left-click
            if (Input.GetMouseButtonDown(0))
            {
                if (OnTopOfPath(mouseWorld) && !onTopOfTower)       //CanPlace()
                {
                    if (StorageManager.Instance.CanPlace(headTower.storageCost))
                    {
                        StorageManager.Instance.AddStorage(headTower.storageCost);
                        objectFollowingMouse = null;    //Neccesary code to make sure object is removed from mouse's code
                        isPlacing = false;      //Gameobject has been placed
                    }
                    else
                    {
                        Debug.Log("Not enough storage to place this tower.");
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))   //Right Click to Cancel / Destory tower (isPlacing == true)
            {
                CancelObjectFollowing(objectFollowingMouse); 
            }   
        }
    }

    public void SetObjectToFollow(GameObject obj)
    {
        objectFollowingMouse = obj;
        isPlacing = true;
    }

    public void CancelObjectFollowing(GameObject obj)
    {
        objectFollowingMouse = null;
        isPlacing = false;
        Destroy(obj);
    }

    public bool getTowerState()
    {
        return isPlacing;
    }

    //Check if mouse is on top of Path 
    public bool OnTopOfPath(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 1f, pathLayer);
    }
    public bool IsOverlappingTower(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        bool isOverlapping = false;

        foreach (Collider2D col in colliders)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = false;

            Collider2D[] results = new Collider2D[10];
            int count = col.Overlap(filter, results);

            for (int i = 0; i < count; i++)
            {
                if (results[i] != null && results[i].gameObject != obj)
                {
                    //Debug.Log($"Overlapping with: {results[i].gameObject.name}");
                    isOverlapping = true;

                    Debug.DrawLine(col.transform.position, results[i].transform.position, Color.red, 0.1f);
                }
            }
        }

        return isOverlapping;
        }
}
