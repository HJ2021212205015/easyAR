using UnityEngine;

namespace ezAR.Model
{
    public class ModelLoader : MonoBehaviour
    {
        [SerializeField] private Transform contentRoot;
        [SerializeField] private Material defaultMaterial;

        private GameObject currentModel;

        public GameObject CurrentModel => currentModel;
        public Transform ContentRoot => contentRoot;

        public void LoadStlFromPath(string filePath)
        {
            var mesh = StlImporter.ImportFromBinaryFile(filePath);
            if (mesh == null)
            {
                Debug.LogError($"[ModelLoader] Failed to load STL: {filePath}");
                return;
            }

            ClearCurrentModel();

            var go = new GameObject("LoadedModel_STL");
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();
            var collider = go.AddComponent<MeshCollider>();

            filter.mesh = mesh;
            collider.sharedMesh = mesh;

            if (defaultMaterial)
            {
                renderer.material = defaultMaterial;
            }
            else
            {
                renderer.material = new Material(Shader.Find("Standard"));
            }

            if (contentRoot)
            {
                go.transform.SetParent(contentRoot, false);
            }

            currentModel = go;
            Debug.Log($"[ModelLoader] Loaded STL model from: {filePath}");
        }

        public void ClearCurrentModel()
        {
            if (currentModel)
            {
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

        public void SetModelOpacity(float opacity)
        {
            if (!currentModel) return;
            var renderer = currentModel.GetComponent<MeshRenderer>();
            if (!renderer) return;

            var material = renderer.material;
            var mode = material.GetInt("_Mode");

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
