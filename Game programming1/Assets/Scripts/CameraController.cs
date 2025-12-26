using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 80f;

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A D
        float v = Input.GetAxis("Vertical");   // W S

        // 以相机朝向为基准，在地面上移动
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * v + right * h;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        float rotate = 0f;

        if (Input.GetKey(KeyCode.Q))
            rotate -= 1f;
        if (Input.GetKey(KeyCode.E))
            rotate += 1f;

        transform.Rotate(Vector3.up, rotate * rotateSpeed * Time.deltaTime, Space.World);
    }
}
