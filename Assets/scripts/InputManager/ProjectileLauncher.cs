using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] Transform launchPoint;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] PlayerScript playerReference;
    [SerializeField] int linePoints = 175;
    [SerializeField] float timeIntervalInPoints = 0.01f;
    [SerializeField] Vector2 offset;
    [SerializeField] float drawTrajectoryWaitTime = 2f;
    float drawTrajectoryTimer = 0;
    [SerializeField] float factor;

    Color initialStartColor;

    public PlayerScript PlayerReference { get => playerReference; set => playerReference = value; }
    public LineRenderer LineRenderer { get => lineRenderer; set => lineRenderer = value; }
    public float Factor { get => factor; set => factor = value; }
    public int LinePoints { get => linePoints; set => linePoints = value; }
    public Transform LaunchPoint { get => launchPoint; set => launchPoint = value; }
    public Vector2 Offset { get => offset; set => offset = value; }
    public float TimeIntervalInPoints { get => timeIntervalInPoints; set => timeIntervalInPoints = value; }

    void OnEnable()
    {
        // components needed when the player is instantiated (lamda function used).
        PlayerScript.OnPlayerInstantiated += initialize;
    }
    void OnDisable()
    {

        PlayerScript.OnPlayerInstantiated -= initialize;


    }
    void initialize(PlayerScript player)
    {
        if (LineRenderer != null)
        {
            initialStartColor = LineRenderer.startColor;
        }
        this.PlayerReference = player;
        LaunchPoint = player.transform;
    }
    void Update()
    {
        if (LineRenderer != null && PlayerReference && PlayerReference.IsGrounded)
        {

            if (Input.GetMouseButton(0))
            {
                drawTrajectoryTimer += Time.deltaTime;
                drawTrajectory();
                LineRenderer.enabled = true;

                if (drawTrajectoryTimer >= drawTrajectoryWaitTime)
                {
                    LineRenderer.startColor = Color.Lerp(LineRenderer.startColor, initialStartColor, Time.deltaTime);

                }


            }
            else
            {
                LineRenderer.startColor = new Color(0, 0, 0, 0);
                LineRenderer.endColor = new Color(0, 0, 0, 0);
                drawTrajectoryTimer = 0;
                LineRenderer.enabled = false;
            }
        }

    }

    Vector2 calculateOnSwipeVector(Vector2 startPos, Vector2 endPos)
    {
        // startPos: first touch hold position .
        // endPos: a position hold in the screen.
        Vector3 originalVector = endPos - startPos; // A vector pointing along the x-axis
                                                    // Define a quaternion (e.g., a 90-degree rotation around the y-axis)
        Quaternion rotation = Quaternion.Euler(0, 0, 180);

        // Rotate the vector using the swipe detection (yes, you can rotate a vector multiplying by a quaternion).
        Vector3 rotatedVector = (rotation * originalVector).normalized;

        // clamp the vector axis 
        float clampX = Mathf.Clamp(originalVector.magnitude / PlayerReference.JumpThreshold.x, PlayerReference.MinJumpForce, PlayerReference.MaxJumpForce);
        float clampY = Mathf.Clamp(originalVector.magnitude / PlayerReference.JumpThreshold.y, PlayerReference.MinJumpForce, PlayerReference.MaxJumpForce);

        Vector2 directionVector = new Vector2(rotatedVector.x * clampX, rotatedVector.y * clampY) * Camera.main.aspect;
        return directionVector;
    }

    void drawTrajectory()
    {

        Vector2 origin = LaunchPoint.position;
        SwipeDetection swipe = InputManager.Instance.GetComponent<SwipeDetection>();
        if (swipe == null) return;
        // the swipe vector represents the direct velocity vector apllied to the player.
        Vector2 startVelocity = calculateOnSwipeVector(swipe.StartPosition, swipe.CurrentPosition) * Factor;
        LineRenderer.positionCount = linePoints;
        float time = 0;
        for (int i = 0; i < linePoints; i++)
        {
            // s = u*t + 1/2*g*t*t

            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time) + offset.x;
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time) + offset.y;
            Vector2 point = new Vector3(x, y);
            LineRenderer.SetPosition(i, origin + point);
            time += TimeIntervalInPoints;
        }
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileLauncher)), CanEditMultipleObjects]
public class ProjectileLauncherEditor : Editor
{
    private Vector2 editorStartPos;
    private Vector2 editorEndPos;
    private bool isDrawingTrajectory = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProjectileLauncher launcher = (ProjectileLauncher)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor Trajectory Preview", EditorStyles.boldLabel);

        // Input fields for start and end positions
        editorStartPos = EditorGUILayout.Vector2Field("Start Position", editorStartPos);
        editorEndPos = EditorGUILayout.Vector2Field("End Position", editorEndPos);

        EditorGUILayout.Space();

        if (GUILayout.Button("Draw Trajectory (Editor)"))
        {
            DrawEditorTrajectory(launcher);
        }

        if (GUILayout.Button("Set Start Position to Player"))
        {
            if (launcher.PlayerReference != null)
            {
                editorStartPos = launcher.PlayerReference.transform.position;
            }
        }

        if (GUILayout.Button("Clear Trajectory"))
        {
            ClearTrajectory(launcher);
        }

