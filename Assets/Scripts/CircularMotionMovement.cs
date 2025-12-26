using System;
using UnityEngine;

/// <summary>
/// Provides circular motion movement for game objects.
/// Objects move in a circular path around their initial position at a configurable speed.
/// </summary>
public class CircularMotionMovement : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("Radius of the circular motion in units")]
    [SerializeField] private float radius = 2f;

    [Tooltip("Time in seconds to complete one full rotation (higher = slower)")]
    [SerializeField] private float period = 2f;

    [Tooltip("Current angle of rotation in radians")]
    [SerializeField] private float angle;

    [Tooltip("Initial angle when the object starts moving")]
    [SerializeField] private float initAngle;

    #endregion

    #region Private Fields

    /// <summary>Center point of the circular motion</summary>
    private Vector3 center;

    #endregion

    #region Properties

    /// <summary>Gets or sets the period of rotation</summary>
    public float Period { get => period; set => period = value; }

    /// <summary>Gets or sets the radius of the circular motion</summary>
    public float Radius { get => radius; set => radius = value; }

    /// <summary>Gets or sets the current angle</summary>
    public float Angle { get => angle; set => angle = value; }

    /// <summary>Gets or sets the initial angle</summary>
    public float InitAngle { get => initAngle; set => initAngle = value; }

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the center point and initial angle.
    /// </summary>
    void Start()
    {
        center = transform.position;
        initAngle = angle;
    }

    /// <summary>
    /// Updates the object's position based on circular motion.
    /// </summary>
    void Update()
    {
        // Skip movement if period is zero or negative
        if (Period <= 0f)
        {
            return;
        }

        // Calculate the angular velocity and update the angle
        angle += (2 * Mathf.PI / Period) * Time.deltaTime;

        // Calculate new position using parametric circle equations
        float x = Mathf.Cos(angle) * Radius;
        float y = Mathf.Sin(angle) * Radius;

        // Apply the new position relative to the center
        transform.position = center + new Vector3(x, y, 0f);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Stops the circular motion by negating the period.
    /// Call ResumeMovement() to resume.
    /// </summary>
    public void StopMovement()
    {
        period *= -1;
    }

    /// <summary>
    /// Resumes the circular motion if it was previously stopped.
    /// </summary>
    public void ResumeMovement()
    {
        if (period <= 0)
        {
            period *= -1;
        }
    }

    #endregion
}