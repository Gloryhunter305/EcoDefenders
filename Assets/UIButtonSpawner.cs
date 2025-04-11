using UnityEngine;
using UnityEngine.UI;

public class UIButtonSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // prefab to spawn
    public MouseCursorScript mouseCursor;  // reference to MouseCursor script

    private ManagerGame gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<ManagerGame>();   
    }

    public void OnButtonClick()
    {
        if (mouseCursor != null && objectToSpawn != null && !gameManager.isWaveActive)
        {
            GameObject spawned = Instantiate(objectToSpawn);
            mouseCursor.SetObjectToFollow(spawned);
        }
    }
}
