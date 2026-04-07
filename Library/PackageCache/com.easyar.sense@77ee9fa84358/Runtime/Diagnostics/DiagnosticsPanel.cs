//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace easyar
{
    [DisallowMultipleComponent]
    internal class DiagnosticsPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectT;
        private RectTransform canvasRectT;
        private readonly Vector2 size = new Vector2(120, 120);
        private Vector2 canvasSize;
        private Vector2 dragOffset;

        public Toggle SessionToggle { get; private set; }
        public Button SessionCopyButton { get; private set; }
        public Button EifButton { get; private set; }
        public Button EifFormatButton { get; private set; }
        public Button EedButton { get; private set; }

        private void Awake()
        {
            canvasRectT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            canvasSize = canvasRectT.rect.size;

            rectT = GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(1, 0.5f);
            rectT.anchorMax = new Vector2(1, 0.5f);
            rectT.offsetMin = new Vector2(-120, -120);
            rectT.offsetMax = rectT.offsetMin + size;

            var space = 0.05f;
            var spaceLine = 0.05f;
            var cell = new Vector2((1 - space * 4) / 3, (1 - space * 5) / 4);


            // session
            var anchorMin = new Vector2(space, 1 - (space + cell.y));
            var anchorMax = anchorMin + cell;
            var sessionTitle = DefaultControls.CreateText(new DefaultControls.Resources()).GetComponent<Text>();
            SetupDeveloperUIObj(sessionTitle.gameObject, gameObject, anchorMin, anchorMax);
            sessionTitle.text = "session";
            sessionTitle.alignment = TextAnchor.MiddleLeft;
            sessionTitle.fontSize = 9;

            anchorMin = anchorMin + new Vector2(cell.x + space, 0);
            anchorMax = anchorMin + cell;
            SessionToggle = DefaultControls.CreateToggle(new DefaultControls.Resources()).GetComponent<Toggle>();
            SetupDeveloperUIObj(SessionToggle.gameObject, gameObject, anchorMin, anchorMax);
            SessionToggle.GetComponentInChildren<Text>().gameObject.SetActive(false);
            foreach (Transform transform in SessionToggle.transform)
            {
                var t = transform.GetComponent<RectTransform>();
                t.offsetMin = Vector2.zero;
                t.offsetMax = Vector2.zero;
                t.anchorMin = Vector2.zero;
                t.anchorMax = Vector2.one;
                foreach (var image in transform.GetComponentsInChildren<UnityEngine.UI.Image>().Where(i => i.transform != transform))
                {
                    var ti = image.GetComponent<RectTransform>();
                    ti.offsetMin = Vector2.zero;
                    ti.offsetMax = Vector2.zero;
                    ti.anchorMin = new Vector2(0.1f, 0.1f);
                    ti.anchorMax = new Vector2(1 - 0.1f, 1 - 0.1f);
                    image.color = new Color(0.3f, 0.3f, 0.3f);
                }
            }

            anchorMin = anchorMin + new Vector2(cell.x + space, 0);
            anchorMax = anchorMin + cell;
            SessionCopyButton = DefaultControls.CreateButton(new DefaultControls.Resources()).GetComponent<Button>();
            SetupDeveloperUIObj(SessionCopyButton.gameObject, gameObject, anchorMin, anchorMax);
            SessionCopyButton.GetComponentInChildren<Text>().text = "copy";


            // eif
            anchorMin = new Vector2(space, 1 - (space + cell.y) * 2 - spaceLine);
            anchorMax = anchorMin + cell;
            var eifTitle = DefaultControls.CreateText(new DefaultControls.Resources()).GetComponent<Text>();
            SetupDeveloperUIObj(eifTitle.gameObject, gameObject, anchorMin, anchorMax);
            eifTitle.text = "eif";
            eifTitle.alignment = TextAnchor.MiddleLeft;

            anchorMin = anchorMin + new Vector2(cell.x + space, 0);
            anchorMax = anchorMin + cell;
            EifFormatButton = DefaultControls.CreateButton(new DefaultControls.Resources()).GetComponent<Button>();
            EifFormatButton.interactable = false;
            SetupDeveloperUIObj(EifFormatButton.gameObject, gameObject, anchorMin, anchorMax);
            EifFormatButton.GetComponentInChildren<Text>().fontSize = 8;

            anchorMin = anchorMin + new Vector2(cell.x + space, 0);
            anchorMax = anchorMin + cell;
            EifButton = DefaultControls.CreateButton(new DefaultControls.Resources()).GetComponent<Button>();
            EifButton.interactable = false;
            SetupDeveloperUIObj(EifButton.gameObject, gameObject, anchorMin, anchorMax);
            EifButton.GetComponentInChildren<Text>().text = "rec";


            // eed
            anchorMin = new Vector2(space, 1 - (space + cell.y) * 3 - spaceLine * 2);
            anchorMax = anchorMin + cell;
            var eedTitle = DefaultControls.CreateText(new DefaultControls.Resources()).GetComponent<Text>();
            SetupDeveloperUIObj(eedTitle.gameObject, gameObject, anchorMin, anchorMax);
            eedTitle.text = "eed";
            eedTitle.alignment = TextAnchor.MiddleLeft;

            anchorMin = anchorMin + new Vector2(cell.x + space, 0) * 2;
            anchorMax = anchorMin + cell;
            EedButton = DefaultControls.CreateButton(new DefaultControls.Resources()).GetComponent<Button>();
            EedButton.interactable = false;
            SetupDeveloperUIObj(EedButton.gameObject, gameObject, anchorMin, anchorMax);
            EedButton.GetComponentInChildren<Text>().text = "rec";


            // title
            anchorMin = new Vector2(space, 0.02f);
            anchorMax = anchorMin + new Vector2(1 - space * 2, 0.1f);
            var panelTitle = DefaultControls.CreateText(new DefaultControls.Resources()).GetComponent<Text>();
            SetupDeveloperUIObj(panelTitle.gameObject, gameObject, anchorMin, anchorMax);
            panelTitle.text = "EasyAR Diagnostics";
            panelTitle.alignment = TextAnchor.LowerCenter;
            panelTitle.resizeTextForBestFit = true;
        }

        private void Update()
        {
            if (canvasSize != canvasRectT.rect.size)
            {
                canvasSize = canvasRectT.rect.size;
                ResetRect();
            }
        }

        public static (GameObject obj, DiagnosticsPanel panel) CreatePanel()
        {
            if (EventSystem.current == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            var devCanvas = new GameObject("EasyAR Diagnostics Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            var canvas = devCanvas.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var uiLayer = LayerMask.NameToLayer("UI");
            if (uiLayer >= 0) { canvas.sortingLayerID = uiLayer; }

            var scaler = devCanvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(600, 600);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            var topPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            topPanel.transform.SetParent(devCanvas.transform, false);

            var diagnosticsPanel = topPanel.AddComponent<DiagnosticsPanel>();

            return (devCanvas, diagnosticsPanel);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            dragOffset = rectT.offsetMin - eventData.position / canvasRectT.localScale;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rectT.offsetMin = dragOffset + eventData.position / canvasRectT.localScale;
            rectT.offsetMax = rectT.offsetMin + size;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            ResetRect();
        }

        private void ResetRect()
        {
            rectT.offsetMin = new Vector3(
                Mathf.Clamp(rectT.offsetMin.x, -canvasSize.x, -rectT.rect.width),
                Mathf.Clamp(rectT.offsetMin.y, -canvasSize.y / 2, canvasSize.y / 2 - rectT.rect.height)
            );
            rectT.offsetMax = rectT.offsetMin + size;
        }

        private void SetupDeveloperUIObj(GameObject obj, GameObject topPanel, Vector2 anchorMin, Vector2 anchorMax)
        {
            obj.transform.SetParent(topPanel.transform, false);
            var panelT = obj.GetComponent<RectTransform>();
            panelT.offsetMin = Vector2.zero;
            panelT.offsetMax = Vector2.zero;
            panelT.anchorMin = anchorMin;
            panelT.anchorMax = anchorMax;
        }
    }
}
