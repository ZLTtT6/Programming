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
        buildController.SetTrackPrefab(Prefab1);
    }

    public void Select2()
    {
        buildController.SetTrackPrefab(Prefab2);
    }

    public void Select3()
    {
        buildController.SetTrackPrefab(Prefab3);
    }
}
