using System;
using UnityEngine;

public class Wallrebound : MonoBehaviour
{
    [Header("Tags")]
    public string wallTag = "Wall";

    [Header("Speed")]
    public float speed = 8f;
    public float pullOut = 0.02f;

    [Header("Optional")]
    public bool ignoreSpin = true;
    public bool keepYZero = true;

    Rigidbody rb;
    bool isRunning;
    Vector3 moveDir = Vector3.forward;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        // Continuous dynamic collision detection
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        StopAndFreeze();
    }

    void FixedUpdate()
    {
        if (!isRunning || rb == null) return;

        Vector3 dir = moveDir;
        if (keepYZero) dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        if (dir.sqrMagnitude < 0.0001f) dir = Vector3.forward;

        dir.Normalize();
        rb.linearVelocity = dir * speed;

        if (ignoreSpin) rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision) => HandleWallCollision(collision);
    void OnCollisionStay(Collision collision) => HandleWallCollision(collision);

    void HandleWallCollision(Collision collision)
    {
        if (!isRunning || rb == null) return;

        var col = collision.collider;
        if (col == null || !col.CompareTag(wallTag)) return;
        if (collision.contactCount <= 0) return;

        Vector3 normal = collision.GetContact(0).normal;
        if (keepYZero) normal = Vector3.ProjectOnPlane(normal, Vector3.up);
        if (normal.sqrMagnitude < 0.0001f) return;
        normal.Normalize();

        Vector3 inDir = moveDir;
        if (keepYZero) inDir = Vector3.ProjectOnPlane(inDir, Vector3.up);
        if (inDir.sqrMagnitude < 0.0001f) return;
        inDir.Normalize();

        moveDir = Vector3.Reflect(inDir, normal).normalized;

        rb.linearVelocity = moveDir * speed;
        rb.position += normal * pullOut;
    }

    public void StartRun(Vector3 initialDirection)
    {
        if (rb == null) return;

        if (keepYZero) initialDirection = Vector3.ProjectOnPlane(initialDirection, Vector3.up);
        if (initialDirection.sqrMagnitude < 0.001f) initialDirection = Vector3.forward;

        moveDir = initialDirection.normalized;

        rb.isKinematic = false;
        rb.linearVelocity = moveDir * speed;

        if (ignoreSpin) rb.angularVelocity = Vector3.zero;

        isRunning = true;
    }

    public void StopAndFreeze()
    {
        if (rb == null) return;

        isRunning = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        moveDir = Vector3.forward;
    }
}
