using UnityEngine;

public class Level1UIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject OtherPanel;
    public GameObject ModulePanel;

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
            Pause();
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
        buildController.HidePreview();
    }

    public void Pause()
    {
         if (OtherPanel == null) return;

    isPaused = true;
    OtherPanel.SetActive(true);
    ModulePanel.SetActive(false);

    Time.timeScale = 0f;

    if (buildController != null)
        buildController.SetBuildActive(false);

    AudioManager.Instance?.PlayButtonClickSFX();
    }

    public void Resume()
    {
        isPaused = false;
        OtherPanel.SetActive(false);
        ModulePanel.SetActive(true);

        Time.timeScale = 1f;

        if (buildController != null)
            buildController.SetBuildActive(true);

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
