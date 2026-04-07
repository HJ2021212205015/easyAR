//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.IO;
using UnityEditor.Callbacks;
using UnityEngine;

namespace easyar
{
    class APIDocGenerator
    {
        [DidReloadScripts(1)]
        static void GenerateAPIDoc()
        {
            GenDocForDLL(UnityPackage.Name, "EasyAR.Sense");
        }

        /// <summary>
        /// Unity does not generate XML documentation file for DLLs.
        /// Even worse, it removes the file after compilation if -doc is used in compiler options.
        /// So, let's hack!
        /// </summary>
        public static void GenDocForDLL(string pakcage, string lib)
        {
            if (EasyARSettings.Instance && !EasyARSettings.Instance.WorkaroundForUnity.GenerateXMLDoc) { return; }

            var src = Path.GetFullPath($"Packages/{pakcage}/Documentation~/{lib}.xml");
            if (!File.Exists(src)) { return; }
            var dstFolder = Path.GetDirectoryName(Application.dataPath) + "/Library/ScriptAssemblies";
            if (!File.Exists(dstFolder + $"/{lib}.dll")) { return; }
            File.Copy(src, dstFolder + $"/{lib}.xml", true);
        }
    }
}
