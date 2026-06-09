using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI messageText;

    [Header("Start Text")]
    public TextMeshProUGUI startText;

    [Header("Success Panel")]
    public GameObject successPanel;
    public TextMeshProUGUI successTitleText;
    public TextMeshProUGUI successSubtitleText;
    public Button nextLevelButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (startText != null) startText.gameObject.SetActive(false);
        if (successPanel != null) successPanel.SetActive(false);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    public void ShowMessage(string text, float duration)
    {
        if (messageText == null) return;
        messageText.gameObject.SetActive(true);
        messageText.text = text;
        StopAllCoroutines();
        if (duration > 0f)
            StartCoroutine(HideMessageAfter(duration));
    }

    IEnumerator HideMessageAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    public void ShowStartText()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            if (messageText != null) messageText.gameObject.SetActive(false);
        }
    }

    public void HideStartText()
    {
        if (startText != null) startText.gameObject.SetActive(false);
    }

    public void ShowSuccessPanel(string title, string subtitle)
    {
        HideStartText();
        if (messageText != null) messageText.gameObject.SetActive(false);
        if (successTitleText != null) successTitleText.text = title;
        if (successSubtitleText != null) successSubtitleText.text = subtitle;
        if (successPanel != null) successPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Assets/Scenes/Level/Level 2.unity");
    }
}
