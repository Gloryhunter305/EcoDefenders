using UnityEngine;
using TMPro;

public class UIStorage : MonoBehaviour
{
    public TextMeshProUGUI storageText;
    
    // Update is called once per frame
    void Update()
    {
        int currentStorage = StorageManager.Instance.getCurrentStorage();
        int maxStorage = StorageManager.Instance.maxStorage;    //Isn't a private variable, so just grab it
        storageText.text = "Tower Storage: " + currentStorage + " /" + maxStorage; 
    }
}