        // Show current trajectory info
        if (isDrawingTrajectory)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Trajectory is being drawn. Adjust start/end positions and click 'Draw Trajectory' to update.", MessageType.Info);

            Vector2 swipeVector = editorEndPos - editorStartPos;
            EditorGUILayout.LabelField($"Swipe Distance: {swipeVector.magnitude:F2}");
            EditorGUILayout.LabelField($"Swipe Direction: {swipeVector.normalized}");
        }
    }

    private void DrawEditorTrajectory(ProjectileLauncher launcher)
    {
        if (launcher.PlayerReference == null)
        {
            EditorUtility.DisplayDialog("Missing Reference", "Assign a PlayerScript reference to use this feature.", "OK");
            return;
        }

        if (launcher.LineRenderer == null)
        {
            EditorUtility.DisplayDialog("Missing Component", "Assign a LineRenderer reference to use this feature.", "OK");
            return;
        }

        // Calculate velocity using the same method as runtime, then apply factor
        Vector2 baseVelocity = CalculateEditorVelocity(launcher, editorStartPos, editorEndPos);
        Vector2 velocity = baseVelocity * launcher.Factor; // Apply factor like in runtime

        // Draw the trajectory
        launcher.LineRenderer.positionCount = launcher.LinePoints;
        Vector2 origin = launcher.LaunchPoint != null ? launcher.LaunchPoint.position : launcher.transform.position;

        float time = 0;
        for (int i = 0; i < launcher.LinePoints; i++)
        {
            // Use the same physics calculation as runtime
            var x = (velocity.x * time) + (Physics.gravity.x / 2 * time * time) + launcher.Offset.x;
            var y = (velocity.y * time) + (Physics.gravity.y / 2 * time * time) + launcher.Offset.y;
            Vector2 point = new Vector2(x, y);
            launcher.LineRenderer.SetPosition(i, origin + point);
            time += launcher.TimeIntervalInPoints;
        }

        launcher.LineRenderer.enabled = true;
        isDrawingTrajectory = true;
        EditorUtility.SetDirty(launcher.LineRenderer);
    }

    private Vector2 CalculateEditorVelocity(ProjectileLauncher launcher, Vector2 startPos, Vector2 endPos)
    {
        // Copy the exact logic from calculateOnSwipeVector
        Vector3 originalVector = endPos - startPos;
        Quaternion rotation = Quaternion.Euler(0, 0, 180);
        Vector3 rotatedVector = (rotation * originalVector).normalized;

        float clampX = Mathf.Clamp(originalVector.magnitude / launcher.PlayerReference.JumpThreshold.x,
            launcher.PlayerReference.MinJumpForce, launcher.PlayerReference.MaxJumpForce);
        float clampY = Mathf.Clamp(originalVector.magnitude / launcher.PlayerReference.JumpThreshold.y,
            launcher.PlayerReference.MinJumpForce, launcher.PlayerReference.MaxJumpForce);

        Vector2 directionVector = new Vector2(rotatedVector.x * clampX, rotatedVector.y * clampY);

        // Apply camera aspect ratio (use main camera if available)
        if (Camera.main != null)
        {
            directionVector *= Camera.main.aspect;
        }
        else
        {
            // Fallback to a reasonable aspect ratio
            directionVector *= 1.777f;
        }

        return directionVector;
    }
    private Vector2 CalculateTrajectoryPoint(Vector2 velocity, float time, Vector2 offset)
    {
        // Physics formula: s = ut + (1/2)at²
        float x = (velocity.x * time) + (Physics.gravity.x / 2 * time * time) + offset.x;
        float y = (velocity.y * time) + (Physics.gravity.y / 2 * time * time) + offset.y;
        return new Vector2(x, y);
    }

    private void ClearTrajectory(ProjectileLauncher launcher)
    {
        if (launcher.LineRenderer != null)
        {
            launcher.LineRenderer.enabled = false;
            launcher.LineRenderer.positionCount = 0;
            isDrawingTrajectory = false;
            EditorUtility.SetDirty(launcher.LineRenderer);
        }
    }

    private void OnSceneGUI()
    {
        ProjectileLauncher launcher = (ProjectileLauncher)target;

        if (isDrawingTrajectory)
        {
            // Draw handles for start and end positions
            Handles.color = Color.green;
            Vector3 newStartPos = Handles.PositionHandle(editorStartPos, Quaternion.identity);
            if (Vector3.Distance(newStartPos, editorStartPos) > 0.01f)
            {
                editorStartPos = newStartPos;
                DrawEditorTrajectory(launcher);
            }

            Handles.color = Color.red;
            Vector3 newEndPos = Handles.PositionHandle(editorEndPos, Quaternion.identity);
            if (Vector3.Distance(newEndPos, editorEndPos) > 0.01f)
            {
                editorEndPos = newEndPos;
                DrawEditorTrajectory(launcher);
            }

            // Draw line between start and end points
            Handles.color = Color.yellow;
            Handles.DrawLine(editorStartPos, editorEndPos);

            // Labels
            Handles.Label(editorStartPos, "Start");
            Handles.Label(editorEndPos, "End");
        }
    }
}
#endif

