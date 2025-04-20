using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Spikes : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int spikeDamage=1;
    [Tooltip("Proportion based on sprite size (solid size is the spriteSize by this number)"), SerializeField] float solidXSize = 0.9583333f;
    [Tooltip("Proportion based on sprite size"), SerializeField] float solidYSize = 0.7f;
    [Tooltip("Proportion based on the solid or not trigger size"), SerializeField] float triggerXSize = 0.49f;
    [Tooltip("how much up is the edgeCollider"), SerializeField] float triggerOffset = 13.4f;


    void Start()
    {
        gameObject.tag = "ground";   
    }

    //ideas.
    // enemy1. enemy always with spikes up, downs when sticks with player.
    // enemy2. what if the enemy detects when the object is near ?? ---> spikes up !!.
    // cannons with spikes !!!.

    // things to have in mind:
    // add core retro anim ??.

    // int horizontalThreshold = 10;
    Vector2 upVector;
    void OnTriggerStay2D(Collider2D collision)
    {
        applyDamage(collision);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        applyDamage(collision);
    }

    private void applyDamage(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable!=null)
        {
            // Apply damage
            if (damageable.CanTakeDamage)
            {
                applyFeedback(collision);
                damageable.takeDamage(spikeDamage);
            }


        }
    }

    private void applyFeedback(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<KnockbackFeedBack>())
        {
            // Calculate knockback direction based on spike orientation
            Vector2 spikeDirection = transform.up;

            // Knockback source is the spike's position
            Vector2 knockbackSource = transform.position;

            // Trigger knockback feedback
            collision.gameObject.GetComponent<KnockbackFeedBack>().PlayFeedBack(knockbackSource, spikeDirection);

            print($"Knockback applied: Direction = {spikeDirection}, Source = {knockbackSource}");
        }
    }






    // enable spriterenderer can be part of an animation
    public void hideSpike()
    {

        // this.GetComponent<SpriteRenderer>().enabled = false;
        if (animator == null) return;

        if (Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "up")
        {
            Animator.Play("hide", -1, 0);

        }
    }
    public void unHideSpike()
    {
        if (animator == null) return;
        //enableSpriteRenderer();
        if (Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hide")
        {
            Animator.Play("up", -1, 0);

        }

    }
    public void enableSpriteRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void desactivateCollider()
    {
        this.GetComponent<Collider2D>().enabled = false;

    }
    public void activateCollider()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }
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

    }



    public Animator Animator { get => animator; set => animator = value; }
    public float X { get => solidXSize; set => solidXSize = value; }
    public float Y { get => solidYSize; set => solidYSize = value; }
    public float TriggerX { get => triggerXSize; set => triggerXSize = value; }
    public float TriggerOffset { get => triggerOffset; set => triggerOffset = value; }
}
#if UNITY_EDITOR
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
