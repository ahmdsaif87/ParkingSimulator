using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SetupLevel2Obstacles : EditorWindow
{
    static readonly string BarrierFenceName = "Props_Traffic Control Barrier Fence";
    static readonly string ConeName = "Props_Traffic cone";

    [MenuItem("Tools/Level 2: Setup Unique Obstacles")]
    static void Setup()
    {
        GameObject parkingZone = GameObject.Find("ParkingZone");
        if (parkingZone == null)
        {
            Debug.LogError("ParkingZone not found!");
            return;
        }

        Transform parkingTf = parkingZone.transform;
        Vector3 oldPos = parkingTf.position;
        Vector3 newPos = new Vector3(60f, 0.87f, 60f);
        parkingTf.position = newPos;
        EditorUtility.SetDirty(parkingZone);
        Debug.Log($"ParkingZone moved: {oldPos} -> {newPos}");

        GameObject fenceTemplate = FindFirstByName(BarrierFenceName);
        if (fenceTemplate == null)
        {
            Debug.LogError("Barrier Fence not found!");
            return;
        }

        GameObject coneTemplate = FindFirstByName(ConeName);
        if (coneTemplate == null)
        {
            Debug.LogError("Traffic Cone not found!");
            return;
        }

        float corridorLeft = 50f;
        float corridorRight = 70f;
        float corridorStart = 35f;
        float corridorEnd = 58f;
        float step = 8f;

        int barrierCount = 0;
        for (float z = corridorStart; z <= corridorEnd; z += step)
        {
            DuplicateTo(fenceTemplate, new Vector3(corridorLeft, 0f, z), "NarrowFence_L");
            DuplicateTo(fenceTemplate, new Vector3(corridorRight, 0f, z), "NarrowFence_R");
            barrierCount += 2;
        }

        DuplicateTo(fenceTemplate, new Vector3(55f, 0f, 63f), "NarrowFence_BL");
        DuplicateTo(fenceTemplate, new Vector3(65f, 0f, 63f), "NarrowFence_BR");
        barrierCount += 2;

        float[] conePositions_x = { 54f, 56f, 58f, 60f, 62f, 64f, 66f };
        int coneCount = 0;
        foreach (float x in conePositions_x)
        {
            DuplicateTo(coneTemplate, new Vector3(x, 0f, 65f), "NarrowCone");
            coneCount++;
        }

        float[] extraCones_x = { 50f, 70f };
        float[] extraCones_z = { 40f, 48f, 56f };
        foreach (float x in extraCones_x)
        {
            foreach (float z in extraCones_z)
            {
                DuplicateTo(coneTemplate, new Vector3(x, 0f, z), "ApproachCone");
                coneCount++;
            }
        }

        Debug.Log($"Level 2 obstacles created: {barrierCount} fences, {coneCount} cones. ParkingZone moved to {newPos}");
        Debug.Log("DONE: Level 2 now has unique obstacles different from Level 1!");
    }

    [MenuItem("Tools/Level 2: Undo Unique Obstacles")]
    static void UndoSetup()
    {
        GameObject parkingZone = GameObject.Find("ParkingZone");
        if (parkingZone != null)
        {
            parkingZone.transform.position = new Vector3(221.99f, 0.87f, 22.53f);
            EditorUtility.SetDirty(parkingZone);
        }

        int count = 0;
        string[] names = { "NarrowFence_L", "NarrowFence_R", "NarrowFence_BL", "NarrowFence_BR",
                           "NarrowCone", "ApproachCone" };
        foreach (string name in names)
        {
            GameObject obj = GameObject.Find(name);
            while (obj != null)
            {
                Undo.DestroyObjectImmediate(obj);
                count++;
                obj = GameObject.Find(name);
            }
        }
        Debug.Log($"Undid Level 2 setup: moved ParkingZone back, removed {count} objects.");
    }

    static GameObject FindFirstByName(string searchName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in allObjects)
        {
            if (go.hideFlags != HideFlags.None) continue;
            if (go.name == searchName) return go;
        }
        return null;
    }

    static GameObject DuplicateTo(GameObject template, Vector3 position, string newName)
    {
        GameObject clone = GameObject.Instantiate(template);
        clone.name = newName;
        clone.transform.position = position;
        Undo.RegisterCreatedObjectUndo(clone, "Create Obstacle");

        Collider col = clone.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        if (clone.CompareTag("Untagged"))
            clone.tag = "Obstacle";

        EditorUtility.SetDirty(clone);
        return clone;
    }
}
