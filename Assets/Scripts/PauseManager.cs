using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    GameObject pausePanel;
    Button continueButton;
    Button mainMenuButton;

    bool isPaused;

    void Start()
    {
        FindReferences();
        if (pausePanel != null) pausePanel.SetActive(false);
        if (continueButton != null) continueButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void FindReferences()
    {
        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in all)
        {
            if (!go.scene.IsValid()) continue;
            if (go.name == "PausePanel") pausePanel = go;
            else if (go.name == "ContinueBtn") continueButton = go.GetComponent<Button>();
            else if (go.name == "MainMenuBtn") mainMenuButton = go.GetComponent<Button>();
            else if (go.name == "PauseBtn")
            {
                var btn = go.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(PauseGame);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        FindReferences();
        MuteSFX(true);
        if (pausePanel != null) pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        MuteSFX(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void MuteSFX(bool mute)
    {
        bool sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        var all = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (var src in all)
        {
            if (!src.gameObject.scene.IsValid()) continue;
            if (src.gameObject.name == "BackgroundMusic") continue;
            src.mute = mute ? true : !sfxOn;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
