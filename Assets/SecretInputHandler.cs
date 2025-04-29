using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretInputHandler : MonoBehaviour
{
    private List<KeyCode> konamiCode = new List<KeyCode>
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A, KeyCode.Return
    };
    private List<KeyCode> inputBuffer = new List<KeyCode>();

    [Header("???")]
    public GameObject konamiPopup;
    private bool isPopupOpen = false;
    [SerializeField] public ManagerGame managerGame; //Since ManagerGame isn't a Singleton
    [SerializeField] private string hardModeSceneName = "MainScene_Hard";

    private void Update()
    {
        if (managerGame != null && managerGame.GetGamePhase() != ManagerGame.GamePhase.Building)   return;
        
        else if (Input.anyKeyDown)
        {
            foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    inputBuffer.Add(key);

                    if (inputBuffer.Count > konamiCode.Count)
                    inputBuffer.RemoveAt(0);

                    //Debug.Log("Current Input Buffer: " + string.Join(", ", inputBuffer));
                    CheckKonamiCode();
                    break;
                }
            }
        }  

        if (isPopupOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        } 
    }

    private void CheckKonamiCode()
    {
        if (inputBuffer.Count != konamiCode.Count) return;

        for (int i = 0; i < konamiCode.Count; i++)
        {
            if (inputBuffer[i] != konamiCode[i])
                return;
        }

        konamiPopup.SetActive(true);
        isPopupOpen = true;
        Time.timeScale = 0; // Pause the game
    }
    public void ActivateHardMode()
    {
        Debug.Log("Hard mode activated!");
        // You can set a flag here and pass it to ManagerGame later
        konamiPopup.SetActive(false);
        Time.timeScale = 1;
        isPopupOpen = false;
    }

    public void ClosePopup()
    {
        konamiPopup.SetActive(false);
        Time.timeScale = 1;
        isPopupOpen = false;
        Debug.Log("Hard mode canceled.");
    }

    public void LoadHardModeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(hardModeSceneName);
    }

    public void ResetKonami()
    {
        konamiPopup.SetActive(false);
    }
}
