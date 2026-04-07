//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Path type.</para>
    /// <para xml:lang="zh">路径类型。</para>
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// <para xml:lang="en">Absolute path.</para>
        /// <para xml:lang="zh">绝对路径。</para>
        /// </summary>
        Absolute,
        /// <summary>
        /// <para xml:lang="en">Unity StreamingAssets path.</para>
        /// <para xml:lang="zh">Unity StreamingAssets路径。</para>
        /// </summary>
        StreamingAssets,
    }

    /// <summary>
    /// <para xml:lang="en">Output file path type.</para>
    /// <para xml:lang="zh">文件输出路径类型。</para>
    /// </summary>
    public enum WritablePathType
    {
        /// <summary>
        /// <para xml:lang="en">Absolute path.</para>
        /// <para xml:lang="zh">绝对路径。</para>
        /// </summary>
        Absolute,
        /// <summary>
        /// <para xml:lang="en">Unity persistent data path.</para>
        /// <para xml:lang="zh">Unity沙盒路径。</para>
        /// </summary>
        PersistentDataPath,
    }

    internal static class FileUtil
    {
        public static IEnumerator LoadFile(string filePath, PathType filePathType, Action<Buffer> onLoad, Action<string> onError)
        {
            return LoadFile(filePath, filePathType, (data) =>
            {
                using (var buffer = Buffer.wrapByteArray(data))
                {
                    onLoad?.Invoke(buffer);
                }
            }, onError);
        }

        public static IEnumerator LoadFile(string filePath, PathType filePathType, Action<byte[]> onLoad, Action<string> onError)
        {
            if (onLoad == null)
            {
                yield break;
            }
            var path = filePath;
            if (filePathType == PathType.StreamingAssets)
            {
                path = Application.streamingAssetsPath + "/" + path;
            }
            using (var request = UnityWebRequest.Get(PathToUrl(path)))
            {
                yield return request.SendWebRequest();
                var error = $"fail to load file {filePath} of type {filePathType}";
                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError($"{error}: {request.error}");
                    onError?.Invoke($"{error}: {request.error}");
                    yield break;
                }
                while (!request.isDone)
                {
                    yield return 0;
                }
                if (request.downloadHandler.data == null)
                {
                    Debug.LogError($"{error}: data is null");
                    onError?.Invoke($"{error}: data is null");
                    yield break;
                }
                onLoad(request.downloadHandler.data);
            }
        }

        public static string PathToUrl(string path)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith("jar:file://") || path.StartsWith("file://") || path.StartsWith("http://") || path.StartsWith("https://"))
            {
                return path;
            }
            if (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android ||
                SystemUtil.IsVisionOS())
            {
                path = "file://" + path;
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = "file:///" + path;
            }
            return path;
        }
    }
}
