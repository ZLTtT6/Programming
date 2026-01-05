using System;
using UnityEngine;

public class Wallrebound : MonoBehaviour
{
    [Header("Tags")]
    public string wallTag = "Wall";
    public string finishTag = "Finish";

    [Header("Speed (constant)")]
    public float speed = 8f;
    public float minReflectDot = 0f;
    public float nudgeOut = 0.02f;

    [Header("Optional")]
    public bool ignoreSpin = true;
    public bool keepYZero = true;

    private Rigidbody rigidbodyComponent;
    private bool isRunning = false;
    private bool hasFinished = false;

    private Vector3 moveDir = Vector3.forward;

    public event Action<Wallrebound> ReachedFinish;

    void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        if (rigidbodyComponent != null)
            rigidbodyComponent.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        StopAndFreeze();
    }

    void FixedUpdate()
    {
        if (!isRunning || rigidbodyComponent == null) return;

        Vector3 dir = moveDir;

        if (keepYZero) dir = Vector3.ProjectOnPlane(dir, Vector3.up);

        if (dir.sqrMagnitude < 0.0001f)
            dir = Vector3.forward;

        dir.Normalize();

        rigidbodyComponent.linearVelocity = dir * speed;

        if (ignoreSpin)
            rigidbodyComponent.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision) => HandleWallCollision(collision);
    void OnCollisionStay(Collision collision) => HandleWallCollision(collision);

    void HandleWallCollision(Collision collision)
    {
        if (!isRunning || rigidbodyComponent == null) return;
        if (collision.collider == null || !collision.collider.CompareTag(wallTag)) return;
        if (collision.contactCount <= 0) return;

        Vector3 normal = collision.GetContact(0).normal;

        if (keepYZero) normal = Vector3.ProjectOnPlane(normal, Vector3.up);
        if (normal.sqrMagnitude < 0.0001f) return;
        normal.Normalize();

        Vector3 inDir = moveDir;
        if (keepYZero) inDir = Vector3.ProjectOnPlane(inDir, Vector3.up);
        if (inDir.sqrMagnitude < 0.0001f) return;
        inDir.Normalize();

        float dotValue = Vector3.Dot(-inDir, normal);

        if (dotValue < minReflectDot)
        {
        }

        Vector3 outDir = Vector3.Reflect(inDir, normal).normalized;

        moveDir = outDir;

        rigidbodyComponent.linearVelocity = moveDir * speed;
        rigidbodyComponent.position += normal * nudgeOut;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isRunning || hasFinished || other == null) return;
        if (!other.CompareTag(finishTag)) return;

        hasFinished = true;
        ReachedFinish?.Invoke(this);
    }

    public void StartRun(Vector3 initialDirection)
    {
        if (rigidbodyComponent == null) return;

        hasFinished = false;

        if (keepYZero) initialDirection = Vector3.ProjectOnPlane(initialDirection, Vector3.up);
        if (initialDirection.sqrMagnitude < 0.001f) initialDirection = Vector3.forward;

        moveDir = initialDirection.normalized; // ✅ 记住方向

        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.linearVelocity = moveDir * speed;

        if (ignoreSpin)
            rigidbodyComponent.angularVelocity = Vector3.zero;

        isRunning = true;
    }

    public void StopAndFreeze()
    {
        if (rigidbodyComponent == null) return;

        isRunning = false;
        hasFinished = false;

        rigidbodyComponent.linearVelocity = Vector3.zero;
        rigidbodyComponent.angularVelocity = Vector3.zero;
        rigidbodyComponent.isKinematic = true;

        moveDir = Vector3.forward;
    }
}
