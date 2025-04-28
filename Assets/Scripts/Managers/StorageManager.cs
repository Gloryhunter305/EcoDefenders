using System.Security.Cryptography;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public int maxStorage = 5;
    private int currentStorage;
    
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
