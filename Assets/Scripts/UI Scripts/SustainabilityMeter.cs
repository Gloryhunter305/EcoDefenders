using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SustainabilityMeter : MonoBehaviour
{
    public int maxHealth = 5;   //Max hits before losing game altogether
    private int currentHealth;

    public Image sustainabilityImage;
    public Image sustainabilityTracker;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private bool GameOverFirst = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateSustainabilityUI();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public int getSustainabilityHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateSustainabilityUI();
    }

    private void UpdateSustainabilityUI()
    {
        if (sustainabilityImage != null)
        {
            sustainabilityImage.fillAmount = (float) currentHealth / maxHealth;
            sustainabilityTracker.fillAmount = (float) currentHealth / maxHealth;
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public bool wasGameOverFirst()
    {
        return GameOverFirst;
    }
    public void setGameOverFirst(bool final)
    {
        GameOverFirst = final;
    }

    private void ShowVictory()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        if (GameResetManager.instance != null)
        {
            GameResetManager.instance.ResetGame();  // Custom reset method you implement
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");

    }

    public void ResetMeter()
    {
        currentHealth = maxHealth;
        GameOverFirst = false;
        UpdateSustainabilityUI();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void PlayerWins()
    {
        ShowVictory();
    }
}
