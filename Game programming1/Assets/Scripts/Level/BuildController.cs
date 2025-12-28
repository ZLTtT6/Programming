using UnityEngine;

public class BuildController : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject trackPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTrackModule();
        }
    }

    void PlaceTrackModule()
    {
        if (gridManager == null)
            return;

        if (trackPrefab == null)
            return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit == false)
            return;

        Vector2Int gridPos = gridManager.WorldToGrid(hitInfo.point);

        bool validPos = gridManager.IsValidGridPosition(gridPos.x, gridPos.y);
        if (validPos == false)
            return;

        TrackManager trackManager = TrackManager.Instance;
        if (trackManager == null)
            return;

        bool occupied = trackManager.IsOccupied(gridPos);
        if (occupied == true)
            return;

        Vector3 spawnPos = gridManager.GridToWorld(gridPos.x, gridPos.y);
        GameObject newObject = Instantiate(trackPrefab, spawnPos, Quaternion.identity);

        TrackModule trackModule = newObject.GetComponent<TrackModule>();
        if (trackModule == null)
            return;

        trackModule.gridPos = gridPos;
    }
}
