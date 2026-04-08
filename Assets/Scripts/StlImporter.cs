using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace ezAR.Model
{
    public static class StlImporter
    {
        /// <summary>
        /// 从文件导入STL（自动检测格式）
        /// </summary>
        public static Mesh ImportFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[StlImporter] File not found: {filePath}");
                return null;
            }

            // 检测文件格式
            if (IsAsciiStl(filePath))
            {
                Debug.Log("[StlImporter] Detected ASCII format");
                return ImportFromAsciiFile(filePath);
            }
            else
            {
                Debug.Log("[StlImporter] Detected Binary format");
                var bytes = File.ReadAllBytes(filePath);
                return ImportFromBinary(bytes);
            }
        }

        /// <summary>
        /// 检测是否为ASCII格式STL
        /// </summary>
        private static bool IsAsciiStl(string filePath)
        {
            using var reader = new StreamReader(filePath);
            string firstLine = reader.ReadLine();
            if (string.IsNullOrEmpty(firstLine)) return false;

            // ASCII STL 以 "solid" 开头
            // 但要注意：有些二进制文件的header也可能以"solid"开头
            // 所以需要进一步检查后面的内容
            firstLine = firstLine.Trim();
            if (!firstLine.StartsWith("solid", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // 检查第二行是否包含 "facet" 或 "endsolid"（ASCII特征）
            string secondLine = reader.ReadLine();
            if (string.IsNullOrEmpty(secondLine)) return false;

            secondLine = secondLine.Trim();
            return secondLine.StartsWith("facet", StringComparison.OrdinalIgnoreCase) ||
                   secondLine.StartsWith("endsolid", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 从ASCII格式STL文件导入
        /// </summary>
        public static Mesh ImportFromAsciiFile(string filePath)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            try
            {
                using var reader = new StreamReader(filePath);
                string line;
                Vector3 currentNormal = Vector3.up;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("facet normal", StringComparison.OrdinalIgnoreCase))
                    {
                        // 解析法线: "facet normal ni nj nk"
                        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 5)
                        {
                            currentNormal = new Vector3(
                                ParseFloat(parts[2]),
                                ParseFloat(parts[3]),
                                ParseFloat(parts[4])
                            );
                        }
                    }
                    else if (line.StartsWith("vertex", StringComparison.OrdinalIgnoreCase))
                    {
                        // 解析顶点: "vertex x y z"
                        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            var vertex = new Vector3(
                                ParseFloat(parts[1]),
                                ParseFloat(parts[2]),
                                ParseFloat(parts[3])
                            );

                            int index = vertices.Count;
                            vertices.Add(vertex);
                            normals.Add(currentNormal);
                            triangles.Add(index);
                        }
                    }
                }

                if (vertices.Count == 0)
                {
                    Debug.LogError("[StlImporter] No vertices found in ASCII STL");
                    return null;
                }

                var mesh = new Mesh();
                mesh.SetVertices(vertices);
                mesh.SetTriangles(triangles, 0);
                mesh.SetNormals(normals);
                mesh.RecalculateBounds();

                Debug.Log($"[StlImporter] Loaded ASCII STL: {vertices.Count} vertices, {triangles.Count / 3} triangles");
                return mesh;
            }
            catch (Exception e)
            {
                Debug.LogError($"[StlImporter] Failed to parse ASCII STL: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 从二进制格式STL文件导入
        /// </summary>
        public static Mesh ImportFromBinaryFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[StlImporter] File not found: {filePath}");
                return null;
            }

            var bytes = File.ReadAllBytes(filePath);
            return ImportFromBinary(bytes);
        }

        /// <summary>
        /// 从二进制数据导入STL
        /// </summary>
        public static Mesh ImportFromBinary(byte[] data)
        {
            if (data == null || data.Length < 84)
            {
                Debug.LogError("[StlImporter] Invalid binary STL data");
                return null;
            }

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var header = new string(reader.ReadChars(80)).Trim();
            var triangleCount = reader.ReadUInt32();

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            for (int i = 0; i < triangleCount; i++)
            {
                var normal = new Vector3(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

                for (int j = 0; j < 3; j++)
                {
                    var vertex = new Vector3(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                    );

                    int index = vertices.Count;
                    vertices.Add(vertex);
                    normals.Add(normal);
                    triangles.Add(index);
                }

                reader.ReadUInt16();
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetNormals(normals);
            mesh.RecalculateBounds();

            Debug.Log($"[StlImporter] Loaded Binary STL: {vertices.Count} vertices, {triangleCount} triangles");
            return mesh;
        }

        /// <summary>
        /// 解析浮点数（支持科学计数法）
        /// </summary>
        private static float ParseFloat(string s)
        {
            return float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
    }
}
