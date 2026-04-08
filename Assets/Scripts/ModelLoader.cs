using UnityEngine;

namespace ezAR.Model
{
    public class ModelLoader : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private ModelMouseHandler mouseHandler;

        [Header("Auto Scale Settings")]
        [SerializeField] private bool autoScale = true;
        [SerializeField] private float targetSize = 0.2f;  // 目标最大尺寸（米）
        [SerializeField] private bool centerModel = true;
        [SerializeField] private bool clearTestModelsOnStart = true;  // 运行时清除测试模型

        private GameObject currentModel;

        public GameObject CurrentModel => currentModel;
        public Transform ContentRoot => contentRoot;

        private void Start()
        {
            // 运行时自动清除测试模型
            if (clearTestModelsOnStart && contentRoot != null)
            {
                ClearTestModels();
            }
        }

        /// <summary>
        /// 清除测试模型（ARContentRoot自身的Mesh组件）
        /// </summary>
        private void ClearTestModels()
        {
            if (contentRoot == null) return;

            var meshFilter = contentRoot.GetComponent<MeshFilter>();
            var meshRenderer = contentRoot.GetComponent<MeshRenderer>();
            var boxCollider = contentRoot.GetComponent<BoxCollider>();

            bool isTestModel = meshFilter != null && meshRenderer != null;
            bool isARContentRoot = meshFilter != null && meshRenderer != null && boxCollider != null;

            if (isARContentRoot)
            {
                Debug.Log("[ModelLoader] Clearing test model components from ARContentRoot");

                // 清除测试模型组件，但保留Transform和脚本
                if (meshFilter != null) Destroy(meshFilter);
                if (meshRenderer != null) Destroy(meshRenderer);
                if (boxCollider != null) Destroy(boxCollider);

                Debug.Log("[ModelLoader] Test model cleared");
            }
            else if (isTestModel)
            {
                Debug.LogWarning($"[ModelLoader] Found potential test model on ContentRoot: {meshFilter?.sharedMesh?.name}");
            }
            else
            {
                Debug.Log("[ModelLoader] No test model found on ContentRoot");
            }
        }

        /// <summary>
        /// 从文件路径加载STL模型（自动检测格式）
        /// </summary>
        public void LoadStlFromPath(string filePath)
        {
            var mesh = StlImporter.ImportFromFile(filePath);
            if (mesh == null)
            {
                Debug.LogError($"[ModelLoader] Failed to load STL: {filePath}");
                return;
            }

            CreateModelFromMesh(mesh, "LoadedModel_STL");
            Debug.Log($"[ModelLoader] Loaded STL model from: {filePath}");
        }

        /// <summary>
        /// 从Mesh创建模型GameObject
        /// </summary>
        private void CreateModelFromMesh(Mesh mesh, string name)
        {
            ClearCurrentModel();

            var go = new GameObject(name);
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();
            var collider = go.AddComponent<MeshCollider>();

            filter.mesh = mesh;
            collider.sharedMesh = mesh;

            // 设置材质
            if (defaultMaterial)
            {
                renderer.material = defaultMaterial;
            }
            else
            {
                renderer.material = new Material(Shader.Find("Standard"));
            }

            // 设置父对象
            if (contentRoot)
            {
                go.transform.SetParent(contentRoot, false);
            }

            // 自动缩放和居中
            if (autoScale)
            {
                AutoScaleModel(go);
            }

            if (centerModel)
            {
                CenterModel(go);
            }

            currentModel = go;

            // 自动选中模型（如果ModelMouseHandler存在）
            if (mouseHandler != null)
            {
                mouseHandler.SelectObject(go);
            }
        }

        /// <summary>
        /// 自动缩放模型到目标尺寸
        /// </summary>
        private void AutoScaleModel(GameObject model)
        {
            var bounds = CalculateBounds(model);
            if (bounds.size == Vector3.zero) return;

            float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            if (maxSize < 0.0001f) return;

            float scale = targetSize / maxSize;
            model.transform.localScale = Vector3.one * scale;

            Debug.Log($"[ModelLoader] Auto scaled model: original size {maxSize:F4}m -> target {targetSize}m, scale factor {scale:F4}");
        }

        /// <summary>
        /// 将模型中心对齐到原点
        /// </summary>
        private void CenterModel(GameObject model)
        {
            var bounds = CalculateBounds(model);
            if (bounds.size == Vector3.zero) return;

            Vector3 offset = bounds.center;
            model.transform.localPosition = -offset;
        }

        /// <summary>
        /// 计算模型的包围盒
        /// </summary>
        private Bounds CalculateBounds(GameObject model)
        {
            var renderers = model.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds();

            var bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            return bounds;
        }

        /// <summary>
        /// 清除当前模型
        /// </summary>
        public void ClearCurrentModel()
        {
            if (currentModel)
            {
                // 如果有选中，先取消选中
                if (mouseHandler != null && mouseHandler.SelectedObject == currentModel)
                {
                    mouseHandler.Deselect();
                }

                Destroy(currentModel);
                currentModel = null;
            }
        }

        public void SetModelTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (!currentModel) return;
            currentModel.transform.localPosition = position;
            currentModel.transform.localRotation = rotation;
            currentModel.transform.localScale = scale;
        }

        public void SetModelPosition(Vector3 position)
        {
            if (!currentModel) return;
            currentModel.transform.localPosition = position;
        }

        public void SetModelRotation(Quaternion rotation)
        {
            if (!currentModel) return;
            currentModel.transform.localRotation = rotation;
        }

        public void SetModelScale(Vector3 scale)
        {
            if (!currentModel) return;
            currentModel.transform.localScale = scale;
        }

        /// <summary>
        /// 设置模型透明度
        /// </summary>
        public void SetModelOpacity(float opacity)
        {
            if (!currentModel) return;
            var renderer = currentModel.GetComponent<MeshRenderer>();
            if (!renderer) return;

            var material = renderer.material;

            if (opacity < 1f)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            else
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
            }

            var color = material.color;
            color.a = opacity;
            material.color = color;
        }
    }
}
