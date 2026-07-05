using UnityEngine;
using UnityEngine.UI;

namespace EasyVehicleSteering
{
    [ExecuteAlways]
    public class GasPedal : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private float pressTravel = 14f;
        [SerializeField] private float pressSmoothSpeed = 10f;

        [Header("Layout")]
        [SerializeField] private Vector2 pedalSize = new Vector2(80, 180);
        [SerializeField] private float pedalGap = 12f;

        private PedalUI gasPedalUI;
        private PedalUI brakePedalUI;
        private Camera uiCam;

        private bool isDrive = true;
        private Image driveBtnImage;
        private Image reverseBtnImage;
        private Color driveActiveColor = new Color(0.2f, 0.6f, 0.2f, 0.85f);
        private Color driveInactiveColor = new Color(0.35f, 0.35f, 0.35f, 0.7f);
        private Color reverseActiveColor = new Color(0.7f, 0.2f, 0.2f, 0.85f);
        private Color reverseInactiveColor = new Color(0.35f, 0.35f, 0.35f, 0.7f);

        class PedalUI
        {
            public RectTransform touchRect;
            public RectTransform pressRT;
            public float pressure;
            public float targetPressure;
            public Color surfaceColor;
            public Color surfacePressedColor;
        }

        void OnEnable()
        {
            Canvas c = GetComponentInParent<Canvas>();
            if (c == null) return;
            uiCam = (c.renderMode != RenderMode.ScreenSpaceOverlay) ? c.worldCamera : null;

            if (gasPedalUI != null && brakePedalUI != null && driveBtnImage != null) return;

            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);

