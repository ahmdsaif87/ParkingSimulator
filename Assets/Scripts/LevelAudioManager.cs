using UnityEngine;

public class LevelAudioManager : MonoBehaviour
{
    [SerializeField] float musicVolume = 0.15f;

    void Start()
    {
        // Create background music AudioSource
        var audioGO = new GameObject("BackgroundMusic");
        audioGO.transform.SetParent(transform);
        var src = audioGO.AddComponent<AudioSource>();
        src.clip = Resources.Load<AudioClip>("Audio/bg-music");
        src.loop = true;
        src.playOnAwake = true;
        src.volume = musicVolume;
        src.mute = PlayerPrefs.GetInt("MusicOn", 1) != 1;
        src.Play();

        // Apply SFX state to all AudioSources
        bool sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        var allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var s in allSources)
        {
            if (s.gameObject.name == "BackgroundMusic") continue;
            s.mute = !sfxOn;
        }
    }
}
