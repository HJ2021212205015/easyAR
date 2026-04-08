using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using ezAR.Model;

namespace ezAR.UI
{
    /// <summary>
    /// STL文件浏览器
    /// 提供运行时文件选择功能（PC端）
    /// </summary>
    public class StlFileBrowser : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ModelLoader modelLoader;
        [SerializeField] private Button openFileButton;
        [SerializeField] private Text statusText;

        [Header("Settings")]
        [SerializeField] private string defaultDirectory = "";

        private void Awake()
        {
            Debug.Log("[StlFileBrowser] Awake called");
        }

        private void Start()
        {
            Debug.Log("[StlFileBrowser] Start called");

            // 自动获取组件（如果未手动指定）
            if (openFileButton == null)
            {
                openFileButton = GetComponent<Button>();
                Debug.Log($"[StlFileBrowser] Auto-get Button: {(openFileButton != null ? "success" : "failed")}");
            }

            if (modelLoader == null)
            {
                modelLoader = FindObjectOfType<ModelLoader>();
                Debug.Log($"[StlFileBrowser] Auto-get ModelLoader: {(modelLoader != null ? "success" : "failed")}");
            }

            // 添加按钮点击事件
            if (openFileButton != null)
            {
                openFileButton.onClick.AddListener(OpenFileDialog);
                Debug.Log("[StlFileBrowser] Button click listener added");
            }
            else
            {
                Debug.LogError("[StlFileBrowser] openFileButton is null!");
            }

            SetStatus("点击按钮加载STL模型");
        }

        /// <summary>
        /// 打开文件选择对话框
        /// </summary>
        public void OpenFileDialog()
        {
            Debug.Log("[StlFileBrowser] OpenFileDialog called");

#if UNITY_EDITOR
            // 编辑器模式：使用Unity内置文件对话框
            string path = UnityEditor.EditorUtility.OpenFilePanel(
                "选择STL模型文件",
                string.IsNullOrEmpty(defaultDirectory) ? "" : defaultDirectory,
                "stl"
            );
            ProcessSelectedFile(path);
#elif UNITY_STANDALONE_WIN
            // Windows独立运行模式：使用Windows原生API
            string path = OpenWindowsFileDialog();
            ProcessSelectedFile(path);
#else
            // 其他平台：不支持
            SetStatus("文件选择对话框仅支持Windows平台");
#endif
        }

#if UNITY_STANDALONE_WIN
        // Windows API 调用
        [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OPENFILENAME ofn);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class OPENFILENAME
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int nMaxCustFilter;
            public int nFilterIndex;
            public string lpstrFile;
            public int nMaxFile;
            public string lpstrFileTitle;
            public int nMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int Flags;
            public short nFileOffset;
            public short nFileExtension;
            public string lpstrDefExt;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int FlagsEx;
        }

        private const int OFN_FILEMUSTEXIST = 0x00001000;
        private const int OFN_PATHMUSTEXIST = 0x00000800;

        private string OpenWindowsFileDialog()
        {
            var ofn = new OPENFILENAME();
            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.lpstrFilter = "STL文件\0*.stl\0所有文件\0*.*\0";
            ofn.lpstrFile = new string(new char[260]);
            ofn.nMaxFile = 260;
            ofn.lpstrFileTitle = new string(new char[260]);
            ofn.nMaxFileTitle = 260;
            ofn.lpstrTitle = "选择STL模型文件";
            ofn.Flags = OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST;

            if (!string.IsNullOrEmpty(defaultDirectory) && Directory.Exists(defaultDirectory))
            {
                ofn.lpstrInitialDir = defaultDirectory;
            }

            if (GetOpenFileName(ofn))
            {
                return ofn.lpstrFile.TrimEnd('\0');
            }

            return null;
        }
#endif

        /// <summary>
        /// 处理选中的文件
        /// </summary>
        private void ProcessSelectedFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!File.Exists(path))
            {
                SetStatus($"文件不存在: {path}");
                return;
            }

            string extension = Path.GetExtension(path).ToLower();
            if (extension != ".stl")
            {
                SetStatus($"不支持的文件格式: {extension}");
                return;
            }

            SetStatus($"正在加载: {Path.GetFileName(path)}");

            if (modelLoader != null)
            {
                modelLoader.LoadStlFromPath(path);
                SetStatus($"已加载: {Path.GetFileName(path)}");
            }
            else
            {
                SetStatus("ModelLoader未配置");
            }
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[StlFileBrowser] {message}");
        }

        /// <summary>
        /// 设置默认目录
        /// </summary>
        public void SetDefaultDirectory(string path)
        {
            defaultDirectory = path;
        }
    }
}
