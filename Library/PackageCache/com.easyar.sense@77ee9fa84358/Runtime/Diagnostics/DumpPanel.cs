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
    internal class DumpPanel : MonoBehaviour
    {
        private string messages = string.Empty;
        private GUIStyle boxStyle;

        private void Start()
        {
            boxStyle = new GUIStyle
            {
                wordWrap = true,
                fontSize = 22,
                alignment = TextAnchor.UpperLeft
            };
            boxStyle.normal.textColor = new Color32(255, 244, 42, 255);
        }

        private void OnGUI()
        {
            if (string.IsNullOrEmpty(messages)) { return; }

            GUI.Box(new Rect(Screen.width / 20, Math.Max(Screen.height / 20, 20), Screen.width - Screen.width / 10, Math.Min(Screen.height / 4, 200)), messages, boxStyle);
        }

        public void Show(string message)
        {
            messages = message;
        }
    }
}
