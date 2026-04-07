//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;
#if UNITY_VISIONOS
using TMPro;
#endif

namespace easyar
{
    internal class CautionBoard : MonoBehaviour
    {
        private string messages = string.Empty;
        private GameObject textObject;
#if UNITY_VISIONOS
        private TextMeshPro textMesh;
#else
        private TextMesh textMesh;
#endif
        private GameObject origin;

        private void Awake()
        {
            textObject = new GameObject();
#if UNITY_VISIONOS
            textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.fontSize = 14;
            textMesh.color = new Color32(255, 167, 51, 255);
            textMesh.alignment = TextAlignmentOptions.Left;
            textMesh.textWrappingMode = TextWrappingModes.NoWrap;
#else
            var mesh = textObject.AddComponent<MeshRenderer>();
            textMesh = textObject.AddComponent<TextMesh>();
            textMesh.fontSize = 14;
            textMesh.color = new Color32(255, 167, 51, 255);
            textMesh.anchor = TextAnchor.MiddleLeft;
            mesh.material.color = textMesh.color;
#endif
            textObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (textObject) { Destroy(textObject); }
        }

        public void Setup(GameObject go)
        {
            origin = go;
        }

        public void Show(string message)
        {
            if (string.IsNullOrEmpty(messages))
            {
                messages = "WARNING FOR DEVELOPERS / 开发人员请关注";
            }
            messages += Environment.NewLine + Environment.NewLine + message;

            textMesh.text = messages;
            textObject.transform.parent = transform;
            textObject.transform.localPosition = new Vector3(0, -0.8f, 5);
            textObject.transform.localRotation = Quaternion.identity;
            textObject.transform.localScale = new Vector3(0.075f, 0.075f, 1);
            textObject.transform.parent = origin ? origin.transform : null;
            textObject.SetActive(true);
        }

        public void Hide()
        {
            messages = string.Empty;
            textObject.SetActive(false);
        }
    }
}
