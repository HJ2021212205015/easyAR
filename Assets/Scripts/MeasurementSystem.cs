using UnityEngine;

namespace ezAR.Measure
{
    public class MeasurementSystem : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private float gridSize = 0.01f; // 1cm
        [SerializeField] private int gridExtent = 50; // 50 cells each direction
        [SerializeField] private float gridHeightOffset = 0f;
        [SerializeField] private Material gridMaterial;

        [Header("Measurement Points")]
        [SerializeField] private GameObject pointAPrefab;
        [SerializeField] private GameObject pointBPrefab;
        [SerializeField] private Color pointAColor = Color.red;
        [SerializeField] private Color pointBColor = Color.blue;

        private GameObject gridPlane;
        private GameObject pointA;
        private GameObject pointB;
        private LineRenderer measurementLine;
        private bool isMeasuring = false;

        public Vector3? PointAPosition { get; private set; }
        public Vector3? PointBPosition { get; private set; }
        public float GridHeight { get; private set; }

        public void CreateGrid()
        {
            if (gridPlane) Destroy(gridPlane);

            gridPlane = new GameObject("MeasurementGrid");
            gridPlane.transform.SetParent(transform, false);
            gridPlane.transform.localPosition = new Vector3(0, gridHeightOffset, 0);

            var filter = gridPlane.AddComponent<MeshFilter>();
            var renderer = gridPlane.AddComponent<MeshRenderer>();

            var mesh = new Mesh();
            var vertices = new System.Collections.Generic.List<Vector3>();
            var indices = new System.Collections.Generic.List<int>();

            float halfExtent = gridSize * gridExtent;

            // Create grid lines
            for (int i = -gridExtent; i <= gridExtent; i++)
            {
                float pos = i * gridSize;

                // Horizontal lines
                vertices.Add(new Vector3(-halfExtent, 0, pos));
                vertices.Add(new Vector3(halfExtent, 0, pos));

                // Vertical lines
                vertices.Add(new Vector3(pos, 0, -halfExtent));
                vertices.Add(new Vector3(pos, 0, halfExtent));
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                indices.Add(i);
            }

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            filter.mesh = mesh;

            if (gridMaterial)
            {
                renderer.material = gridMaterial;
            }
            else
            {
                var mat = new Material(Shader.Find("Unlit/Color"));
                mat.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                renderer.material = mat;
            }
        }

        public void SetGridHeight(float height)
        {
            GridHeight = height;
            if (gridPlane)
            {
                gridPlane.transform.localPosition = new Vector3(0, height, 0);
            }
        }

        public void RecordPointA()
        {
            if (!gridPlane) return;

            var localPos = new Vector3(0, GridHeight, 0);
            PointAPosition = gridPlane.transform.TransformPoint(localPos);

            if (pointA) Destroy(pointA);
            pointA = CreateMeasurementPoint("PointA", pointAColor, PointAPosition.Value);

            UpdateMeasurementLine();
            isMeasuring = true;
        }

        public void RecordPointB()
        {
            if (!gridPlane) return;

            var localPos = new Vector3(0, GridHeight, 0);
            PointBPosition = gridPlane.transform.TransformPoint(localPos);

            if (pointB) Destroy(pointB);
            pointB = CreateMeasurementPoint("PointB", pointBColor, PointBPosition.Value);

            UpdateMeasurementLine();
        }

        private GameObject CreateMeasurementPoint(string name, Color color, Vector3 position)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;
            go.transform.SetParent(transform, false);
            go.transform.position = position;
            go.transform.localScale = Vector3.one * 0.005f; // 5mm sphere

            var renderer = go.GetComponent<Renderer>();
            var mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            renderer.material = mat;

            return go;
        }

        private void UpdateMeasurementLine()
        {
            if (!PointAPosition.HasValue || !PointBPosition.HasValue) return;

            if (!measurementLine)
            {
                var lineObj = new GameObject("MeasurementLine");
                lineObj.transform.SetParent(transform, false);
                measurementLine = lineObj.AddComponent<LineRenderer>();
                measurementLine.startWidth = 0.002f;
                measurementLine.endWidth = 0.002f;
                measurementLine.material = new Material(Shader.Find("Unlit/Color"));
                measurementLine.material.color = Color.yellow;
            }

            measurementLine.SetPosition(0, PointAPosition.Value);
            measurementLine.SetPosition(1, PointBPosition.Value);
        }

        public MeasurementResult GetMeasurementResult()
        {
            if (!PointAPosition.HasValue || !PointBPosition.HasValue)
            {
                return new MeasurementResult();
            }

            var a = PointAPosition.Value;
            var b = PointBPosition.Value;

            float horizontalDistance = new Vector2(b.x - a.x, b.z - a.z).magnitude;
            float verticalDifference = b.y - a.y;
            float directDistance = Vector3.Distance(a, b);

            return new MeasurementResult
            {
                pointA = a,
                pointB = b,
                horizontalDistance = horizontalDistance,
                verticalDifference = verticalDifference,
                directDistance = directDistance
            };
        }

        public void ClearMeasurement()
        {
            if (pointA) Destroy(pointA);
            if (pointB) Destroy(pointB);
            if (measurementLine) Destroy(measurementLine.gameObject);

            PointAPosition = null;
            PointBPosition = null;
            isMeasuring = false;
        }

        public void ToggleGrid(bool visible)
        {
            if (gridPlane)
            {
                gridPlane.SetActive(visible);
            }
        }
    }

    public struct MeasurementResult
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public float horizontalDistance;
        public float verticalDifference;
        public float directDistance;

        public override string ToString()
        {
            return $"水平距离: {horizontalDistance * 100:F2} cm\n" +
                   $"垂直差: {verticalDifference * 100:F2} cm\n" +
                   $"直线距离: {directDistance * 100:F2} cm";
        }
    }
}