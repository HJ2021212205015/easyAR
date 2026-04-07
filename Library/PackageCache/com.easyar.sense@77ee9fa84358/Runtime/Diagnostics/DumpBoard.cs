//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;
#if UNITY_VISIONOS
using TMPro;
#endif

namespace easyar
{
    internal class DumpBoard : MonoBehaviour
    {
        private GameObject textObject;
#if UNITY_VISIONOS
        private TextMeshPro textMesh;
#else
        private TextMesh textMesh;
#endif

        private void Awake()
        {
            textObject = new GameObject();
            textObject.transform.parent = transform;
            textObject.transform.localPosition = new Vector3(-1.5f, 1, 5);
            textObject.transform.localRotation = Quaternion.identity;
            textObject.transform.localScale = new Vector3(0.075f, 0.075f, 1);
#if UNITY_VISIONOS
            textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.fontSize = 14;
            textMesh.color = new Color32(255, 244, 42, 255);
            textMesh.alignment = TextAlignmentOptions.TopLeft;
            textMesh.textWrappingMode = TextWrappingModes.NoWrap;
#else
            var mesh = textObject.AddComponent<MeshRenderer>();
            textMesh = textObject.AddComponent<TextMesh>();
            textMesh.fontSize = 14;
            textMesh.color = new Color32(255, 244, 42, 255);
            textMesh.anchor = TextAnchor.UpperLeft;
            mesh.material.color = textMesh.color;
#endif
        }

        private void OnDestroy()
        {
            if (textObject) { Destroy(textObject); }
        }

        public void Show(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                textObject.SetActive(false);
                return;
            }

            textMesh.text = message;
            textObject.SetActive(true);
        }
    }
}
