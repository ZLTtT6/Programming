using UnityEngine;

public class BuildController : MonoBehaviour
{
    public GridManager gridManager;

    [Header("Selected Prefab")]
    public GameObject selectedTrackPrefab;

    [Header("Rotate")]
    public int rotateStep = 90;

    private int rotateIndex = 0;

    private GameObject previewObject;
    private Vector2Int hoverGridPos;
    private bool hasHoverPos = false;

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
            TogglePlaceOrRemove();
        }
    }

    public void SetSelectedTrackPrefab(GameObject prefab)
    {
        selectedTrackPrefab = prefab;
        rotateIndex = 0;

        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    void RotateLeft()
    {
        rotateIndex -= 1;
        if (rotateIndex < 0)
        {
            rotateIndex = 3;
        }
    }

    void RotateRight()
    {
        rotateIndex += 1;
        if (rotateIndex > 3)
        {
            rotateIndex = 0;
        }
    }

    void UpdatePreview()
    {
        hasHoverPos = false;

        if (gridManager == null)
        {
            HidePreview();
            return;
        }

        if (selectedTrackPrefab == null)
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

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit == false)
        {
            HidePreview();
            return;
        }

        Vector2Int gridPos = gridManager.WorldToGrid(hitInfo.point);

        bool validPos = gridManager.IsValidGridPosition(gridPos.x, gridPos.y);
        if (validPos == false)
        {
            HidePreview();
            return;
        }

        hoverGridPos = gridPos;
        hasHoverPos = true;

        EnsurePreviewObject();

        Vector3 spawnPos = gridManager.GridToWorld(gridPos.x, gridPos.y);
        Quaternion prefabRotation = selectedTrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateIndex * rotateStep, 0f);
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

        previewObject = Instantiate(selectedTrackPrefab);
        previewObject.name = "Preview_" + selectedTrackPrefab.name;

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

    void TogglePlaceOrRemove()
    {
        if (hasHoverPos == false)
            return;

        if (gridManager == null)
            return;

        if (selectedTrackPrefab == null)
            return;

        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        TrackModule existingTrack = trackManager.GetTrack(hoverGridPos);
        if (existingTrack != null)
        {
            trackManager.RemoveTrack(hoverGridPos);
            Destroy(existingTrack.gameObject);
            return;
        }

        bool occupied = trackManager.IsOccupied(hoverGridPos);
        if (occupied == true)
            return;

        // 3) ·ñÔò·ÅÖÃ
        Vector3 spawnPos = gridManager.GridToWorld(hoverGridPos.x, hoverGridPos.y);

        Quaternion prefabRotation = selectedTrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateIndex * rotateStep, 0f);
        Quaternion spawnRotation = rotateOffset * prefabRotation;

        GameObject newObject = Instantiate(selectedTrackPrefab, spawnPos, spawnRotation);

        TrackModule trackModule = newObject.GetComponent<TrackModule>();
        if (trackModule == null)
            return;

        trackModule.gridPos = hoverGridPos;
        trackModule.rotationIndex = rotateIndex;

        trackManager.AddTrack(hoverGridPos, trackModule);
        trackModule.UpdateWalls();
    }
}
