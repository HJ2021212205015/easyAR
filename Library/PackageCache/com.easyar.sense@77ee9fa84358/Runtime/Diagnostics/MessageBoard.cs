//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_VISIONOS
using TMPro;
#endif

namespace easyar
{
    internal class MessageBoard : MonoBehaviour
    {
        private GameObject textObject;
        private GameObject cubeObject;
#if UNITY_VISIONOS
        private TextMeshPro textMesh;
#else
        private TextMesh textMesh;
#endif
        private GameObject origin;
        private readonly Queue<Tuple<DiagnosticsMessageType, string, float>> messageQueue = new Queue<Tuple<DiagnosticsMessageType, string, float>>();

        private void Start()
        {
            textObject = new GameObject();
#if UNITY_VISIONOS
            textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.fontSize = 14;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.color = Color.white;
            textMesh.textWrappingMode = TextWrappingModes.NoWrap;
#else
            var mesh = textObject.AddComponent<MeshRenderer>();
            textMesh = textObject.AddComponent<TextMesh>();
            textMesh.fontSize = 14;
            textMesh.anchor = TextAnchor.MiddleCenter;
            mesh.material.color = textMesh.color;
#endif
            textObject.SetActive(false);

            cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (SystemUtil.RenderPipeline == SystemUtil.RenderPipelineType.URP)
            {
                cubeObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            }
            cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0, 0, 0.5f);
            cubeObject.SetActive(false);

            StartCoroutine(ShowMessage());
        }

        private void OnDestroy()
        {
            if (textObject) { Destroy(textObject); }
            if (cubeObject) { Destroy(cubeObject); }
        }

        public void Setup(GameObject go)
        {
            origin = go;
        }

        public void Enqueue(DiagnosticsMessageType type, string message, float seconds)
        {
            if (type != DiagnosticsMessageType.Error && type != DiagnosticsMessageType.FatalError && type != DiagnosticsMessageType.Warning && type != DiagnosticsMessageType.Info)
            {
                throw new InvalidOperationException("invalid type");
            }
            messageQueue.Enqueue(Tuple.Create(type, message, seconds));
        }

        public void DequeueAll()
        {
            while (messageQueue.Count > 0)
            {
                messageQueue.Dequeue();
            }
            StopAllCoroutines();
            HideMessage();
            StartCoroutine(ShowMessage());
        }

        private IEnumerator ShowMessage()
        {
            while (true)
            {
                if (messageQueue.Count <= 0)
                {
                    yield return null;
                    continue;
                }

                var item = messageQueue.Peek();
                ShowMessage(item.Item1, item.Item2);
                yield return new WaitForSeconds(item.Item3);
                HideMessage();
                messageQueue.Dequeue();
            }
        }

        private void ShowMessage(DiagnosticsMessageType type, string message)
        {
            textMesh.text = message;
            textMesh.color = Color.white;
            if (type == DiagnosticsMessageType.Warning)
            {
                textMesh.color = new Color32(255, 167, 51, 255);
            }
            else if (type == DiagnosticsMessageType.Error)
            {
                textMesh.color = Color.red;
            }

            textObject.transform.parent = transform;
            textObject.transform.localPosition = new Vector3(0, -0.8f, 5);
            textObject.transform.localRotation = Quaternion.identity;
            textObject.transform.localScale = new Vector3(0.075f, 0.075f, 1);
            textObject.transform.parent = origin ? origin.transform : null;
            textObject.SetActive(true);
            if (type == DiagnosticsMessageType.FatalError)
            {
                cubeObject.transform.parent = transform;
                cubeObject.transform.localPosition = new Vector3(0, -0.8f, 5.1f);
                cubeObject.transform.localRotation = Quaternion.identity;
                cubeObject.transform.localScale = new Vector3(10, 10, 0.1f);
                cubeObject.transform.parent = origin ? origin.transform : null;
                cubeObject.SetActive(true);
            }
        }

        private void HideMessage()
        {
            textObject.SetActive(false);
            cubeObject.SetActive(false);
        }
    }
}
