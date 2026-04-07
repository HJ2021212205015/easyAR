//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    class EnableScope : IDisposable
    {
        private bool m_Disposed;
        private bool context;

        public EnableScope()
        {
            context = GUI.enabled;
        }

        public EnableScope(bool enabled)
        {
            context = GUI.enabled;
            GUI.enabled = enabled;
        }

        ~EnableScope()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    CloseScope();
                }

                m_Disposed = true;
            }
        }

        private void CloseScope() => GUI.enabled = context;
    }

    class IndentScope : IDisposable
    {
        private bool m_Disposed;

        public IndentScope()
        {
            EditorGUI.indentLevel += 1;
        }

        ~IndentScope()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    CloseScope();
                }

                m_Disposed = true;
            }
        }

        private void CloseScope() => EditorGUI.indentLevel -= 1;
    }
}
