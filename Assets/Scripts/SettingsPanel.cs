using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    void Awake()
    {
        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in all)
        {
            if (!go.scene.IsValid()) continue;
            if (go.name == "QuitSettingsBtn")
            {
                var btn = go.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => gameObject.SetActive(false));
                }
                break;
            }
        }
    }
}
