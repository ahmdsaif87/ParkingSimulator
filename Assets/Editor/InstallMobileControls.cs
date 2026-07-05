using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class InstallMobileControls
{
    const string SteeringWheelPath = "Assets/EasyVehicleSteering/Prefabs/SteeringWheel.prefab";

    [MenuItem("Tools/Install Mobile Controls in Current Scene")]
    public static void Install()
    {
        Canvas canvas = Object.FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene.");
            return;
        }

        if (GameObject.Find("MobileControls") != null)
        {
            Debug.Log("Mobile controls already installed.");
            return;
        }

        GameObject mobileControls = new GameObject("MobileControls");
        mobileControls.transform.SetParent(canvas.transform, false);

        // ── Steering Wheel from prefab ──
        GameObject swPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SteeringWheelPath);
        if (swPrefab == null)
        {
            Debug.LogError("SteeringWheel prefab not found at: " + SteeringWheelPath);
            return;
        }
        GameObject swInstance = (GameObject)PrefabUtility.InstantiatePrefab(swPrefab);
        swInstance.name = "SteeringWheel";
        swInstance.transform.SetParent(mobileControls.transform, false);
        RectTransform swRect = swInstance.GetComponent<RectTransform>();
        swRect.anchorMin = Vector2.zero;
        swRect.anchorMax = Vector2.zero;
        swRect.anchoredPosition = new Vector2(220, 180);
        swRect.sizeDelta = new Vector2(300, 300);

        // ── Pedals (bottom-right, side-by-side like real car) ──
        GameObject gpObj = new GameObject("GasPedal");
        gpObj.transform.SetParent(mobileControls.transform, false);
        RectTransform gpRect = gpObj.AddComponent<RectTransform>();
        gpRect.anchorMin = new Vector2(1, 0);
        gpRect.anchorMax = new Vector2(1, 0);
        gpRect.anchoredPosition = new Vector2(-120, 140);
        gpRect.sizeDelta = new Vector2(200, 200);
        gpObj.AddComponent<EasyVehicleSteering.GasPedal>();

        // ── Ensure EventSystem exists ──
        if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("Created EventSystem.");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Mobile controls installed! Save scene to keep changes.");
    }

    [MenuItem("Tools/Remove Mobile Controls from Current Scene")]
    public static void Remove()
    {
        GameObject mc = GameObject.Find("MobileControls");
        if (mc != null)
        {
            Object.DestroyImmediate(mc);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("Mobile controls removed.");
        }
        else
        {
            Debug.Log("No mobile controls found.");
        }
    }
}
