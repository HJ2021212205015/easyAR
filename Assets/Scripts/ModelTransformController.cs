using UnityEngine;

namespace ezAR.Model
{
    public class ModelTransformController : MonoBehaviour
    {
        [SerializeField] private ModelLoader modelLoader;

        [Header("Transform Settings")]
        [SerializeField] private float moveSpeed = 0.01f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private float scaleSpeed = 0.1f;
        [SerializeField] private float minScale = 0.01f;
        [SerializeField] private float maxScale = 10f;

        public void MoveModel(Vector3 delta)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            modelLoader.CurrentModel.transform.localPosition += delta * moveSpeed;
        }

        public void MoveModelX(float delta)
        {
            MoveModel(new Vector3(delta, 0, 0));
        }

        public void MoveModelY(float delta)
        {
            MoveModel(new Vector3(0, delta, 0));
        }

        public void MoveModelZ(float delta)
        {
            MoveModel(new Vector3(0, 0, delta));
        }

        public void RotateModel(Vector3 eulerDelta)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            var currentRotation = modelLoader.CurrentModel.transform.localEulerAngles;
            var newRotation = currentRotation + eulerDelta * rotateSpeed;
            modelLoader.CurrentModel.transform.localEulerAngles = newRotation;
        }

        public void RotateModelX(float delta)
        {
            RotateModel(new Vector3(delta, 0, 0));
        }

        public void RotateModelY(float delta)
        {
            RotateModel(new Vector3(0, delta, 0));
        }

        public void RotateModelZ(float delta)
        {
            RotateModel(new Vector3(0, 0, delta));
        }

        public void ScaleModel(float delta)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            var currentScale = modelLoader.CurrentModel.transform.localScale;
            var newScale = currentScale.x + delta * scaleSpeed;
            newScale = Mathf.Clamp(newScale, minScale, maxScale);
            modelLoader.CurrentModel.transform.localScale = Vector3.one * newScale;
        }

        public void ScaleModelUp()
        {
            ScaleModel(1f);
        }

        public void ScaleModelDown()
        {
            ScaleModel(-1f);
        }

        public void ResetTransform()
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            modelLoader.CurrentModel.transform.localPosition = Vector3.zero;
            modelLoader.CurrentModel.transform.localRotation = Quaternion.identity;
            modelLoader.CurrentModel.transform.localScale = Vector3.one;
        }

        public void SetPosition(Vector3 position)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            modelLoader.SetModelPosition(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            modelLoader.SetModelRotation(rotation);
        }

        public void SetScale(float uniformScale)
        {
            if (!modelLoader || !modelLoader.CurrentModel) return;
            var clampedScale = Mathf.Clamp(uniformScale, minScale, maxScale);
            modelLoader.SetModelScale(Vector3.one * clampedScale);
        }
    }
}
