using UnityEngine;
using System.Collections.Generic;

namespace ezAR.Model
{
    /// <summary>
    /// PC端模型鼠标交互控制器
    /// 支持鼠标拖拽移动、旋转、滚轮缩放
    /// 支持点击高亮选中效果
    /// </summary>
    public class ModelMouseHandler : MonoBehaviour
    {
        [Header("Target (Optional)")]
        [SerializeField] private Transform targetTransform;
        [SerializeField] private ModelTransformController transformController;

        [Header("Camera")]
        [SerializeField] private Camera arCamera;

        [Header("Interaction Settings")]
        [SerializeField] private float moveSensitivity = 1f;  // 移动灵敏度
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float scaleSpeed = 0.1f;
        [SerializeField] private float minScale = 0.01f;
        [SerializeField] private float maxScale = 10f;
        [SerializeField] private bool enableInteraction = true;

        [Header("Selection Settings")]
        [SerializeField] private LayerMask selectableLayers = -1;
        [SerializeField] private Color highlightColor = new Color(0f, 0.8f, 1f, 1f);
        [SerializeField] private float highlightIntensity = 1.5f;
        [SerializeField] private bool highlightOnSelect = true;

        [Header("Keyboard Shortcuts")]
        [SerializeField] private KeyCode resetKey = KeyCode.R;
        [SerializeField] private KeyCode yMoveModifier = KeyCode.LeftShift;

        // 状态
        private bool isDragging = false;
        private bool isRotating = false;
        private Vector3 lastMousePosition;

        // 原始变换（用于重置）
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;

        // 拖拽相关
        private float dragDistance;  // 点击时物体到相机的距离

        // 高亮相关
        private GameObject selectedObject;
        private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
        private List<Material> highlightMaterials = new List<Material>();
        private Shader highlightShader;

        public bool EnableInteraction
        {
            get => enableInteraction;
            set => enableInteraction = value;
        }

        public GameObject SelectedObject => selectedObject;

        private void Awake()
        {
                if (!arCamera)
                {
                    arCamera = Camera.main;
                }

                if (targetTransform == null)
                {
                    targetTransform = transform;
                }

                SaveOriginalTransform();
                highlightShader = Shader.Find("Standard");
            }

        private void OnDestroy()
        {
            ClearHighlight();
        }

        private void SaveOriginalTransform()
        {
            if (targetTransform != null)
            {
                originalPosition = targetTransform.localPosition;
                originalRotation = targetTransform.localRotation;
                originalScale = targetTransform.localScale;
            }
        }

        private void Update()
        {
            if (!enableInteraction) return;

            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleMouseInput()
        {
            // ========== 左键点击/拖拽 ==========
            if (Input.GetMouseButtonDown(0))
            {
                HandleLeftButtonDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // ========== 右键旋转 ==========
            if (Input.GetMouseButtonDown(1))
            {
                isRotating = true;
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isRotating = false;
            }

            // ========== 处理拖拽（增量方式）==========
            if (isDragging && targetTransform != null)
            {
                HandleDrag();
            }

            // ========== 处理旋转 ==========
            if (isRotating && targetTransform != null)
            {
                HandleRotate();
            }

            // ========== 滚轮缩放 ==========
            HandleScroll();
        }

        private void HandleLeftButtonDown()
        {
            if (arCamera == null) return;

            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, selectableLayers))
            {
                GameObject hitObject = hit.collider.gameObject;

                // 选择新对象
                if (hitObject != selectedObject)
                {
                    ClearHighlight();
                    selectedObject = hitObject;
                    targetTransform = hitObject.transform;
                    SaveOriginalTransform();

                    if (highlightOnSelect)
                    {
                        ApplyHighlight(hitObject);
                    }

                    Debug.Log($"[ModelMouseHandler] Selected: {hitObject.name}");
                }

                // 开始拖拽
                isDragging = true;
                lastMousePosition = Input.mousePosition;

                // 记录物体到相机的距离（用于计算移动比例）
                dragDistance = Vector3.Distance(targetTransform.position, arCamera.transform.position);
            }
            else
            {
                // 点击空白处取消选择
                if (selectedObject != null)
                {
                    ClearHighlight();
                    selectedObject = null;
                    targetTransform = null;
                    isDragging = false;
                }
            }
        }

