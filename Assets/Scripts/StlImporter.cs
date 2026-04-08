using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ezAR.Model
{
    public static class StlImporter
    {
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

            return mesh;
        }
    }
}
