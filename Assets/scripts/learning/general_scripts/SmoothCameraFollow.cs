using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Speed of the camera's smooth transition
    public Vector3 offset; // Offset from the player
    public float zoomSpeed = 2f; // Speed of the zoom effect
    public float minZoom = 5f; // Minimum zoom level
    public float maxZoom = 10f; // Maximum zoom level

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Smoothly follow the player
        Vector3 desiredPosition = player.position + offset;
        
        Vector3 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z=-10f;
        transform.position = smoothedPosition;

        // Zoom in and out based on player's speed
        float playerSpeed = player.GetComponent<Rigidbody2D>().velocity.magnitude;
        float desiredZoom = Mathf.Lerp(maxZoom, minZoom, playerSpeed / zoomSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredZoom, Time.deltaTime);
    }
}
