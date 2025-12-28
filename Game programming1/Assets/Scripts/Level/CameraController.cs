using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotateSpeed = 80f;

    [Header("Rotation Pivot")]
    public float groundHeight = 0f;

    [Header("Grid Reference")]
    public GridManager gridManager;

    [Header("Camera Move Area")]
    public bool enableCameraLimit = true;
    public float cameraMoveAreaSize = 50f;

    public Vector2 cameraMinXZ;
    public Vector2 cameraMaxXZ;

    void Start()
    {
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        UpdateBounds();
        ClampPosition();
    }

    void Update()
    {
        HandleMove();
        HandleRotate();

        if (enableCameraLimit)
        {
            ClampPosition();
        }
    }

    void HandleMove()
    {
        float inputX = 0f;
        float inputZ = 0f;

        if (Input.GetKey(KeyCode.W)) inputZ += 1f;
        if (Input.GetKey(KeyCode.S)) inputZ -= 1f;
        if (Input.GetKey(KeyCode.A)) inputX -= 1f;
        if (Input.GetKey(KeyCode.D)) inputX += 1f;

        if (Mathf.Abs(inputX) < 0.001f && Mathf.Abs(inputZ) < 0.001f)
            return;

        Vector3 moveDirection = transform.forward * inputZ + transform.right * inputX;
        moveDirection.y = 0f;

        if (moveDirection.sqrMagnitude < 0.0001f)
            return;

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    void HandleRotate()
    {
        float rotateInput = 0f;

        if (Input.GetKey(KeyCode.Q)) rotateInput -= 1f;
        if (Input.GetKey(KeyCode.E)) rotateInput += 1f;

        if (Mathf.Abs(rotateInput) < 0.001f)
            return;

        Vector3 pivotPoint = GetGroundPivotPoint();
        transform.RotateAround(pivotPoint, Vector3.up, rotateInput * rotateSpeed * Time.deltaTime);
    }

    Vector3 GetGroundPivotPoint()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
            if (mainCamera == null)
                return transform.position;
        }

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Mathf.Abs(ray.direction.y) < 0.0001f)
        {
            Vector3 fallbackPoint = transform.position + transform.forward * 10f;
            fallbackPoint.y = groundHeight;
            return fallbackPoint;
        }

        float t = (groundHeight - ray.origin.y) / ray.direction.y;
        if (t < 0f)
            t = 10f;

        Vector3 groundPoint = ray.origin + ray.direction * t;
        groundPoint.y = groundHeight;
        return groundPoint;
    }

    public void UpdateBounds()
    {
        if (gridManager == null)
            return;

        float gridWidthWorld = gridManager.gridWidth * gridManager.cellSize;
        float gridHeightWorld = gridManager.gridHeight * gridManager.cellSize;

        Vector3 gridCenter = gridManager.originPosition + new Vector3(gridWidthWorld * 0.5f, 0f, gridHeightWorld * 0.5f);

        float areaSize = cameraMoveAreaSize;
        if (enableCameraLimit == false)
            areaSize = Mathf.Min(gridWidthWorld, gridHeightWorld);

        float halfSize = areaSize * 0.5f;

        cameraMinXZ = new Vector2(gridCenter.x - halfSize, gridCenter.z - halfSize);
        cameraMaxXZ = new Vector2(gridCenter.x + halfSize, gridCenter.z + halfSize);
    }

    void ClampPosition()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, cameraMinXZ.x, cameraMaxXZ.x);
        currentPosition.z = Mathf.Clamp(currentPosition.z, cameraMinXZ.y, cameraMaxXZ.y);

        transform.position = currentPosition;
    }

    public void SetCameraMoveAreaSize(float size)
    {
        cameraMoveAreaSize = size;
        UpdateBounds();
        ClampPosition();
    }

    public void EnableCameraLimit(bool enable)
    {
        enableCameraLimit = enable;
        UpdateBounds();
        ClampPosition();
    }
}
