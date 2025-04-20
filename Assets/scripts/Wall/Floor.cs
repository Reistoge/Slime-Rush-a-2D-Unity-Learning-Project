using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ShadowCaster2D caster;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float floorHeight;
 


    public BoxCollider2D Col { get => col; set => col = value; }
    public SpriteRenderer Sr { get => sr; set => sr = value; }
    public ShadowCaster2D Caster { get => caster; set => caster = value; }
    public float PlatformHeight { get => floorHeight; set => floorHeight = value; }

}
#if UNITY_EDITOR
[CustomEditor(typeof(Floor)), CanEditMultipleObjects]
public class FloorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Floor handler = (Floor)target;
        if (GUILayout.Button("Set Collision Bounds"))
        {
            handler.Sr = handler.GetComponent<SpriteRenderer>();
            handler.Col = handler.GetComponent<BoxCollider2D>();

            handler.Caster = handler.GetComponent<ShadowCaster2D>();

            handler.Col.size = new Vector2(handler.Sr.size.x, handler.PlatformHeight);
            DestroyImmediate(handler.Caster);
            handler.Caster = handler.AddComponent<ShadowCaster2D>();

        }
    }
}
#endif
