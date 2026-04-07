using easyar;
using UnityEngine;

namespace ezAR.Tracking
{
    public class ImageTargetContentAnchor : MonoBehaviour
    {
        [SerializeField] private Transform contentRoot;
        [SerializeField] private Vector3 localOffset = new Vector3(0f, 0.03f, 0f);
        [SerializeField] private bool hideContentWhenTrackingLost = true;

        private ImageTargetController imageTarget;

        public Transform ContentRoot => contentRoot;
        public Vector3 LocalOffset => localOffset;

        private void Awake()
        {
            imageTarget = GetComponent<ImageTargetController>();

            if (!contentRoot && transform.childCount > 0)
            {
                contentRoot = transform.GetChild(0);
            }

            ApplyOffset();
        }

        private void OnEnable()
        {
            if (!imageTarget)
            {
                imageTarget = GetComponent<ImageTargetController>();
            }

            if (!imageTarget)
            {
                return;
            }

            imageTarget.TargetFound += HandleTargetFound;
            imageTarget.TargetLost += HandleTargetLost;
        }

        private void OnDisable()
        {
            if (!imageTarget)
            {
                return;
            }

            imageTarget.TargetFound -= HandleTargetFound;
            imageTarget.TargetLost -= HandleTargetLost;
        }

        public void SetOffset(Vector3 offset)
        {
            localOffset = offset;
            ApplyOffset();
        }

        public void SetOffset(float x, float y, float z)
        {
            SetOffset(new Vector3(x, y, z));
        }

        private void HandleTargetFound()
        {
            ApplyOffset();
            SetContentVisible(true);
            Debug.Log($"[ezAR] Target found: {imageTarget.Target?.name()}");
        }

        private void HandleTargetLost()
        {
            if (hideContentWhenTrackingLost)
            {
                SetContentVisible(false);
            }

            Debug.Log($"[ezAR] Target lost: {imageTarget.Target?.name()}");
        }

        private void ApplyOffset()
        {
            if (!contentRoot)
            {
                return;
            }

            contentRoot.localPosition = localOffset;
            contentRoot.localRotation = Quaternion.identity;
            contentRoot.localScale = Vector3.one;
        }

        private void SetContentVisible(bool visible)
        {
            if (!contentRoot)
            {
                return;
            }

            contentRoot.gameObject.SetActive(visible);
        }
    }
}
