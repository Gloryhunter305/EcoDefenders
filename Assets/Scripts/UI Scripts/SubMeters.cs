using UnityEngine;
using UnityEngine.UI;
public class SubMeters : MonoBehaviour
{
    public int maxHealth = 20;
    [SerializeField] private int currentHealth;     //Current health of submeter
    public SustainabilityMeter sustainabilityMeter;
    public Image meterImage;

    public Enemy.EnemyType enemyType;

    private bool meterBroken = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;   
        UpdateMeterUI();
    }

    public void TakeDamageToMeter(int damage)      //UPDATES UI STUFF
    {
        currentHealth -= damage;
        UpdateMeterUI();
    }

    public void HitByEnemy()
    {
        //Do damage first then check if the sub-meter survives
        //Take damage assigned by the enemy type
        switch (enemyType)
        {
            case Enemy.EnemyType.PowerPlant:
                TakeDamageToMeter(5);
                break;
            case Enemy.EnemyType.River:
                TakeDamageToMeter(4);
                break;
            case Enemy.EnemyType.Field:
                TakeDamageToMeter(10);
                break;
        }

        if (currentHealth <= 0 && !meterBroken)
        {
            meterBroken = true; 
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
        meterBroken = false;
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
