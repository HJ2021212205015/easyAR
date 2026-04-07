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

namespace easyar
{
    internal class MessagePanel : MonoBehaviour
    {
        private Texture2D texture;
        private Texture2D textureFatal;
        private GUIStyle boxStyle;
        private readonly Queue<Tuple<DiagnosticsMessageType, string, float>> messageQueue = new Queue<Tuple<DiagnosticsMessageType, string, float>>();
        private bool isDisappearing;

        private void Start()
        {
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
            texture.Apply();
            textureFatal = new Texture2D(1, 1);
            textureFatal.SetPixel(0, 0, new Color(0.8f, 0, 0, 0.6f));
            textureFatal.Apply();
            boxStyle = new GUIStyle
            {
                wordWrap = true,
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };
            boxStyle.normal.textColor = Color.white;
            boxStyle.normal.background = texture;
            StartCoroutine(ShowMessage());
        }

        private void OnGUI()
        {
            if (messageQueue.Count <= 0) { return; }

            var item = messageQueue.Peek();
            Color color = Color.white;
            if (item.Item1 == DiagnosticsMessageType.Warning)
            {
                color = new Color32(255, 167, 51, 255);
            }
            else if (item.Item1 == DiagnosticsMessageType.Error)
            {
                color = Color.red;
            }
            color.a = boxStyle.normal.textColor.a;
            color.a += isDisappearing ? -Time.deltaTime * 2 : Time.deltaTime * 2;
            color.a = color.a > 1 ? 1 : (color.a < 0 ? 0 : color.a);
            boxStyle.normal.textColor = color;
            boxStyle.normal.background = item.Item1 == DiagnosticsMessageType.FatalError ? textureFatal : texture;
            GUI.Box(new Rect(0, Screen.height / 2, Screen.width, Math.Min(Screen.height / 4, item.Item1 == DiagnosticsMessageType.FatalError ? 512 : 160)), item.Item2, boxStyle);
        }

        private void OnDestroy()
        {
            if (texture) { Destroy(texture); }
            if (textureFatal) { Destroy(textureFatal); }
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

                var color = boxStyle.normal.textColor;
                color.a = 0;
                boxStyle.normal.textColor = color;
                isDisappearing = false;

                var time = messageQueue.Peek().Item3;
                yield return new WaitForSeconds(time > 1 ? time - 0.5f : time / 2);
                isDisappearing = true;
                yield return new WaitForSeconds(time > 1 ? 0.5f : time / 2);

                messageQueue.Dequeue();
            }
        }
    }
}
