using UnityEngine;

public class BuildController : MonoBehaviour
{
    public GridManager gridManager;

    [Header("Selected Prefab")]
    public GameObject TrackPrefab;

    [Header("Rotate")]
    public int rotateAngle = 90;
    private int rotateTime = 0;

    private GameObject previewObject;
    private Vector2Int previewObjectPos;
    private bool emptyNot = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateLeft();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RotateRight();
        }

        UpdatePreview();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceOrDelect();
        }
    }

    // for UI to select
    public void SetTrackPrefab(GameObject prefab)
    {
        TrackPrefab = prefab;
        rotateTime = 0;

        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    void RotateLeft()
    {
        rotateTime -= 1;
        if (rotateTime < 0)
        {
            rotateTime = 3;
        }
    }
    void RotateRight()
    {
        rotateTime += 1;
        if (rotateTime > 3)
        {
            rotateTime = 0;
        }
    }

    void UpdatePreview()
    {
        emptyNot = false;

        if (gridManager == null)
        {
            HidePreview();
            return;
        }

        if (TrackPrefab == null)
        {
            HidePreview();
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            HidePreview();
            return;
        }

        // Determine mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit == false)
        {
            HidePreview();
            return;
        }

        // Get the world coordinates of the points hit by the ray to grid coordinates.
        Vector2Int gridPos = gridManager.WorldToGrid(hitInfo.point);

        bool validPos = gridManager.GridPosition(gridPos.x, gridPos.y);
        if (validPos == false)
        {
            HidePreview();
            return;
        }

        previewObjectPos = gridPos;
        emptyNot = true;

        EnsurePreviewObject();

        // Determine the position and rotation of the preview object
        Vector3 spawnPos = gridManager.GridToWorld(gridPos.x, gridPos.y);
        Quaternion prefabRotation = TrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateTime * rotateAngle, 0f);
        Quaternion spawnRotation = rotateOffset * prefabRotation;

        previewObject.transform.position = spawnPos;
        previewObject.transform.rotation = spawnRotation;

        if (previewObject.activeSelf == false)
        {
            previewObject.SetActive(true);
        }
    }

    void EnsurePreviewObject()
    {
        if (previewObject != null)
            return;

        previewObject = Instantiate(TrackPrefab);
        previewObject.name = "Preview_" + TrackPrefab.name;

        Collider[] colliders = previewObject.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        Rigidbody[] rigidbodies = previewObject.GetComponentsInChildren<Rigidbody>(true);
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].useGravity = false;
        }

        TrackModule trackModule = previewObject.GetComponent<TrackModule>();
        if (trackModule != null)
        {
            trackModule.enabled = false;
        }

        SetLayerRecursively(previewObject, 2);
        previewObject.SetActive(false);
    }

    void SetLayerRecursively(GameObject rootObject, int layer)
    {
        rootObject.layer = layer;

        for (int i = 0; i < rootObject.transform.childCount; i++)
        {
            Transform child = rootObject.transform.GetChild(i);
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    void HidePreview()
    {
        if (previewObject != null)
        {
            previewObject.SetActive(false);
        }
    }

    void PlaceOrDelect()
    {
        if (emptyNot == false)
            return;

        if (gridManager == null)
            return;

        if (TrackPrefab == null)
            return;

        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        // Check if the current cell already has a TrackModule.
        TrackModule existingTrack = trackManager.GetTrack(previewObjectPos);
        if (existingTrack != null)
        {
            trackManager.RemoveTrack(previewObjectPos);
            Destroy(existingTrack.gameObject);
            return;
        }

        bool occupied = trackManager.IsOccupied(previewObjectPos);
        if (occupied == true)
            return;

        // Calculate the world coordinates of the center of this grid.
        Vector3 spawnPos = gridManager.GridToWorld(previewObjectPos.x, previewObjectPos.y);

        // Get rotate prefab
        Quaternion prefabRotation = TrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateTime * rotateAngle, 0f);
        Quaternion spawnRotation = rotateOffset * prefabRotation;

        GameObject newObject = Instantiate(TrackPrefab, spawnPos, spawnRotation);

        TrackModule trackModule = newObject.GetComponent<TrackModule>();
        if (trackModule == null)
            return;

        // Record the grid position of the module
        trackModule.gridPos = previewObjectPos;
        trackModule.rotationIndex = rotateTime;
        trackManager.AddTrack(previewObjectPos, trackModule);
        trackModule.UpdateWalls();
    }
}
