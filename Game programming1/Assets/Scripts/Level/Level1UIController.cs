using UnityEngine;

public class Level1UIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject OtherPanel;

    private bool isPaused = false;


    [Header("References")]
    public StartMoving raceManager;
    public BuildController buildController;
    public Level1Finish level1Finish;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void OnStartButtonPointerDown()
    {

        if (buildController != null)
            buildController.enabled = false;
    }

    public void OnStartButtonClicked()
    {
        AudioManager.Instance?.PlayButtonClickSFX(); 
        AudioManager.Instance?.PlayGameplayMusic();
        if (buildController != null)
            buildController.enabled = false;

        Time.timeScale = 1f;

        if (winPanel != null) 
            winPanel.SetActive(false);
        if (losePanel != null) 
            losePanel.SetActive(false);
        if (level1Finish != null) 
            level1Finish.ResetFinish();
        if (raceManager != null)
            raceManager.StartRace();
        if (OtherPanel != null)
            OtherPanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (OtherPanel == null) return;

        isPaused = !isPaused;
        OtherPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        AudioManager.Instance?.PlayButtonClickSFX();
    }

    public void Resume()
    {
        if (OtherPanel == null) return;

        isPaused = false;
        OtherPanel.SetActive(false);
        Time.timeScale = 1f;

        AudioManager.Instance?.PlayButtonClickSFX();
    }

    public void ShowWinUI()
    {
        if (winPanel != null) 
            winPanel.SetActive(true);
        if (losePanel != null) 
            losePanel.SetActive(false);
        if (OtherPanel != null)
            OtherPanel.SetActive(false);
        AudioManager.Instance?.PlayWinMusic();
    }

    public void ShowLoseUI()
    {
        if (losePanel != null) 
            losePanel.SetActive(true);
        if (winPanel != null) 
            winPanel.SetActive(false);
        if (OtherPanel != null)
            OtherPanel.SetActive(false);
        AudioManager.Instance?.PlayLoseMusic();

    }
}
