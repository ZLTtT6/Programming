using UnityEngine;

public class Level1Finish : MonoBehaviour
{
    [Header("Ball")]
    public Wallrebound fastBall;
    public Wallrebound slowBall;

    [Header("UI")]
    public Level1UIController level1UIController;

    [Header("Which level is this?")]
    public int currentLevelIndex = 1;

    private bool hasFirstArrived = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasFirstArrived) return;
        if (other == null) return;

        Wallrebound ball = other.GetComponentInParent<Wallrebound>();
        if (ball == null) return;
        if (ball != fastBall && ball != slowBall) return;

        hasFirstArrived = true;

        if (ball == fastBall)
        {
            if (level1UIController != null) level1UIController.ShowWinUI();

            if (currentLevelIndex == 1)
                PlayerPrefs.SetInt("UnlockLevel2", 1);

            if (currentLevelIndex == 2)
                PlayerPrefs.SetInt("UnlockLevel3", 1);

            PlayerPrefs.Save();
            Time.timeScale = 0;
        }
        else
        {
            if (level1UIController != null) level1UIController.ShowLoseUI();
            Time.timeScale = 0;
        }
    }

    public void ResetFinish()
    {
        hasFirstArrived = false;
    }
}
