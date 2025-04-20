using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Color = UnityEngine.Color;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
[RequireComponent(typeof(WaterTriggerHandler))]
public class InteractableWater : MonoBehaviour
{
    [Header("Springs")]
    [SerializeField] float spriteConstant = 1.4f;
    [SerializeField] float damping = 1.1f;
    [SerializeField] float spread = 6.5f;
    [SerializeField, Range(1, 10)] float wavePropagationIterations = 8;
    [SerializeField, Range(0, 20)] float speedMult = 5.5f;



    [Header("Force")]
    [SerializeField,] float forceMultiplier = 0.2f;
    [SerializeField, Range(1, 50f)] float maxForce = 5f;

    [Header("Collision")]
    [SerializeField, Range(1, 10f)] float playerCollisionRadiusMult = 4.15f;

    [Header("Mesh Generation")]
    [SerializeField, Range(2, 500)] int numOfXVertices = 70;

    public float width = 10f;
    public float height = 4f;
    [SerializeField] Material waterMat;
    const int NUM_OF_Y_VERTICES = 2;
    [Header("Gizmo")]
    [SerializeField] Color gizmoColor = Color.white;
    Mesh mesh;
    MeshRenderer meshRender;
    MeshFilter meshFilter;
    Vector3[] vertices;
    int[] topVerticesIndex;
    EdgeCollider2D coll;
    public class WaterPoint
    {
        public float velocity, acceleration, pos, targetHeight;
    }
    private List<WaterPoint> waterPoints = new List<WaterPoint>();

    public Color GizmoColor { get => gizmoColor; set => gizmoColor = value; }
    public float Width { get => width; set => width = value; }
    public float Height { get => height; set => height = value; }
    public float ForceMultiplier { get => forceMultiplier; set => forceMultiplier = value; }
    public float MaxForce { get => maxForce; set => maxForce = value; }

    void FixedUpdate()
    {
        //update all spring position
        for (int i = 1; i < waterPoints.Count -1; i++)
        {
            WaterPoint point = waterPoints[i];
            float x = point.pos - point.targetHeight;
            float acceleration = -spriteConstant * x - damping * point.velocity;
            point.pos += point.velocity * speedMult * Time.fixedDeltaTime;
            vertices[topVerticesIndex[i]].y = point.pos;
            point.velocity += acceleration * speedMult * Time.fixedDeltaTime;


        }
        //wave propagation
        for (int j = 0; j < wavePropagationIterations; j++)
        {
            for (int i = 1; i < waterPoints.Count - 1; i++)
            {
                float leftDelta = spread * (waterPoints[i].pos - waterPoints[i - 1].pos) * speedMult * Time.fixedDeltaTime;
                waterPoints[i - 1].velocity += leftDelta;

                float rightDelta = spread * (waterPoints[i].pos - waterPoints[i + 1].pos) * speedMult * Time.fixedDeltaTime;
                waterPoints[i + 1].velocity += rightDelta;

            }
        }
        //update mesh
        mesh.vertices = vertices;
    }
    public void Splash(Collider2D col, float force)
    {
        float radius = col.bounds.extents.x - playerCollisionRadiusMult;
        Vector2 center = col.transform.position;
        for (int i = 0; i < waterPoints.Count; i++)
        {
            Vector2 vertexWorldPos = transform.TransformPoint(vertices[topVerticesIndex[i]]);
            if (IsPointInsideCircle(vertexWorldPos, center, radius))
            {
                waterPoints[i].velocity = force;
            }
        }
    }
    bool IsPointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        float distanceSquared = (point - center).sqrMagnitude;
        return distanceSquared <= (radius*radius);

    }

    void Start()
    {
        // this methods is called when you add a component to the gameobject.
        coll = GetComponent<EdgeCollider2D>();
        generateMesh();
        createWaterPoints();
    }
    void createWaterPoints()
    {
        waterPoints.Clear();
        for (int i = 0; i < topVerticesIndex.Length; i++)
        {
            waterPoints.Add(new WaterPoint
            {
                pos = vertices[topVerticesIndex[i]].y,
                targetHeight = vertices[topVerticesIndex[i]].y,

            });
        }

    }
    void Reset()
    {
        // this methods is called when you add a component to the gameobject.
        coll = GetComponent<EdgeCollider2D>();
        coll.isTrigger = true;
    }

    public void generateMesh()
    {
        mesh = new Mesh();
        vertices = new Vector3[(numOfXVertices * NUM_OF_Y_VERTICES)];
        topVerticesIndex = new int[numOfXVertices];
        for (int y = 0; y < NUM_OF_Y_VERTICES; y++)
        {
            for (int x = 0; x < numOfXVertices; x++)
            {
                float xPos = (x / (float)(numOfXVertices - 1)) * width - width / 2;
                float yPos = (y / (float)(NUM_OF_Y_VERTICES - 1)) * height - height / 2;
                vertices[y * numOfXVertices + x] = new Vector3(xPos, yPos, 0f);
                if (y == NUM_OF_Y_VERTICES - 1)
                {
                    topVerticesIndex[x] = y * numOfXVertices + x;
                }


            }
        }
        int[] triangles = new int[(numOfXVertices - 1) * (NUM_OF_Y_VERTICES - 1) * 6];
        int index = 0;
        for (int y = 0; y < NUM_OF_Y_VERTICES - 1; y++)
        {
            for (int x = 0; x < numOfXVertices - 1; x++)
            {
                int bottomLeft = y * numOfXVertices + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + numOfXVertices;
                int topRight = topLeft + 1;

                //first triangle
                triangles[index++] = bottomLeft;
                triangles[index++] = topLeft;
                triangles[index++] = bottomRight;

                //second triangle
                triangles[index++] = bottomRight;
                triangles[index++] = topLeft;
                triangles[index++] = topRight;



            }
        }
        //UVS 
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x + width / 2) / width, (vertices[i].y + height / 2) / height);
        }
        if (meshRender == null)
        {
            meshRender = GetComponent<MeshRenderer>();

        }
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
        meshRender.material = waterMat;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;




    }

    public void resetEdgeCollider()
    {
        coll = GetComponent<EdgeCollider2D>();
        Vector2[] newPoints = new Vector2[2];
        Vector2 firstPoint = new Vector2(vertices[topVerticesIndex[0]].x, vertices[topVerticesIndex[0]].y);
        newPoints[0] = firstPoint;
        Vector2 secondPoint = new Vector2(vertices[topVerticesIndex[topVerticesIndex.Length - 1]].x, vertices[topVerticesIndex[topVerticesIndex.Length - 1]].y);
        newPoints[1] = secondPoint;
        coll.offset = Vector2.zero;

    }
}
[CustomEditor(typeof(InteractableWater))]
public class InteractableWaterEditor : Editor
{
    InteractableWater water;
    private void OnEnable()
    {
        water = (InteractableWater)target;

    }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        root.Add(new VisualElement { style = { height = 10 } });
        Button generateMeshButton = new Button(() => water.generateMesh())
        {
            text = "Generate Mesh"
        };
        root.Add(generateMeshButton);

