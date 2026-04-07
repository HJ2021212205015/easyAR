//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace easyar
{
    [CustomEditor(typeof(MegaTrackerFrameFilter), true)]
    class MegaTrackerFrameFilterEditor : Editor
    {
        Queue<Tuple<DateTime, string, bool>> logs = new();
        string spotid;
        string token;
        private bool showAdvanced;

        public override void OnInspectorGUI()
        {
#if !EASYAR_ENABLE_MEGA
            EditorGUILayout.HelpBox($"Package com.easyar.mega is required to use {nameof(MegaTrackerFrameFilter)}", MessageType.Error);
#else
            DrawDefaultInspector();

            var filter = (MegaTrackerFrameFilter)target;

            EditorGUILayout.LabelField("Service");
            {
                EditorGUI.indentLevel += 1;

                var serviceType = (MegaApiType)EditorGUILayout.EnumPopup("Type", filter.ServiceType);
                if (filter.ServiceType != serviceType)
                {
                    filter.ServiceType = serviceType;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(filter);
                }

                var accessSource = (MegaServiceAccessSourceType)EditorGUILayout.EnumPopup("Access Source", filter.ServiceAccessSource);
                if (filter.ServiceAccessSource != accessSource)
                {
                    filter.ServiceAccessSource = accessSource;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(filter);
                }

                if (accessSource == MegaServiceAccessSourceType.APIKey)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.AppID"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.ServerAddress"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.APIKey"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyAccessData.APISecret"), true);
                    EditorGUI.indentLevel -= 1;
                }
                else if (accessSource == MegaServiceAccessSourceType.Token)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.AppID"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.ServerAddress"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("tokenAccessData.Token"), true);
                    EditorGUI.indentLevel -= 1;
                }

                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("requestTimeParameters"), true);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requestTimeParameters.Timeout"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requestTimeParameters.RequestInterval"), true);
            EditorGUI.indentLevel -= 1;

            var locationInputMode = (MegaLocationInputMode)EditorGUILayout.EnumPopup(new GUIContent("Location Input Mode", $"Must set to {MegaLocationInputMode.Simulator} when debug remotely or running on PC otherwize Mega will fail to work. Set to {MegaLocationInputMode.Onsite} when running on site to reach the best performance.\n远程调试或电脑上运行必须设置成 {MegaLocationInputMode.Simulator}，否则将无法使用。现场使用要设置成 {MegaLocationInputMode.Onsite} 以达到最佳效果。"), filter.LocationInputMode, (mode) => { return (MegaLocationInputMode)mode != MegaLocationInputMode.FramePlayer; }, false);
            if (filter.LocationInputMode != locationInputMode)
            {
                filter.LocationInputMode = locationInputMode;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(filter);
            }

            var minInputFrameLevel = (MegaInputFrameLevel)EditorGUILayout.EnumPopup("Min InputFrame Level", filter.MinInputFrameLevel);
            if (filter.MinInputFrameLevel != minInputFrameLevel)
            {
                filter.MinInputFrameLevel = minInputFrameLevel;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(filter);
            }
            showAdvanced = EditorGUILayout.Toggle("Advanced Options", showAdvanced);
            if (showAdvanced)
            {
                EditorGUI.indentLevel += 1;

                var curPrior = filter.BlockPrior;
                var prior = new MegaBlockPrior(new(), default);
                var hasPrior = EditorGUILayout.Toggle("Block Prior", curPrior.OnSome);
                if (hasPrior)
                {
                    EditorGUI.indentLevel += 1;

                    var modeProperty = serializedObject.FindProperty("blockPrior.Mode");
                    EditorGUILayout.PropertyField(modeProperty, true);
                    var blocksProperty = serializedObject.FindProperty("blockPrior.Blocks");
                    EditorGUILayout.PropertyField(blocksProperty, true);

                    prior.Mode = (BlockPriorMode)modeProperty.enumValueIndex;
                    for (int i = 0; i < blocksProperty.arraySize; i++)
                    {
                        SerializedProperty element = blocksProperty.GetArrayElementAtIndex(i);
                        string value = element.stringValue;
                        prior.Blocks.Add(element.stringValue);
                    }

                    EditorGUI.indentLevel -= 1;
                }
                if (hasPrior != curPrior.OnSome || (hasPrior && (prior.Mode != curPrior.Value.Mode || curPrior.Value.Blocks == null || !prior.Blocks.SequenceEqual(curPrior.Value.Blocks))))
                {
                    filter.BlockPrior = hasPrior ? prior : Optional<MegaBlockPrior>.Empty;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(filter);
                }
                EditorGUI.indentLevel -= 1;
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            using (_ = new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUILayout.Label("Test Area", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
                if (!Application.isPlaying)
                {
                    EditorGUILayout.HelpBox("Available in play mode", MessageType.Info);
                }

                if (!Application.isPlaying) { GUI.enabled = false; }

                if (filter.LocationInputMode == MegaLocationInputMode.Simulator)
                {
                    var locationOld = filter.SimulatorLocation;
                    var hasLocation = EditorGUILayout.Toggle("Simulator Location", locationOld.OnSome);

                    EditorGUI.indentLevel += 1;
                    double latitude = 0, longitude = 0;
                    if (hasLocation)
                    {
                        latitude = EditorGUILayout.DoubleField("Latitude", locationOld.OnSome ? locationOld.Value.latitude : 0);
                        longitude = EditorGUILayout.DoubleField("Longitude", locationOld.OnSome ? locationOld.Value.longitude : 0);
                    }
                    if (locationOld.OnSome != hasLocation || (locationOld.OnSome && locationOld.Value.latitude != latitude) || (locationOld.OnSome && locationOld.Value.longitude != longitude))
                    {
                        filter.SimulatorLocation = hasLocation ? new EasyAR.Mega.Scene.Location
                        {
                            latitude = latitude,
                            longitude = longitude,
                        } : Optional<EasyAR.Mega.Scene.Location>.Empty;
                    }
                    EditorGUI.indentLevel -= 1;
                }

                if (filter.ServiceType == MegaApiType.Landmark)
                {
                    spotid = EditorGUILayout.TextField("Spot ID", spotid);
                    using (_ = new GUILayout.HorizontalScope())
                    {
                        var guiEnabled = GUI.enabled;
                        if (filter.LandmarkFilter == null) { GUI.enabled = false; }
                        if (GUILayout.Button("Filter By Location", GUILayout.Height(30)))
                        {
                            filter.LandmarkFilter.Value.FilterByLocation((response) =>
                            {
                                if (!this) { return; }

                                var str = "[" + DateTime.Now.ToString("HH:mm:ss") + "] Filter: " + response.Status + (response.SpotVersionId.OnSome ? " Spot Version " + response.SpotVersionId : string.Empty); ;
                                if (response.ErrorMessage.OnSome)
                                {
                                    str += Environment.NewLine + response.ErrorMessage;
                                }
                                logs.Enqueue(Tuple.Create(DateTime.Now, str, response.Status == MegaLandmarkFilterStatus.Found || response.Status == MegaLandmarkFilterStatus.NotFound));
                                Repaint();
                            });
                        }

                        if (GUILayout.Button("Filter By Spot ID", GUILayout.Height(30)))
                        {
                            filter.LandmarkFilter.Value.FilterBySpotId(spotid, (response) =>
                            {
                                if (!this) { return; }

                                var str = "[" + DateTime.Now.ToString("HH:mm:ss") + "] Filter: " + response.Status + (response.SpotVersionId.OnSome ? " Spot Version " + response.SpotVersionId : string.Empty);
                                if (response.ErrorMessage.OnSome)
                                {
                                    str += Environment.NewLine + response.ErrorMessage;
                                }
                                logs.Enqueue(Tuple.Create(DateTime.Now, str, response.Status == MegaLandmarkFilterStatus.Found || response.Status == MegaLandmarkFilterStatus.NotFound));
                                Repaint();
                            });
                        }
                        GUI.enabled = guiEnabled;
                    }
                }

                if (filter.ServiceAccessSource == MegaServiceAccessSourceType.Token)
                {
                    token = EditorGUILayout.TextField("Token", token);
                    if (GUILayout.Button("Update Token", GUILayout.Height(30)))
                    {
                        filter.UpdateToken(token);
                    }
                }

                if (logs.Count > 5) { logs.Dequeue(); }
                if (!Application.isPlaying) { GUI.enabled = true; }

                foreach (var log in logs)
                {
                    EditorGUILayout.HelpBox(log.Item2, log.Item3 ? MessageType.Info : MessageType.Warning);
                }
            }
#endif
        }
    }
}
