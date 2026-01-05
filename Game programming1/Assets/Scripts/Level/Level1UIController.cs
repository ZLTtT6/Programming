using UnityEngine;

public class Level1UIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject startButton;
    public GameObject finishPanel;

    [Header("References")]
    public StartMoving raceManager;
    public BuildController buildController;

    void Start()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }

        if (raceManager != null)
        {
            raceManager.RaceFinished += OnRaceFinished;
        }
    }

    void OnDestroy()
    {
        if (raceManager != null)
        {
            raceManager.RaceFinished -= OnRaceFinished;
        }
    }

    public void OnStartButtonPointerDown()
    {
        if (buildController != null)
        {
            buildController.enabled = false;
        }
    }

    public void OnStartButtonClicked()
    {
        if (buildController != null)
        {
            buildController.enabled = false;
        }

        Time.timeScale = 1f;

        if (raceManager != null)
        {
            raceManager.StartRace();
        }

        if (startButton != null)
        {
            startButton.SetActive(false);
        }

        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }
    }

    public void ShowStartButton()
    {
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    void OnRaceFinished()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
        }

        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }
}
