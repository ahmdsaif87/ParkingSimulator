using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class CopyLevel1ToLevel2
{
    [MenuItem("Tools/Copy _RouteArrows and ParkingFloor from Level 1 to Level 2")]
    public static void CopyObjects()
    {
        string scene1Path = "Assets/Scenes/Level/Level 1.unity";
        string scene2Path = "Assets/Scenes/Level/Level 2.unity";
        string tempPrefabDir = "Assets/Editor/TempCopy/";

        if (!Directory.Exists(tempPrefabDir))
            Directory.CreateDirectory(tempPrefabDir);

        // Open Level 1
        EditorSceneManager.OpenScene(scene1Path);
        var routeArrows = GameObject.Find("_RouteArrows");
        var parkingZone = GameObject.Find("ParkingZone");
        GameObject parkingFloor = null;
        if (parkingZone != null)
        {
            foreach (Transform child in parkingZone.transform)
            {
                if (child.name == "ParkingFloor")
                {
                    parkingFloor = child.gameObject;
                    break;
                }
            }
        }

        if (routeArrows == null)
        {
            Debug.LogError("_RouteArrows not found in Level 1");
            return;
        }
        if (parkingFloor == null)
        {
            Debug.LogError("ParkingFloor not found in Level 1");
            return;
        }

        // Save as prefab assets
        string arrowsPrefabPath = tempPrefabDir + "_RouteArrows.prefab";
        string floorPrefabPath = tempPrefabDir + "ParkingFloor.prefab";

        PrefabUtility.SaveAsPrefabAsset(routeArrows, arrowsPrefabPath);
        PrefabUtility.SaveAsPrefabAsset(parkingFloor, floorPrefabPath);

        // Open Level 2
        EditorSceneManager.OpenScene(scene2Path);

        // Instantiate arrows
        var arrowsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(arrowsPrefabPath);
        if (arrowsPrefab != null)
        {
            var arrowsInstance = (GameObject)PrefabUtility.InstantiatePrefab(arrowsPrefab);
            arrowsInstance.name = "_RouteArrows";
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        // Add ParkingFloor to existing ParkingZone
        var floorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(floorPrefabPath);
        var parkingZoneL2 = GameObject.Find("ParkingZone");
        if (floorPrefab != null && parkingZoneL2 != null)
        {
            var floorInstance = (GameObject)PrefabUtility.InstantiatePrefab(floorPrefab);
            floorInstance.name = "ParkingFloor";
            floorInstance.transform.SetParent(parkingZoneL2.transform);
            floorInstance.transform.localPosition = new Vector3(-0.127f, -0.43f, 0.031f);
            floorInstance.transform.localRotation = Quaternion.Euler(90, 0, 0);
            floorInstance.transform.localScale = new Vector3(1.4728246f, 1f, 5f);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        // Save scene
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        // Cleanup temp prefabs
        AssetDatabase.DeleteAsset(tempPrefabDir);

        Debug.Log("Successfully copied _RouteArrows and ParkingFloor to Level 2");
    }
}
