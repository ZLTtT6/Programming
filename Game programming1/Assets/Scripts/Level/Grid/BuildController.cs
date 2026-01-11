using System.Collections.Generic;
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

    [Header("Prefab3 (Attachment)")]
    public GameObject prefab3;

    private Dictionary<Vector2Int, GameObject> prefab3Map = new Dictionary<Vector2Int, GameObject>();

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

    bool IsSelectingPrefab3()
    {
        return prefab3 != null && TrackPrefab == prefab3;
    }

    bool HasPrefab3(Vector2Int pos)
    {
        return prefab3Map.ContainsKey(pos) && prefab3Map[pos] != null;
    }

    void RemovePrefab3(Vector2Int pos)
    {
        if (prefab3Map.TryGetValue(pos, out GameObject go) && go != null)
        {
            Destroy(go);
        }
        prefab3Map.Remove(pos);
    }

    bool CanPlacePrefab3(Vector2Int pos)
    {
        TrackManager tm = TrackManager.Instance;
        if (tm == null) return false;

        TrackModule baseTrack = tm.GetTrack(pos);
        if (baseTrack == null) return false;

        return baseTrack.trackType == 1 || baseTrack.trackType == 2;
    }

    bool IsAdjacentToOccupied(Vector2Int pos)
    {
        TrackManager tm = TrackManager.Instance;
        if (tm == null) return false;

        return tm.IsOccupied(pos + Vector2Int.up)
            || tm.IsOccupied(pos + Vector2Int.down)
            || tm.IsOccupied(pos + Vector2Int.left)
            || tm.IsOccupied(pos + Vector2Int.right);
    }

    bool CanPlaceTrack(Vector2Int pos)
    {
        TrackManager tm = TrackManager.Instance;
        if (tm == null) return false;
        if (tm.IsOccupied(pos)) return false;
        return IsAdjacentToOccupied(pos);
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

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit == false)
        {
            HidePreview();
            return;
        }

        Vector2Int gridPos = gridManager.WorldToGrid(hitInfo.point);

        bool validPos = gridManager.GridPosition(gridPos.x, gridPos.y);
        if (validPos == false)
        {
            HidePreview();
            return;
        }

        previewObjectPos = gridPos;

        EnsurePreviewObject();

        Vector3 spawnPos = gridManager.GridToWorld(gridPos.x, gridPos.y);

        Quaternion prefabRotation = TrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateTime * rotateAngle, 0f);
        Quaternion spawnRotation = rotateOffset * prefabRotation;

        previewObject.transform.position = spawnPos;
        previewObject.transform.rotation = spawnRotation;

        if (previewObject.activeSelf == false)
            previewObject.SetActive(true);

        TrackManager tm = TrackManager.Instance;
        if (tm == null)
        {
            emptyNot = false;
            return;
        }

        if (IsSelectingPrefab3())
        {
            emptyNot = HasPrefab3(previewObjectPos) || CanPlacePrefab3(previewObjectPos);
            return;
        }

        TrackModule existingTrack = tm.GetTrack(previewObjectPos);
        if (existingTrack != null)
        {
            emptyNot = true;
            return;
        }

        emptyNot = CanPlaceTrack(previewObjectPos);
    }
    public void SetBuildActive(bool active)
    {
        enabled = active;
        if (!active) HidePreview();
        if (!active) emptyNot = false;
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

    public void HidePreview()
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

        if (IsSelectingPrefab3())
        {
            if (HasPrefab3(previewObjectPos))
            {
                RemovePrefab3(previewObjectPos);
                return;
            }

            if (!CanPlacePrefab3(previewObjectPos))
                return;

            Vector3 spawnPos3 = gridManager.GridToWorld(previewObjectPos.x, previewObjectPos.y);

            Quaternion prefabRotation3 = TrackPrefab.transform.rotation;
            Quaternion rotateOffset3 = Quaternion.Euler(0f, rotateTime * rotateAngle, 0f);
            Quaternion spawnRotation3 = rotateOffset3 * prefabRotation3;

            GameObject new3 = Instantiate(TrackPrefab, spawnPos3, spawnRotation3);
            prefab3Map[previewObjectPos] = new3;
            return;
        }

        TrackModule existingTrack = trackManager.GetTrack(previewObjectPos);
        if (existingTrack != null)
        {
            if (HasPrefab3(previewObjectPos))
                RemovePrefab3(previewObjectPos);

            trackManager.RemoveTrack(previewObjectPos);
            Destroy(existingTrack.gameObject);
            return;
        }

        bool occupied = trackManager.IsOccupied(previewObjectPos);
        if (occupied == true)
            return;

        if (!CanPlaceTrack(previewObjectPos))
            return;

        Vector3 spawnPos = gridManager.GridToWorld(previewObjectPos.x, previewObjectPos.y);

        Quaternion prefabRotation = TrackPrefab.transform.rotation;
        Quaternion rotateOffset = Quaternion.Euler(0f, rotateTime * rotateAngle, 0f);
        Quaternion spawnRotation = rotateOffset * prefabRotation;

        GameObject newObject = Instantiate(TrackPrefab, spawnPos, spawnRotation);

        TrackModule trackModule = newObject.GetComponent<TrackModule>();
        if (trackModule == null)
            return;

        trackModule.gridPos = previewObjectPos;
        trackModule.rotationIndex = rotateTime;
        trackManager.AddTrack(previewObjectPos, trackModule);
        trackModule.UpdateWalls();
    }
}
