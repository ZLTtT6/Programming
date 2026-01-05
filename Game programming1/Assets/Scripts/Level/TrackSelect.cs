using UnityEngine;

public class TrackSelect : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;

    [Header("Build Controller")]
    public BuildController buildController;

    void Start()
    {
        if (buildController == null)
        {
            buildController = FindFirstObjectByType<BuildController>();
        }

        Select1();
    }

    public void Select1()
    {
        if (buildController == null)
            return;

        buildController.SetSelectedTrackPrefab(Prefab1);
    }

    public void Select2()
    {
        if (buildController == null)
            return;

        buildController.SetSelectedTrackPrefab(Prefab2);
    }

    public void Select3()
    {
        if (buildController == null)
            return;

        buildController.SetSelectedTrackPrefab(Prefab3);
    }
}
