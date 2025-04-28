using UnityEngine;
using TMPro;

public class UIStorage : MonoBehaviour
{
    public TextMeshProUGUI storageText;
    
    // Update is called once per frame
    void Update()
    {
        StorageManager storageManager = FindFirstObjectByType<StorageManager>();

        int currentStorage = storageManager.getCurrentStorage();
        int maxStorage = storageManager.maxStorage;    //Isn't a private variable, so just grab it
        storageText.text = "Tower Storage: " + currentStorage + " /" + maxStorage; 
    }
}
