using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject helpPanel;

    [Header("Scene Names")]
    [SerializeField] private string level1SceneName = "Level1";
    [SerializeField] private string level2SceneName = "Level2";
    [SerializeField] private string level3SceneName = "Level3";

    void Start()
    {
        OpenMainPanel();
    }

    void CloseAllPanels()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (levelPanel != null) levelPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    void OpenMainPanel()
    {
        CloseAllPanels();
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    public void OnClickStart()
    {
        CloseAllPanels();
        if (levelPanel != null) levelPanel.SetActive(true);
    }

    public void OnClickSettings()
    {
        CloseAllPanels();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OnClickHelp()
    {
        CloseAllPanels();
        if (helpPanel != null) helpPanel.SetActive(true);
    }

    public void OnClickBackToMain()
    {
        OpenMainPanel();
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickLevel1()
    {
        LoadLevel(level1SceneName);
    }

    public void OnClickLevel2()
    {
        LoadLevel(level2SceneName);
    }

    public void OnClickLevel3()
    {
        LoadLevel(level3SceneName);
    }

    void LoadLevel(string sceneName)
    {
        if (sceneName == null || sceneName.Trim().Length == 0)
            return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
