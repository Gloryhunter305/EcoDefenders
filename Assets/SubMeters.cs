using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SubMeters : MonoBehaviour
{
    public int maxHealth = 5;
    [SerializeField] private int currentHealth;

    public string subMeterName;
    public SustainabilityMeter sustainabilityMeter;
    public Image meterImage;
    

    private int hitCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;   
        UpdateMeterUI();
    }

    public void TakeDamage(int damage)      //UPDATES UI STUFF
    {
        currentHealth -= damage;
        //Debug.Log(subMeterName + " has taken 1 damage");
        UpdateMeterUI();
    }

    public void HitByEnemy()
    {
        hitCount++;
        if (hitCount < 5)   //gameobject exists
        {
            TakeDamage(1);
        }

        if (hitCount == 5)
        {
            BreakSubMeter();
        }
    }

    private void BreakSubMeter()
    {
        sustainabilityMeter.TakeDamage(1);
        meterImage.gameObject.SetActive(false);
    }

    public void ResetSubMeter()
    {
        currentHealth = maxHealth;
        hitCount = 0;
        meterImage.gameObject.SetActive(true);
        UpdateMeterUI();
    }

    private void UpdateMeterUI()
    {
        if (meterImage != null)
        {
            meterImage.fillAmount = (float) currentHealth / maxHealth;
        }
    }
}
