using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] Transform launchPoint;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] PlayerScript player;
    [SerializeField] int linePoints = 175;
    [SerializeField] float timeIntervalInPoints = 0.01f;
    [SerializeField] Vector2 offset;
    
    [SerializeField] float factor;

    void OnEnable()
    {
        // components needed when the player is instantiated (lamda function used).
        PlayerScript.OnPlayerInstantiated += () =>{
            player = GameManager.Instance.PlayerInScene.GetComponent<PlayerScript>();
            launchPoint = player.transform;
            };
    }
    void Update()
    {
        if (lineRenderer != null && player && player.IsGrounded)
        {

            if (Input.GetMouseButton(0) )
            {
                
                DrawTrajectory();

                
                lineRenderer.enabled = true;
            }
            else
                lineRenderer.enabled = false;
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
        float clampX = Mathf.Clamp(originalVector.magnitude / player.JumpThreshold.x, player.MinJumpForce, player.MaxJumpForce);
        float clampY = Mathf.Clamp(originalVector.magnitude / player.JumpThreshold.y, player.MinJumpForce, player.MaxJumpForce);
        
        Vector2 directionVector = new Vector2(rotatedVector.x * clampX, rotatedVector.y * clampY) * Camera.main.aspect;
        return directionVector;
    }

    void DrawTrajectory()
    {

        Vector2 origin = launchPoint.position;
        SwipeDetection swipe = InputManager.Instance.GetComponent<SwipeDetection>();
        if(swipe == null) return;
        // the swipe vector represents the direct velocity vector apllied to the player.
        Vector2 startVelocity = calculateOnSwipeVector(swipe.StartPosition, swipe.CurrentPosition) * factor;
        lineRenderer.positionCount = linePoints;
        float time = 0;
        for (int i = 0; i < linePoints; i++)
        {
            // s = u*t + 1/2*g*t*t
            
            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time) + offset.x;
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time) + offset.y;
            Vector2 point = new Vector3(x, y);
            lineRenderer.SetPosition(i, origin + point);
            time += timeIntervalInPoints;
        }
    }
}