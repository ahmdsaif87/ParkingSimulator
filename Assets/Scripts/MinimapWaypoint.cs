using UnityEngine;

public class MinimapWaypoint : MonoBehaviour
{
    public Transform target;
    public GameObject pointerPrefab;

    Camera minimapCam;
    GameObject pointer;
    Renderer pointerRenderer;

    void Start()
    {
        var camGO = GameObject.Find("MinimapCamera");
        if (camGO != null) minimapCam = camGO.GetComponent<Camera>();

        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointer.name = "MinimapPointer";
        pointer.transform.localScale = new Vector3(8f, 8f, 8f);
        pointer.layer = 6;

        pointerRenderer = pointer.GetComponent<MeshRenderer>();
        pointerRenderer.material = new Material(Shader.Find("Unlit/Color"));
        pointerRenderer.material.color = Color.red;

        var col = pointer.GetComponent<Collider>();
        if (col != null) Destroy(col);
    }

    void LateUpdate()
    {
        if (target == null || minimapCam == null || pointer == null) return;

        Vector3 dirToTarget = target.position - transform.position;
        float dist = dirToTarget.magnitude;
        float viewRadius = minimapCam.orthographicSize;

        if (dist < viewRadius * 0.8f)
        {
            pointer.SetActive(false);
            return;
        }

        pointer.SetActive(true);

        Vector3 dirNorm = dirToTarget.normalized;
        float edgeDist = viewRadius * 0.85f;
        Vector3 worldPos = transform.position + dirNorm * edgeDist;
        worldPos.y = 0.2f;

        pointer.transform.position = worldPos;
        pointer.transform.rotation = Quaternion.identity;
    }
}
