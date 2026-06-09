using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform parkingZone;
    public Camera minimapCamera;
    public RawImage minimapRawImage;
    public RectTransform playerArrow;
    public RectTransform parkingDot;
    public RectTransform minimapBorder;

    [Header("Settings")]
    public float mapSize = 80f;
    public float minimapSize = 200f;
    public float playerArrowSize = 20f;
    public float parkingDotSize = 12f;
    public Color parkingDotColor = Color.red;
    public Color playerArrowColor = Color.yellow;

    void Start()
    {
        if (player == null)
        {
            GameObject car = GameObject.Find("MuscleCar");
            if (car != null) player = car.transform;
        }

        if (parkingZone == null)
        {
            GameObject pz = GameObject.Find("ParkingZone");
            if (pz != null) parkingZone = pz.transform;
        }

        if (minimapCamera == null)
        {
            GameObject camGO = GameObject.Find("MinimapCamera");
            if (camGO != null) minimapCamera = camGO.GetComponent<Camera>();
        }

        if (minimapCamera != null)
        {
            minimapCamera.orthographic = true;
            minimapCamera.orthographicSize = mapSize * 0.5f;
            minimapCamera.nearClipPlane = 0.3f;
            minimapCamera.farClipPlane = 100f;
            minimapCamera.backgroundColor = new Color(0.12f, 0.18f, 0.12f, 1f);
            minimapCamera.clearFlags = CameraClearFlags.SolidColor;
            minimapCamera.cullingMask = (1 << 0) | (1 << 6);
        }

        if (parkingDot != null)
        {
            Image dotImg = parkingDot.GetComponent<Image>();
            if (dotImg != null) dotImg.color = parkingDotColor;
            parkingDot.sizeDelta = new Vector2(parkingDotSize, parkingDotSize);
        }

        if (playerArrow != null)
        {
            Image arrowImg = playerArrow.GetComponent<Image>();
            if (arrowImg != null) arrowImg.color = playerArrowColor;
            playerArrow.sizeDelta = new Vector2(playerArrowSize, playerArrowSize);
        }

        if (minimapBorder != null)
        {
            minimapBorder.sizeDelta = new Vector2(minimapSize, minimapSize);
        }
    }

    void LateUpdate()
    {
        if (player == null || parkingZone == null || minimapCamera == null) return;

        Vector3 playerPos = player.position;
        float playerAngle = player.eulerAngles.y;

        Vector3 camPos = playerPos;
        camPos.y = 50f;
        minimapCamera.transform.position = camPos;
        minimapCamera.transform.rotation = Quaternion.Euler(90f, playerAngle, 0f);

        Vector3 worldOffset = parkingZone.position - playerPos;
        float angleRad = playerAngle * Mathf.Deg2Rad;

        Vector3 playerForward = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));
        Vector3 playerRight = new Vector3(Mathf.Cos(angleRad), 0, -Mathf.Sin(angleRad));

        float forwardDist = Vector3.Dot(worldOffset, playerForward);
        float rightDist = Vector3.Dot(worldOffset, playerRight);

        float mapPixelScale = minimapSize / mapSize;
        float px = rightDist * mapPixelScale;
        float py = forwardDist * mapPixelScale;

        float dist = Mathf.Sqrt(px * px + py * py);
        float maxDist = minimapSize * 0.42f;

        bool isClamped = dist > maxDist;
        if (isClamped)
        {
            float scale = maxDist / dist;
            px *= scale;
            py *= scale;
        }

        parkingDot.anchoredPosition = new Vector2(px, py);
        parkingDot.gameObject.SetActive(true);
        parkingDot.localScale = isClamped ? Vector3.one * 1.3f : Vector3.one;
    }
}
