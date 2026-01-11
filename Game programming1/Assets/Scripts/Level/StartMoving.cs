using UnityEngine;
using System;

public class StartMoving : MonoBehaviour
{
    [Header("Ball GameObjects")]
    public Wallrebound fastBall;
    public Wallrebound slowBall;

    [Header("Start Direction")]
    public Vector3 startDirection = - Vector3.forward;

    private bool hasStarted = false;

    void Start()
    {
        if (fastBall != null) fastBall.StopAndFreeze();
        if (slowBall != null) slowBall.StopAndFreeze();
    }

    public void StartRace()
    {
        if (hasStarted) 
            return;
        hasStarted = true;

        Vector3 dir = startDirection;
        if (dir.sqrMagnitude < 0.001f) dir = Vector3.forward;
        dir.Normalize();

        if (fastBall != null) fastBall.StartRun(dir);
        if (slowBall != null) slowBall.StartRun(dir);
    }
}