        Button placeEdgeColliderButton = new Button(() => water.resetEdgeCollider())
        {
            text = "Place Edge Collider"
        };
        root.Add(placeEdgeColliderButton);
        return root;
    }
    private void ChangeDimensions(ref float width, ref float height, float calculatedWidthMax, float calculatedHeightMax)
    {
        width = Mathf.Max(0.1f, calculatedWidthMax);
        height = Mathf.Max(0.1f, calculatedHeightMax);

    }
    private void OnSceneGUI()
    {
        // DRAW the wireframe box
        Handles.color = water.GizmoColor;
        Vector3 center = water.transform.position;
        Vector3 size = new Vector3(water.Width, water.Height, 0.1f);
        Handles.DrawWireCube(center, size);

        // Handles for width and height
        float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
        Vector3 snap = Vector3.one * 0.1f;

        // corner handles
        Vector3[] corners = new Vector3[4];
        corners[0] = center + new Vector3(-water.Width / 2, -water.Height / 2, 0); // bottom-left
        corners[0] = center + new Vector3(water.Width / 2, water.Height / 2, 0); // bottom-right
        corners[0] = center + new Vector3(-water.Width / 2, water.Height / 2, 0); // top-left
        corners[0] = center + new Vector3(water.Width / 2, water.Height / 2, 0); // top-right

        //Handle for each corner
        EditorGUI.BeginChangeCheck();
        Vector3 newBottomLeft = Handles.FreeMoveHandle(corners[0], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, corners[1].x - newBottomLeft.x, corners[3].y - newBottomLeft.y);
            water.transform.position += new Vector3((newBottomLeft.x - corners[0].x) / 2, (newBottomLeft.y - corners[0].y) / 2, 0);
        }
        EditorGUI.BeginChangeCheck();
        Vector3 newBottomRight = Handles.FreeMoveHandle(corners[1], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, newBottomRight.x - corners[0].x, corners[3].y - newBottomRight.y);
            water.transform.position += new Vector3((newBottomRight.x - corners[1].x) / 2, (newBottomRight.y - corners[1].y) / 2, 0);
        }
        EditorGUI.BeginChangeCheck();
        Vector3 newTopLeft = Handles.FreeMoveHandle(corners[2], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, corners[3].x - newTopLeft.x, newTopLeft.y - corners[0].y);
            water.transform.position += new Vector3((newTopLeft.x - corners[2].x) / 2, (newBottomRight.y - corners[1].y) / 2, 0);
        }
        EditorGUI.BeginChangeCheck();
        Vector3 newTopRight = Handles.FreeMoveHandle(corners[3], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, newTopRight.x - corners[2].x, newTopRight.y - corners[1].y);
            water.transform.position += new Vector3((newTopRight.x - corners[3].x) / 2, (newTopRight.y - corners[3].y / 2));
        }
        if (GUI.changed)
        {
            water.generateMesh();
        }

    }

}
