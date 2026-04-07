//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    internal class CautionPanel : MonoBehaviour
    {
        private string messages = string.Empty;
        private Texture2D texture;
        private GUIStyle boxStyle;

        private void Start()
        {
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
            texture.Apply();
            boxStyle = new GUIStyle
            {
                wordWrap = true,
                fontSize = 20,
                alignment = TextAnchor.UpperLeft
            };
            boxStyle.normal.textColor = new Color32(255, 167, 51, 255);
            boxStyle.normal.background = texture;
        }

        private void OnGUI()
        {
            if (string.IsNullOrEmpty(messages)) { return; }

            GUI.Box(new Rect(Screen.width / 20, Math.Max((int)(Screen.height * 0.7), 20), Screen.width - Screen.width / 10, Math.Min(Screen.height / 4, 200)), messages, boxStyle);
        }

        private void OnDestroy()
        {
            if (texture) { Destroy(texture); }
        }

        public void Show(string message)
        {
            if (string.IsNullOrEmpty(messages))
            {
                messages = "WARNING FOR DEVELOPERS / 开发人员请关注";
            }
            messages += Environment.NewLine + Environment.NewLine + message;
        }

        public void Hide()
        {
            messages = string.Empty;
        }
    }
}
