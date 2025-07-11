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

public class Spikes : MonoBehaviour, IEnemyBehaviour
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
 
    Vector2 upVector;
    void OnTriggerStay2D(Collider2D collision)
    {

         
        dealDamage(collision.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        dealDamage(collision.gameObject);
    }
    public void dealDamage(GameObject o)
    {
        
        if (o.TryGetComponent<IDamageable>(out IDamageable damageable) && damageable.CanTakeDamage) 
        {
            // only when the player is damaged, the spikes deal damages and push the object
            damageable.takeDamage(spikeDamage);
            pushObject(o);
            
        }
    }
     
    public void pushObject(GameObject o){
        if(o.CompareTag("Player")){
            pushPlayer(o);
        }
        else{
            
        }
    }
    public void pushPlayer(GameObject o){
        if (o.TryGetComponent<KnockbackFeedBack>(out KnockbackFeedBack knocback)) 
        {
            // Calculate knockback direction based on spike orientation
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
    public int Damage { get => spikeDamage; set => spikeDamage=value; }
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