        private void HandleDrag()
        {
            // 获取屏幕像素增量
            Vector3 screenDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // 根据物体到相机的距离计算移动比例
            float scaleFactor = dragDistance * 0.002f * moveSensitivity;

            if (Input.GetKey(yMoveModifier))
            {
                // ========== Y轴移动模式 ==========
                float worldMoveY = screenDelta.y * scaleFactor;
                targetTransform.position += Vector3.up * worldMoveY;

                // 更新拖拽距离
                dragDistance = Vector3.Distance(targetTransform.position, arCamera.transform.position);
            }
            else
            {
                // ========== 水平面移动模式（视图相关）==========
                // 使用射线投射计算移动，确保屏幕移动方向与世界移动方向完全一致

                Vector3 worldPos = targetTransform.position;

                // 构建一个与相机视线垂直的平面，经过物体位置
                Plane dragPlane = new Plane(arCamera.transform.forward, worldPos);

                // 计算上一帧鼠标位置对应的世界位置
                Ray lastRay = arCamera.ScreenPointToRay(lastMousePosition - screenDelta);
                float lastDistance;
                dragPlane.Raycast(lastRay, out lastDistance);
                Vector3 lastWorldPos = lastRay.GetPoint(lastDistance);

                // 计算当前鼠标位置对应的世界位置
                Ray currentRay = arCamera.ScreenPointToRay(lastMousePosition);
                float currentDistance;
                dragPlane.Raycast(currentRay, out currentDistance);
                Vector3 currentWorldPos = currentRay.GetPoint(currentDistance);

                // 计算世界空间移动量
                Vector3 worldDelta = currentWorldPos - lastWorldPos;

                // 投影到水平面（只保留XZ分量）
                worldDelta.y = 0;

                // 应用到物体位置
                targetTransform.position += worldDelta;

                // 更新拖拽距离
                dragDistance = Vector3.Distance(targetTransform.position, arCamera.transform.position);
            }
        }

        private void HandleRotate()
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotX = -delta.y * rotateSpeed;
            float rotY = delta.x * rotateSpeed;
            targetTransform.rotation *= Quaternion.Euler(rotX, rotY, 0);
        }

        private void HandleScroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f && targetTransform != null)
            {
                // 保存世界位置和旋转（因为修改localScale在父对象有缩放时会影响世界位置）
                Vector3 worldPos = targetTransform.position;
                Quaternion worldRot = targetTransform.rotation;

                float currentScale = targetTransform.localScale.x;
                float newScale = currentScale + scroll * scaleSpeed;
                newScale = Mathf.Clamp(newScale, minScale, maxScale);
                targetTransform.localScale = Vector3.one * newScale;

                // 恢复世界位置和旋转
                targetTransform.position = worldPos;
                targetTransform.rotation = worldRot;

                // 更新拖拽距离
                dragDistance = Vector3.Distance(targetTransform.position, arCamera.transform.position);
            }
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKeyDown(resetKey))
            {
                ResetTransform();
                Debug.Log("[ModelMouseHandler] Transform reset");
            }
        }

        #region 高亮功能

        private void ApplyHighlight(GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;

            foreach (var renderer in renderers)
            {
                originalMaterials[renderer] = renderer.sharedMaterials;

                Material[] highlightMats = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < highlightMats.Length; i++)
                {
                    Material highlightMat = new Material(highlightShader);
                    highlightMat.CopyPropertiesFromMaterial(renderer.sharedMaterials[i]);
                    highlightMat.EnableKeyword("_EMISSION");
                    highlightMat.SetColor("_EmissionColor", highlightColor * highlightIntensity);
                    highlightMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    highlightMats[i] = highlightMat;
                    highlightMaterials.Add(highlightMat);
                }

                renderer.materials = highlightMats;
            }
        }

        private void ClearHighlight()
        {
            foreach (var kvp in originalMaterials)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.materials = kvp.Value;
                }
            }
            originalMaterials.Clear();

            foreach (var mat in highlightMaterials)
            {
                if (mat != null)
                {
                    Destroy(mat);
                }
            }
            highlightMaterials.Clear();
        }

        #endregion

        #region 公共方法

        public void ResetTransform()
        {
            if (targetTransform != null)
            {
                targetTransform.localPosition = originalPosition;
                targetTransform.localRotation = originalRotation;
                targetTransform.localScale = originalScale;
            }
            else if (transformController != null)
            {
                transformController.ResetTransform();
            }
        }

        public void SelectObject(GameObject obj)
        {
            if (obj == null) return;

            ClearHighlight();
            selectedObject = obj;
            targetTransform = obj.transform;
            SaveOriginalTransform();

            if (highlightOnSelect)
            {
                ApplyHighlight(obj);
            }
        }

        public void Deselect()
        {
            ClearHighlight();
            selectedObject = null;
        }

        #endregion
    }
}
