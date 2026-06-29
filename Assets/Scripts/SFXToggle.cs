using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SFXToggle : MonoBehaviour
{
    private const string PREFS_KEY = "SFXOn";
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
        ApplySFXState(isOn);
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
        ApplySFXState(isOn);
    }

    void UpdateLabel(bool isOn)
    {
        if (label != null) label.text = isOn ? "SFX On" : "SFX Off";
    }

    void ApplySFXState(bool isOn)
    {
        var allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var src in allSources)
        {
            if (src.gameObject.name == "BackgroundMusic") continue;
            src.mute = !isOn;
        }
    }
}
