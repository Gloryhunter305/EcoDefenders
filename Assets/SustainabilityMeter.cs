using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class SustainabilityMeter : MonoBehaviour
{
    public int maxHealth = 5;   //Max hits before losing game altogether
    private int currentHealth;

    public Image sustainabilityImage;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateSustainabilityUI();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameOver();
        }
        UpdateSustainabilityUI();
    }

    private void UpdateSustainabilityUI()
    {
        if (sustainabilityImage != null)
        {
            sustainabilityImage.fillAmount = (float) currentHealth / maxHealth;
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShowVictory()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void PlayerWins()
    {
        ShowVictory();
    }
}
