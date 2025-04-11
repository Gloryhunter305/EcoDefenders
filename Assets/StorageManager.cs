using System.Security.Cryptography;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    public int maxStorage = 5;
    private int currentStorage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }   
        Instance = this;
    }
    
    public bool CanPlace(int storageCost)
    {
        return (currentStorage + storageCost) <= maxStorage;
    }

    public void AddStorage(int storageCost)
    {
        currentStorage += storageCost;
        Debug.Log($"Storage used: {currentStorage}/{maxStorage}");
    }

    public int getCurrentStorage()
    {
        return currentStorage;
    }

    public void RemoveStorage(int storageCost)
    {
        currentStorage -= storageCost;
        currentStorage = Mathf.Max(currentStorage, 0);
    }

    public void IncreaseMaxStorage(int increase)
    {
        maxStorage += increase;
    }
}
