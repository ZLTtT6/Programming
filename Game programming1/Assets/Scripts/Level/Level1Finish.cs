using UnityEngine;

public class Level1Finish : MonoBehaviour
{
    [Header("Ball")]
    public Wallrebound fastBall;
    public Wallrebound slowBall;

    [Header("UI")]
    public Level1UIController Level1UIController;

    private bool hasFirstArrived = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasFirstArrived) 
            return;
        if (other == null) 
            return;
        Wallrebound ball = other.GetComponentInParent<Wallrebound>();
        if (ball == null) 
            return;

        if (ball != fastBall && ball != slowBall) 
            return;

        hasFirstArrived = true;

        if (ball == fastBall)
        {
            Level1UIController.ShowWinUI();
            Time.timeScale = 0;
        }
        else
        {
            Level1UIController.ShowLoseUI();
            Time.timeScale = 0;
        }
    }

    public void ResetFinish()
    {
        hasFirstArrived = false;
    }
}
