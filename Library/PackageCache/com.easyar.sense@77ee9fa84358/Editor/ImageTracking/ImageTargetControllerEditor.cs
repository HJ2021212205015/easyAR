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
    [CustomEditor(typeof(ImageTargetController), true)]
    class ImageTargetControllerEditor : Editor
    {
        public void OnEnable()
        {
            var controller = (ImageTargetController)target;
            UpdateScale(controller, controller.GizmoData.Scale);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controller = (ImageTargetController)target;

            var sourceType = serializedObject.FindProperty("SourceType");
            EditorGUILayout.PropertyField(sourceType, new GUIContent("Source"), true);
            serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel += 1;
            switch ((ImageTargetController.DataSource)sourceType.enumValueIndex)
            {
                case ImageTargetController.DataSource.ImageFile:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageFileSource.PathType"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageFileSource.Path"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageFileSource.Name"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageFileSource.Scale"), true);
                    break;
                case ImageTargetController.DataSource.TargetDataFile:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetDataFileSource.PathType"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetDataFileSource.Path"), true);
                    break;
                case ImageTargetController.DataSource.Texture2D:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Texture2DSource.Texture"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Texture2DSource.Name"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Texture2DSource.Scale"), true);
                    break;
                default:
                    break;
            }
            EditorGUI.indentLevel -= 1;

            var tracker = serializedObject.FindProperty("tracker");
            EditorGUILayout.PropertyField(tracker, new GUIContent("Tracker"), true);

            var trackerHasSet = serializedObject.FindProperty("trackerHasSet");
            if (!trackerHasSet.boolValue)
            {
                if (!tracker.objectReferenceValue)
                {
                    tracker.objectReferenceValue = ObjectUtil.FindFirstObjectByType<ImageTrackerFrameFilter>();
                }
                if (tracker.objectReferenceValue)
                {
                    trackerHasSet.boolValue = true;
                }
            }
            var texture = Optional<Texture2D>.Empty;
            if (controller.Source is ImageTargetController.Texture2DSourceData texture2DSourceData)
            {
                texture = texture2DSourceData.Texture;
            }
            serializedObject.ApplyModifiedProperties();
            if (controller.Source is ImageTargetController.Texture2DSourceData texture2DSource && texture2DSource.Texture != texture)
            {
                texture2DSource.Name = texture2DSource.Texture ? texture2DSource.Texture.name : string.Empty;
                serializedObject.ApplyModifiedProperties();
            }
            controller.Tracker = (ImageTrackerFrameFilter)tracker.objectReferenceValue;

            if (Event.current.type == EventType.Used)
            {
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    var objg = obj as GameObject;
                    if (objg && objg.GetComponent<ImageTrackerFrameFilter>() && !AssetDatabase.GetAssetPath(obj).Equals(""))
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
            }

            CheckScale();
        }

        void CheckScale()
        {
            if (Application.isPlaying)
            {
                return;
            }
            var controller = (ImageTargetController)target;
            if (controller.Source is ImageTargetController.ImageFileSourceData imageFileSource)
            {
                if (controller.GizmoData.Scale != imageFileSource.Scale)
                {
                    UpdateScale(controller, imageFileSource.Scale);
                }
                else if (controller.GizmoData.ScaleX != controller.transform.localScale.x)
                {
                    imageFileSource.Scale = Math.Abs(controller.transform.localScale.x);
                    UpdateScale(controller, imageFileSource.Scale);
                }
                else if (controller.GizmoData.Scale != controller.transform.localScale.y)
                {
                    imageFileSource.Scale = Math.Abs(controller.transform.localScale.y);
                    UpdateScale(controller, imageFileSource.Scale);
                }
                else if (controller.GizmoData.Scale != controller.transform.localScale.z)
                {
                    imageFileSource.Scale = Math.Abs(controller.transform.localScale.z);
                    UpdateScale(controller, imageFileSource.Scale);
                }
                else if (controller.GizmoData.HorizontalFlip != controller.HorizontalFlip)
                {
                    UpdateScale(controller, imageFileSource.Scale);
                }
            }
            if (controller.Source is ImageTargetController.Texture2DSourceData texture2DSource)
            {
                if (controller.GizmoData.Scale != texture2DSource.Scale)
                {
                    UpdateScale(controller, texture2DSource.Scale);
                }
                else if (controller.GizmoData.ScaleX != controller.transform.localScale.x)
                {
                    texture2DSource.Scale = Math.Abs(controller.transform.localScale.x);
                    UpdateScale(controller, texture2DSource.Scale);
                }
                else if (controller.GizmoData.Scale != controller.transform.localScale.y)
                {
                    texture2DSource.Scale = Math.Abs(controller.transform.localScale.y);
                    UpdateScale(controller, texture2DSource.Scale);
                }
                else if (controller.GizmoData.Scale != controller.transform.localScale.z)
                {
                    texture2DSource.Scale = Math.Abs(controller.transform.localScale.z);
                    UpdateScale(controller, texture2DSource.Scale);
                }
                else if (controller.GizmoData.HorizontalFlip != controller.HorizontalFlip)
                {
                    UpdateScale(controller, texture2DSource.Scale);
                }
            }
            else
            {
                if (controller.GizmoData.HorizontalFlip != controller.HorizontalFlip || controller.GizmoData.ScaleX != controller.transform.localScale.x || controller.GizmoData.Scale != controller.transform.localScale.y || controller.GizmoData.Scale != controller.transform.localScale.z)
                {
                    UpdateScale(controller, controller.GizmoData.Scale);
                }
            }
        }

        static private void UpdateScale(ImageTargetController controller, float s)
        {
            if (Application.isPlaying)
            {
                return;
            }
            var vec3Unit = Vector3.one;
            if (controller.HorizontalFlip)
            {
                vec3Unit.x = -vec3Unit.x;
            }
            controller.transform.localScale = vec3Unit * s;

            controller.GizmoData.Scale = s;
            controller.GizmoData.ScaleX = controller.transform.localScale.x;
            controller.GizmoData.HorizontalFlip = controller.HorizontalFlip;
        }

        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy)]
        static void DrawGizmo(ImageTargetController scr, GizmoType gizmoType)
        {
            var source = scr.LoadingSource.ValueOrDefault(scr.Source);
            var signature = scr.Source?.GetType().ToString() ?? string.Empty;

            if (source is ImageTargetController.ImageFileSourceData imageFileSourceData)
            {
                if (EasyARSettings.Instance && !EasyARSettings.Instance.GizmoConfig.ImageTarget.EnableImageFile) { return; }
                signature += imageFileSourceData.PathType.ToString() + imageFileSourceData.Path;
            }
            else if (source is ImageTargetController.TargetDataFileSourceData targetDataFileSourceData)
            {
                if (EasyARSettings.Instance && !EasyARSettings.Instance.GizmoConfig.ImageTarget.EnableTargetDataFile) { return; }
                signature += targetDataFileSourceData.PathType.ToString() + targetDataFileSourceData.Path;
            }
            else if (source is ImageTargetController.TargetSourceData targetSourceData)
            {
                if (EasyARSettings.Instance && !EasyARSettings.Instance.GizmoConfig.ImageTarget.EnableTarget) { return; }
                signature += scr.Target?.runtimeID().ToString();
            }
            else if (source is ImageTargetController.Texture2DSourceData texture2DSourceData)
            {
                if (EasyARSettings.Instance && !EasyARSettings.Instance.GizmoConfig.ImageTarget.EnableTexture2D) { return; }
                signature += texture2DSourceData.Texture ? texture2DSourceData.Texture.GetInstanceID() + (texture2DSourceData.IsTextureLoadable ? string.Empty : "_invalid") : (scr.Target != null ? scr.Target?.runtimeID().ToString() : "null");
            }

            if (scr.GizmoData.Material == null)
            {
                scr.GizmoData.Material = new Material(Shader.Find("EasyAR/ImageTargetGizmo"));
            }
            if (scr.GizmoData.Signature != signature)
            {
                if (scr.GizmoData.Texture != null)
                {
                    UnityEngine.Object.DestroyImmediate(scr.GizmoData.Texture);
                    scr.GizmoData.Texture = null;
                }

                string path;
                if (source is ImageTargetController.ImageFileSourceData imageFileSource)
                {
                    path = imageFileSource.Path;
                    if (imageFileSource.PathType == PathType.StreamingAssets)
                    {
                        path = Application.streamingAssetsPath + "/" + imageFileSource.Path;
                    }
                    if (System.IO.File.Exists(path))
                    {
                        var sourceData = System.IO.File.ReadAllBytes(path);
                        scr.GizmoData.Texture = new Texture2D(2, 2);
                        scr.GizmoData.Texture.LoadImage(sourceData);
                        scr.GizmoData.Texture.Apply();
                        UpdateScale(scr, imageFileSource.Scale);
                        if (SceneView.lastActiveSceneView)
                        {
                            SceneView.lastActiveSceneView.Repaint();
                        }
                    }
                }
                else if (source is ImageTargetController.TargetDataFileSourceData targetDataFileSource)
                {
                    path = targetDataFileSource.Path;
                    if (targetDataFileSource.PathType == PathType.StreamingAssets)
                    {
                        path = Application.streamingAssetsPath + "/" + targetDataFileSource.Path;
                    }
                    if (System.IO.File.Exists(path))
                    {
                        if (!EasyARController.IsReady)
                        {
                            EasyARController.Initialize();
                            if (!EasyARController.IsReady)
                            {
                                Debug.LogWarning("EasyAR Sense target data gizmo enabled but license key validation failed, target data gizmo will not show");
                            }
                        }
                        var sourceData = System.IO.File.ReadAllBytes(path);

                        using (Buffer buffer = Buffer.wrapByteArray(sourceData))
                        {
                            var targetOptional = ImageTarget.createFromTargetData(buffer);
                            if (targetOptional.OnSome)
                            {
                                using (ImageTarget target = targetOptional.Value)
                                {
                                    var imageList = target.images();
                                    if (imageList.Count > 0)
                                    {
                                        var image = imageList[0];
                                        scr.GizmoData.Texture = new Texture2D(image.width(), image.height(), TextureFormat.R8, false);
                                        scr.GizmoData.Texture.LoadRawTextureData(image.buffer().data(), image.buffer().size());
                                        scr.GizmoData.Texture.Apply();
                                    }
                                    foreach (var image in imageList)
                                    {
                                        image.Dispose();
                                    }
                                    UpdateScale(scr, target.scale());
                                    if (SceneView.lastActiveSceneView)
                                    {
                                        SceneView.lastActiveSceneView.Repaint();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (source is ImageTargetController.Texture2DSourceData texture2DSource)
                {
                    if (texture2DSource.IsTextureLoadable)
                    {
                        var texture = texture2DSource.Texture;
                        scr.GizmoData.Texture = new Texture2D(texture.width, texture.height, texture.format, false);
                        scr.GizmoData.Texture.LoadRawTextureData(texture.GetRawTextureData());
                        scr.GizmoData.Texture.Apply();
                        UpdateScale(scr, texture2DSource.Scale);
                        if (SceneView.lastActiveSceneView)
                        {
                            SceneView.lastActiveSceneView.Repaint();
                        }
                    }
                    else
                    {
                        if (texture2DSource.Texture)
                        {
                            Debug.LogError(texture2DSource.TextureUnloadableReason);
                        }
                    }
                }

                if (!scr.GizmoData.Texture && scr.Target != null)
                {
                    var imageList = (scr.Target as ImageTarget).images();
                    if (imageList.Count > 0)
                    {
                        var image = imageList[0];
                        scr.GizmoData.Texture = new Texture2D(image.width(), image.height(), TextureFormat.R8, false);
                        scr.GizmoData.Texture.LoadRawTextureData(image.buffer().data(), image.buffer().size());
                        scr.GizmoData.Texture.Apply();
                    }
                    foreach (var image in imageList)
                    {
                        image.Dispose();
                    }
                    UpdateScale(scr, (scr.Target as ImageTarget).scale());
                    if (SceneView.lastActiveSceneView)
                    {
                        SceneView.lastActiveSceneView.Repaint();
                    }
                }

                if (scr.GizmoData.Texture == null)
                {
                    scr.GizmoData.Texture = new Texture2D(2, 2);
                    scr.GizmoData.Texture.LoadImage(new byte[0]);
                    scr.GizmoData.Texture.Apply();
                }
                scr.GizmoData.Signature = signature;
            }

            if (scr.GizmoData.Material && scr.GizmoData.Texture)
            {
                scr.GizmoData.Material.SetMatrix("_Transform", scr.transform.localToWorldMatrix);
                if (scr.GizmoData.Texture.format == TextureFormat.R8)
                {
                    scr.GizmoData.Material.SetInt("_isRenderGrayTexture", 1);
                }
                else
                {
                    scr.GizmoData.Material.SetInt("_isRenderGrayTexture", 0);
                }
                scr.GizmoData.Material.SetFloat("_Ratio", (float)scr.GizmoData.Texture.height / scr.GizmoData.Texture.width);
                Gizmos.DrawGUITexture(new Rect(0, 0, 1, 1), scr.GizmoData.Texture, scr.GizmoData.Material);
            }
        }
    }
}
