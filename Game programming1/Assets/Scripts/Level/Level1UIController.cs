using UnityEngine;

public class Level1UIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject OtherPanel;


    [Header("References")]
    public StartMoving raceManager;
    public BuildController buildController;
    public Level1Finish level1Finish;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void OnStartButtonPointerDown()
    {
        if (buildController != null)
            buildController.enabled = false;
    }

    public void OnStartButtonClicked()
    {
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

    public void Otherpanel()
    {
        if (OtherPanel != null)
            OtherPanel.SetActive(true);
    }

    public void ShowWinUI()
    {
        if (winPanel != null) 
            winPanel.SetActive(true);
        if (losePanel != null) 
            losePanel.SetActive(false);
        if (OtherPanel != null)
            OtherPanel.SetActive(false);
    }

    public void ShowLoseUI()
    {
        if (losePanel != null) 
            losePanel.SetActive(true);
        if (winPanel != null) 
            winPanel.SetActive(false);
        if (OtherPanel != null)
            OtherPanel.SetActive(false);
    }
}
