using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip backgroundMusic; // ±≥æ∞“Ù¿÷ BackgroungMusic
    [SerializeField] private AudioClip gameplayMusic;   // ”Œœ∑“Ù¿÷ GameMusic
    [SerializeField] private AudioClip winMusic;        //  §¿˚“Ù¿÷ WinMusic
    [SerializeField] private AudioClip loseMusic;       //  ß∞‹“Ù¿÷ LoseMusic

    [Header("SFX Clips")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip placeTrackSFX;
    [SerializeField] private AudioClip deleteTrackSFX;

    [Header("Master Volume (Slider)")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private string masterSliderObjectName = "BackgroundSlider";
    [Range(0f, 1f)][SerializeField] private float defaultMasterVolume = 1f;

    private const string PREF_MASTER_VOL = "MasterVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        float v = PlayerPrefs.GetFloat(PREF_MASTER_VOL, defaultMasterVolume);
        ApplyMasterVolume(v, save: false);

        TryFindAndBindSlider();

        PlayBackgroundMusic(restart: true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic(restart: true);

        TryFindAndBindSlider();
    }

    private void TryFindAndBindSlider()
    {
        if (masterVolumeSlider == null)
        {
            var go = GameObject.Find(masterSliderObjectName);
            if (go != null)
                masterVolumeSlider = go.GetComponent<Slider>();
        }

        BindMasterSlider(masterVolumeSlider);
    }

    public void BindMasterSlider(Slider slider)
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterSliderChanged);

        masterVolumeSlider = slider;

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(AudioListener.volume);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        }
    }

    private void OnMasterSliderChanged(float value)
    {
        SetMasterVolume(value);
    }

    public void SetMasterVolume(float volume)
    {
        ApplyMasterVolume(volume, save: true);
    }

    private void ApplyMasterVolume(float volume, bool save)
    {
        volume = Mathf.Clamp01(volume);
        AudioListener.volume = volume;

        if (save)
        {
            PlayerPrefs.SetFloat(PREF_MASTER_VOL, volume);
            PlayerPrefs.Save();
        }

        if (masterVolumeSlider != null)
            masterVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void PlayBackgroundMusic(bool restart = false)
    {
        PlayMusic(backgroundMusic, loop: true, restart: restart);
    }

    public void PlayGameplayMusic(bool restart = true)
    {
        StopMusic();
        PlayMusic(gameplayMusic, loop: true, restart: restart);
    }

    public void PlayWinMusic()
    {
        StopMusic();
        PlayMusic(winMusic, loop: false, restart: true);
    }

    public void PlayLoseMusic()
    {
        StopMusic();
        PlayMusic(loseMusic, loop: false, restart: true);
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    private void PlayMusic(AudioClip clip, bool loop, bool restart)
    {
        if (musicSource == null || clip == null) return;

        if (!restart && musicSource.isPlaying && musicSource.clip == clip)
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayButtonClickSFX() => PlaySFX(buttonClickSFX);
    public void PlayPlaceTrackSFX() => PlaySFX(placeTrackSFX);
    public void PlayDeleteTrackSFX() => PlaySFX(deleteTrackSFX);

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