            BuildAll();
            isDrive = true;
        }

        void Update()
        {
            if (!Application.isPlaying || gasPedalUI == null) return;
            ReadPressure(gasPedalUI);
            ReadPressure(brakePedalUI);
            Animate(gasPedalUI);
            Animate(brakePedalUI);

            float gas = gasPedalUI.pressure;
            float brake = brakePedalUI.pressure;

            if (gas > 0.05f && brake > 0.05f)
            {
                if (gas >= brake) brake = 0f;
                else gas = 0f;
            }

            if (gas > 0.05f)
            {
                float dir = isDrive ? gas : -gas;
                InputHandler.SetSimulatedVertical(dir);
                InputHandler.SetBrake(false);
            }
            else if (brake > 0.05f)
            {
                InputHandler.SetSimulatedVertical(-brake);
                InputHandler.SetBrake(true);
            }
            else
            {
                InputHandler.ClearSimulatedVertical();
                InputHandler.SetBrake(false);
            }
        }

        // ───────────────────── Input ─────────────────────

        void ReadPressure(PedalUI p)
        {
            bool touching = false;
            Vector2 screenPos = Vector2.zero;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) continue;
                if (RectTransformUtility.RectangleContainsScreenPoint(p.touchRect, t.position, uiCam))
                {
                    touching = true;
                    screenPos = t.position;
                    break;
                }
            }

            if (!touching && Input.GetMouseButton(0))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(p.touchRect, Input.mousePosition, uiCam))
                {
                    touching = true;
                    screenPos = Input.mousePosition;
                }
            }

            if (touching)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    p.touchRect, screenPos, uiCam, out Vector2 lp);
                float h = p.touchRect.rect.height;
                p.targetPressure = Mathf.Clamp01(Mathf.InverseLerp(h * 0.35f, -h * 0.35f, lp.y));
            }
            else
            {
                p.targetPressure = 0f;
            }

            p.pressure = Mathf.Lerp(p.pressure, p.targetPressure, Time.deltaTime * pressSmoothSpeed);
        }

        void Animate(PedalUI p)
        {
            Vector3 pos = p.pressRT.localPosition;
            pos.y = Mathf.Lerp(pos.y, -p.pressure * pressTravel, Time.deltaTime * pressSmoothSpeed * 1.5f);
            p.pressRT.localPosition = pos;

            p.pressRT.GetComponent<Image>().color = Color.Lerp(p.surfaceColor, p.surfacePressedColor, p.pressure * 0.4f);
        }

        // ───────────────────── Visual Builder ─────────────────────

        void BuildAll()
        {
            float offsetX = pedalGap * 0.5f + pedalSize.x * 0.5f;

            gasPedalUI = BuildSinglePedal("PedalGas", new Vector2(offsetX, 0),
                new Color(0.22f, 0.52f, 0.22f), new Color(0.3f, 0.65f, 0.3f));

            brakePedalUI = BuildSinglePedal("PedalBrake", new Vector2(-offsetX, 0),
                new Color(0.55f, 0.14f, 0.14f), new Color(0.7f, 0.18f, 0.18f));

            BuildGearButtons();
            UpdateGearVisual();
        }

        PedalUI BuildSinglePedal(string name, Vector2 pos, Color bodyColor, Color surfaceColor)
        {
            PedalUI p = new PedalUI();
            p.surfaceColor = surfaceColor;
            p.surfacePressedColor = surfaceColor * 0.7f;

            float w = pedalSize.x;
            float h = pedalSize.y;

            // ── Root ──
            GameObject root = new GameObject(name, typeof(RectTransform));
            root.transform.SetParent(transform, false);
            RectTransform rt = root.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0);
            rt.anchorMax = new Vector2(0.5f, 0);
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(w, h);

            // ── Shadow ──
            MakeImage(root.transform, "Shadow", Vector2.zero, new Vector2(w + 6, h + 6), new Color(0, 0, 0, 0.45f));

            // ── Pedal body (static, dark frame) ──
            MakeImage(root.transform, "Body", Vector2.zero, new Vector2(w, h), bodyColor);

            // ── Body grip ridges (static, revealed when surface depresses) ──
            for (int i = -2; i <= 2; i++)
            {
                MakeImage(root.transform, "BodyRidge",
                    new Vector2(0, i * (h * 0.15f)),
                    new Vector2(w - 14, 2.5f),
                    new Color(0, 0, 0, 0.3f));
            }

            // ── Side edge highlights (left + right thin bright lines) ──
            MakeImage(root.transform, "EdgeL",
                new Vector2(-w * 0.5f + 2, 0), new Vector2(2, h - 10),
                new Color(1, 1, 1, 0.06f));
            MakeImage(root.transform, "EdgeR",
                new Vector2(w * 0.5f - 2, 0), new Vector2(2, h - 10),
                new Color(1, 1, 1, 0.06f));

            // ── Press surface (moves down when pressed) ──
            float sw = w - 12;
            float sh = h - 14;

            GameObject surfGO = new GameObject("Surface", typeof(RectTransform));
            surfGO.transform.SetParent(root.transform, false);
            RectTransform surfRT = surfGO.GetComponent<RectTransform>();
            surfRT.anchorMin = new Vector2(0.5f, 0.5f);
            surfRT.anchorMax = new Vector2(0.5f, 0.5f);
            surfRT.anchoredPosition = new Vector2(0, 2);
            surfRT.sizeDelta = new Vector2(sw, sh);
            Image surfImg = surfGO.AddComponent<Image>();
            surfImg.color = surfaceColor;
            p.pressRT = surfRT;

            // ── Surface grip ridges ──
            for (int i = -2; i <= 2; i++)
            {
                MakeImage(surfGO.transform, "SGrip",
                    new Vector2(0, i * (sh * 0.15f)),
                    new Vector2(sw - 10, 2),
                    new Color(0, 0, 0, 0.22f));
            }

            // ── Top highlight bar ──
            MakeImage(surfGO.transform, "TopHL",
                new Vector2(0, sh * 0.5f - 3),
                new Vector2(sw - 8, 3),
                new Color(1, 1, 1, 0.18f));

            // ── Bottom shadow on surface ──
            MakeImage(surfGO.transform, "BotShadow",
                new Vector2(0, -sh * 0.5f + 3),
                new Vector2(sw - 8, 3),
                new Color(0, 0, 0, 0.2f));

            // ── Touch zone (transparent, full size) ──
            GameObject tgo = new GameObject("Touch", typeof(RectTransform));
            tgo.transform.SetParent(root.transform, false);
            RectTransform trt = tgo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.sizeDelta = Vector2.zero;
            Image timg = tgo.AddComponent<Image>();
            timg.color = new Color(0, 0, 0, 0.01f);
            timg.raycastTarget = true;
            p.touchRect = trt;

            return p;
        }

        // ───────────────────── Gear Buttons ─────────────────────

        void BuildGearButtons()
        {
            float offsetX = pedalGap * 0.5f + pedalSize.x * 0.5f;
            float btnW = 55f;
            float btnH = 32f;
            float btnY = pedalSize.y + 12f;

            driveBtnImage = BuildGearBtn("BtnD", "D",
                new Vector2(-offsetX, btnY), new Vector2(btnW, btnH),
                driveActiveColor, OnDriveClick);

            reverseBtnImage = BuildGearBtn("BtnR", "R",
                new Vector2(offsetX, btnY), new Vector2(btnW, btnH),
                reverseActiveColor, OnReverseClick);
        }

        Image BuildGearBtn(string name, string label, Vector2 pos, Vector2 size, Color color, UnityEngine.Events.UnityAction onClick)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(transform, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0);
            rt.anchorMax = new Vector2(0.5f, 0);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            Image img = go.GetComponent<Image>();
            img.color = color;

            Button btn = go.GetComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(onClick);

            GameObject lbl = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            lbl.transform.SetParent(go.transform, false);
            RectTransform lr = lbl.GetComponent<RectTransform>();
            lr.anchorMin = Vector2.zero;
            lr.anchorMax = Vector2.one;
            lr.sizeDelta = Vector2.zero;
            Text t = lbl.GetComponent<Text>();
            t.text = label;
            t.alignment = TextAnchor.MiddleCenter;
            t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            t.fontSize = 22;
            t.fontStyle = FontStyle.Bold;
            t.color = Color.white;
            t.raycastTarget = false;

            return img;
        }

        void OnDriveClick()
        {
            isDrive = true;
            UpdateGearVisual();
        }

        void OnReverseClick()
        {
            isDrive = false;
            UpdateGearVisual();
        }

        void UpdateGearVisual()
        {
            if (driveBtnImage != null)
                driveBtnImage.color = isDrive ? driveActiveColor : driveInactiveColor;
            if (reverseBtnImage != null)
                reverseBtnImage.color = isDrive ? reverseInactiveColor : reverseActiveColor;
        }

        // ───────────────────── Utility ─────────────────────

        static Image MakeImage(Transform parent, string name, Vector2 pos, Vector2 size, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            Image img = go.GetComponent<Image>();
            img.color = color;
            img.raycastTarget = false;
            return img;
        }
    }
}
