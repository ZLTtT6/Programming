using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject helpPanel;

    [Header("Scene Names")]
    [SerializeField] private string level1SceneName = "Level1";
    [SerializeField] private string level2SceneName = "Level2";
    [SerializeField] private string level3SceneName = "Level3";
    [SerializeField] private string startSceneName = "Start";
    [SerializeField] private string FreeSceneName = "Freedom";

    [Header("Levels")]
    public bool isLevel2 = false;
    public GameObject level2Buttpm;
    public GameObject noLevel2Buttpm;

    public bool isLevel3 = false;
    public GameObject level3Button;
    public GameObject noLevel3Buttpm;

    private const string KEY_UNLOCK_L2 = "UnlockLevel2";
    private const string KEY_UNLOCK_L3 = "UnlockLevel3";

    void Start()
    {
        LoadUnlockState();
        OpenMainPanel();
        RefreshLevelPanels();
    }

    private void LoadUnlockState()
    {
        isLevel2 = PlayerPrefs.GetInt(KEY_UNLOCK_L2, 0) == 1;
        isLevel3 = PlayerPrefs.GetInt(KEY_UNLOCK_L3, 0) == 1;

        if (isLevel3) isLevel2 = true;
    }

    private void SaveUnlockState()
    {
        PlayerPrefs.SetInt(KEY_UNLOCK_L2, isLevel2 ? 1 : 0);
        PlayerPrefs.SetInt(KEY_UNLOCK_L3, isLevel3 ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void RefreshLevelPanels()
    {
        if (level2Buttpm != null) level2Buttpm.SetActive(isLevel2);
        if (noLevel2Buttpm != null) noLevel2Buttpm.SetActive(!isLevel2);

        if (level3Button != null) level3Button.SetActive(isLevel3);
        if (noLevel3Buttpm != null) noLevel3Buttpm.SetActive(!isLevel3);
    }

    public void UnlockLevel2()
    {
        isLevel2 = true;
        SaveUnlockState();
        RefreshLevelPanels();
    }

    public void UnlockLevel3()
    {
        isLevel2 = true;
        isLevel3 = true;
        SaveUnlockState();
        RefreshLevelPanels();
    }

    void CloseAllPanels()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (levelPanel != null) levelPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    void OpenMainPanel()
    {
        CloseAllPanels();
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    public void OnClickLevelSelect()
    {
        CloseAllPanels();
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);

        LoadUnlockState();
        RefreshLevelPanels();
    }

    public void OnClickLevel()
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
        if (!isLevel2) return;
        LoadLevel(level2SceneName);
    }

    public void OnClickLevel3()
    {
        if (!isLevel3) return;
        LoadLevel(level3SceneName);
    }

    public void OnClickStartLevel()
    {
        LoadLevel(startSceneName);
    }

    public void OnClickFreeLevel()
    {
        LoadLevel(FreeSceneName);
    }

    void LoadLevel(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
            return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
