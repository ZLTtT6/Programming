using UnityEngine;

public class BuildController : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject trackPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceModule();
        }
    }

    void TryPlaceModule()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        Vector2Int gridPos = gridManager.GetGridPosition(hit.point);

        if (!gridManager.IsValidGridPosition(gridPos.x, gridPos.y))
            return;

        if (TrackManager.Instance.HasTrack(gridPos))
            return;

        Vector3 spawnPos = gridManager.GetWorldPosition(gridPos.x, gridPos.y);

        GameObject go = Instantiate(trackPrefab, spawnPos, Quaternion.identity);

        TrackModule module = go.GetComponent<TrackModule>();
        module.gridPos = gridPos;
    }
}
