using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicToggle : MonoBehaviour
{
    private const string PREFS_KEY = "MusicOn";
    private Button button;
    private TextMeshProUGUI label;

    void Awake()
    {
        button = GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        bool isOn = PlayerPrefs.GetInt(PREFS_KEY, 1) == 1;
        UpdateLabel(isOn);
        button.onClick.AddListener(Toggle);
    }

    void Start()
    {
        bool isOn = PlayerPrefs.GetInt(PREFS_KEY, 1) == 1;
        ApplyMusicState(isOn);
    }

    void OnEnable()
    {
        bool isOn = PlayerPrefs.GetInt(PREFS_KEY, 1) == 1;
        UpdateLabel(isOn);
    }

    void Toggle()
    {
        bool isOn = PlayerPrefs.GetInt(PREFS_KEY, 1) != 1;
        PlayerPrefs.SetInt(PREFS_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateLabel(isOn);
        ApplyMusicState(isOn);
    }

    void UpdateLabel(bool isOn)
    {
        if (label != null) label.text = isOn ? "Music On" : "Music Off";
    }

    void ApplyMusicState(bool isOn)
    {
        var musicSource = FindFirstObjectByType<AudioSource>();
        if (musicSource != null && musicSource.gameObject.name == "BackgroundMusic")
            musicSource.mute = !isOn;
    }
}
