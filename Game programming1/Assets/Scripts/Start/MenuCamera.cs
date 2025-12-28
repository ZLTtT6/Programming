using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 2f;
    public float rotateSpeed = 20f;

    private float currentAngle = 0f;

    void Update()
    {
        if (target == null)
            return;

        currentAngle += rotateSpeed * Time.deltaTime;

        float angleRad = currentAngle * Mathf.Deg2Rad;

        float offsetX = Mathf.Sin(angleRad) * distance;
        float offsetZ = Mathf.Cos(angleRad) * distance;

        Vector3 offset = new Vector3(offsetX, height, offsetZ);

        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}
