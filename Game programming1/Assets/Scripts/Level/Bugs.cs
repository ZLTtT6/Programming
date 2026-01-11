using UnityEngine;

public class Bugs : MonoBehaviour
{
    public GameObject Losepanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        var ball = other.GetComponentInParent<Wallrebound>();
        if (ball == null) return;
        Losepanel.SetActive(true);
        Time.timeScale = 0f;
    }

}
