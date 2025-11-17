using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Spike hazard that damages players and enemies on contact.
/// Implements IEnemyBehaviour for consistent enemy damage behavior.
/// </summary>
public class Spikes : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int spikeDamage = 1;
    [SerializeField] private bool onTrigger = true;
    [SerializeField] private bool onCollision = true;

    [Tooltip("Proportion based on sprite size (solid size is the spriteSize by this number)")]
    [SerializeField] private float solidXSize = 0.9583333f;

    [Tooltip("Proportion based on sprite size")]
    [SerializeField] private float solidYSize = 0.7f;

    [Tooltip("Proportion based on the solid or not trigger size")]
    [SerializeField] private float triggerXSize = 0.49f;

    [Tooltip("how much up is the edgeCollider")]
    [SerializeField] private float triggerOffset = 13.4f;

    private void Start()
    {
        gameObject.tag = "ground";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onTrigger)
        {
            dealDamage(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onCollision)
        {
            dealDamage(collision.gameObject);
        }
    }

    /// <summary>
    /// Deal damage to a game object if it implements IDamageable.
    /// </summary>
    /// <param name="target">The object to damage</param>
    public void dealDamage(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable damageable) && damageable.CanTakeDamage)
        {
            damageable.takeDamage(spikeDamage);
            pushObject(target);
        }
    }

    /// <summary>
    /// Applies knockback to an object based on its type.
    /// </summary>
    /// <param name="obj">The object to push back</param>
    public void pushObject(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            pushPlayer(obj);
        }
    }

    /// <summary>
    /// Applies knockback feedback to the player.
    /// </summary>
    /// <param name="player">The player object to knock back</param>
    public void pushPlayer(GameObject player)
    {
        if (o.TryGetComponent<KnockbackFeedBack>(out KnockbackFeedBack knocback)) 
        {
            // Calculate knockback direction based on spike orientation
            //Vector2 shootDirection = transform.up;
            Vector2 shootDirection = transform.up;
            // Knockback source is the spike's position
            Vector2 knockbackSource = transform.position;

            // default object push feedback
            knocback.triggerFeedback(knockbackSource, shootDirection);

            print($"Knockback applied: Direction = {shootDirection}, Source = {knockbackSource}" + " Object = {knocback.name}");
        }


    }

 





    // enable spriterenderer can be part of an animation
    public void hideSpike()
    {
        if (animator == null) return;

        if (Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "up")
        {
            Animator.Play("hide", -1, 0);
        }
    }

    /// <summary>
    /// Shows the spike by playing the up animation.
    /// </summary>
    public void unHideSpike()
    {
        if (animator == null) return;

        if (Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hide")
        {
            Animator.Play("up", -1, 0);
        }
    }

    /// <summary>
    /// Enables the sprite renderer.
    /// </summary>
    public void enableSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Disables the collider.
    /// </summary>
    public void desactivateCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// Enables the collider.
    /// </summary>
    public void activateCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// Sets the collision bounds for the spike based on sprite size.
    /// Editor utility method.
    /// </summary>
    /// <param name="spike">The spike to configure</param>
    public void setBounds(Spikes spike)
    {
        SpriteRenderer sr = spike.Animator.gameObject.GetComponent<SpriteRenderer>();
        float x = sr.size.x * spike.X;
        float y = sr.size.y * spike.Y;
        spike.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(x, y);
        List<Vector2> points = new List<Vector2>();
        if (spike.GetComponent<EdgeCollider2D>() == null)
        {

            spike.AddComponent<EdgeCollider2D>();
            spike.gameObject.GetComponent<EdgeCollider2D>().isTrigger = true;

        }
        points.Add(new Vector2(x * spike.TriggerX, spike.gameObject.GetComponent<EdgeCollider2D>().points[0].y));
        points.Add(new Vector2(-x * spike.TriggerX, spike.gameObject.GetComponent<EdgeCollider2D>().points[1].y));
        spike.gameObject.GetComponent<EdgeCollider2D>().SetPoints(points);
        spike.gameObject.GetComponent<EdgeCollider2D>().offset = new Vector2(0, spike.TriggerOffset);

        // Configure 2D light
        animator.gameObject.GetComponent<Light2D>().lightType = Light2D.LightType.Freeform;

        Vector2 spriteSize = sr.size;
        Vector3[] shapePath = new Vector3[4];
        float verticalOffset = 5f;
        shapePath[0] = new Vector3(-spriteSize.x / 2, verticalOffset, 0);
        shapePath[1] = new Vector3(spriteSize.x / 2, verticalOffset, 0);
        shapePath[2] = new Vector3(spriteSize.x / 2, spriteSize.y, 0);
        shapePath[3] = new Vector3(-spriteSize.x / 2, spriteSize.y, 0);
        animator.gameObject.GetComponent<Light2D>().SetShapePath(shapePath);
    }

    // Properties
    public Animator Animator { get => animator; set => animator = value; }
    public float X { get => solidXSize; set => solidXSize = value; }
    public float Y { get => solidYSize; set => solidYSize = value; }
    public float TriggerX { get => triggerXSize; set => triggerXSize = value; }
    public float TriggerOffset { get => triggerOffset; set => triggerOffset = value; }
    public int Damage { get => spikeDamage; set => spikeDamage = value; }
}

#if UNITY_EDITOR
/// <summary>
/// Custom editor for Spikes to provide utility buttons.
/// </summary>
[CustomEditor(typeof(Spikes)), CanEditMultipleObjects]
public class SpikesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Spikes handler = (Spikes)target;

        if (GUILayout.Button("Set Collision Bounds"))
        {
            handler.setBounds(handler);
        }

        if (GUILayout.Button("Set Collision Bounds to All"))
        {
            List<UnityEngine.Object> items = FindObjectsByType(typeof(Spikes), FindObjectsSortMode.None).ToList();
            foreach (UnityEngine.Object item in items)
            {
                item.GetComponent<Spikes>().setBounds(item.GetComponent<Spikes>());
            }
        }
    }
}
#endif
