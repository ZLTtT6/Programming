using UnityEngine;
using System;

public class StartMoving : MonoBehaviour
{
    [Header("Ball GameObjects")]
    public Wallrebound fastBall;
    public Wallrebound slowBall;

    [Header("Start Direction")]
    public Vector3 startDirection = Vector3.forward;

    [Header("Finish Rule")]
    public bool endFinishes = true;

    public event Action RaceStarted;
    public event Action RaceFinished;

    private bool isRacing = false;
    private int finishedCount = 0;
    private int totalBalls = 0;

    void Awake()
    {
        totalBalls = 0;

        if (fastBall != null)
        {
            totalBalls += 1;
            fastBall.ReachedFinish += OnBallReachedFinish;
        }

        if (slowBall != null)
        {
            totalBalls += 1;
            slowBall.ReachedFinish += OnBallReachedFinish;
        }
    }

    void Start()
    {
        FreezeAll();
    }

    void OnDestroy()
    {
        if (fastBall != null)
        {
            fastBall.ReachedFinish -= OnBallReachedFinish;
        }

        if (slowBall != null)
        {
            slowBall.ReachedFinish -= OnBallReachedFinish;
        }
    }

    public void FreezeAll()
    {
        isRacing = false;
        finishedCount = 0;

        if (fastBall != null)
        {
            fastBall.StopAndFreeze();
        }

        if (slowBall != null)
        {
            slowBall.StopAndFreeze();
        }
    }

    public void StartRace()
    {
        if (isRacing == true)
            return;

        if (totalBalls <= 0)
            return;

        finishedCount = 0;
        isRacing = true;

        Vector3 direction = startDirection;
        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector3.forward;
        }

        if (fastBall != null)
        {
            fastBall.StartRun(direction);
        }

        if (slowBall != null)
        {
            slowBall.StartRun(direction);
        }

        Action startedEvent = RaceStarted;
        if (startedEvent != null)
        {
            startedEvent();
        }
    }

    private void OnBallReachedFinish(Wallrebound ball)
    {
        if (isRacing == false)
            return;

        finishedCount += 1;

        if (endFinishes == true)
        {
            EndRace();
            return;
        }

        if (finishedCount >= totalBalls)
        {
            EndRace();
        }
    }

    private void EndRace()
    {
        isRacing = false;

        if (fastBall != null)
        {
            fastBall.StopAndFreeze();
        }

        if (slowBall != null)
        {
            slowBall.StopAndFreeze();
        }

        Action finishedEvent = RaceFinished;
        if (finishedEvent != null)
        {
            finishedEvent();
        }
    }
}
