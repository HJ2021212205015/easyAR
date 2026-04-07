//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using System.Text.RegularExpressions;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">Utility to get package infomation.</para>
    /// <para xml:lang="zh">获取包信息的工具。</para>
    /// </summary>
    public sealed class UnityPackage
    {
        /// <summary>
        /// <para xml:lang="en">The package version number.</para>
        /// <para xml:lang="zh">包版本号。</para>
        /// </summary>
        public const string Version = "4001.0.0+4903.88c5ea80f";
        /// <summary>
        /// <para xml:lang="en">Package identifier.</para>
        /// <para xml:lang="zh">包标识名称。</para>
        /// </summary>
        public const string Name = "com.easyar.sense";
        /// <summary>
        /// <para xml:lang="en">Name appears in the Unity Editor.</para>
        /// <para xml:lang="zh">显示在Unity编辑器中的名称。</para>
        /// </summary>
        public const string DisplayName = "EasyAR Sense Unity Plugin";

        /// <summary>
        /// <para xml:lang="en">Parsed package version.</para>
        /// <para xml:lang="zh">解析过的包版本。</para>
        /// </summary>
        public static Version ParsedVersion => VersionHelper.ParsePackageVersion(Version);
    }

    /// <summary>
    /// <para xml:lang="en">Utility to get EasyAR Sense library infomation.</para>
    /// <para xml:lang="zh">获取EasyAR Sense库信息的工具。</para>
    /// </summary>
    public sealed class SenseLibrary
    {
        /// <summary>
        /// <para xml:lang="en">Gets the version number of EasyARSense.</para>
        /// <para xml:lang="zh">获得EasyARSense的版本号。</para>
        /// </summary>
        public static string Version => Engine.versionString();
        /// <summary>
        /// <para xml:lang="en">Gets the product name of EasyARSense. (Including variant, operating system and CPU architecture.)</para>
        /// <para xml:lang="zh">获得EasyARSense的产品名称。（包括版本变种、操作系统和CPU架构）</para>
        /// </summary>
        public static string Name => Engine.name();
        /// <summary>
        /// <para xml:lang="en">Gets the release variant of EasyARSense.</para>
        /// <para xml:lang="zh">获得EasyARSense的发布版本。</para>
        /// </summary>
        public static string Variant => Engine.variant();
    }

    internal static class VersionHelper
    {
        private static Regex semVRegex = new Regex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");
        private static Regex buildmetaRegex = new Regex(@"^(?<revision>0|[1-9]\d*)");

        public static Version ParsePackageVersion(string str)
        {
            if (str.StartsWith("?.?.?"))
            {
                return new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            }
            var match = semVRegex.Match(str);
            if (!match.Success)
            {
                return new Version(0, 0);
            }
            int revision = 0;
            if (match.Groups["buildmetadata"].Success)
            {
                var match2 = buildmetaRegex.Match(match.Groups["buildmetadata"].Value);
                if (match2.Success)
                {
                    revision = match2.Groups["revision"].Success ? int.Parse(match2.Groups["revision"].Value) : 0;
                }
            }
            var major = match.Groups["major"].Success ? int.Parse(match.Groups["major"].Value) : 0;
            var minor = match.Groups["minor"].Success ? int.Parse(match.Groups["minor"].Value) : 0;
            var patch = match.Groups["patch"].Success ? int.Parse(match.Groups["patch"].Value) : 0;
            return new Version(major, minor, patch, revision);
        }
    }
}
