using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CannonLevelHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject defaultCannon;
    [SerializeField] GameObject manualRotateCannon;
    [SerializeField] GameObject chooseDirectionCannon;
    [SerializeField] GameObject autoCannon;


    public GameObject DefaultCannon { get => defaultCannon; set => defaultCannon = value; }
    public GameObject ManualRotateCannon { get => manualRotateCannon; set => manualRotateCannon = value; }
    public GameObject ChooseDirectionCannon { get => chooseDirectionCannon; set => chooseDirectionCannon = value; }
    public GameObject AutoCannon { get => autoCannon; set => autoCannon = value; }
}
#if UNITY_EDITOR
[CustomEditor(typeof(CannonLevelHandler))]
public class CannonLevelHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
            
        CannonLevelHandler handler = (CannonLevelHandler)target;
        if (GUILayout.Button("Instantiate Default Cannon at Origin"))
        {
            GameObject cannon = Instantiate(handler.DefaultCannon, Vector3.zero, Quaternion.identity);
            cannon.transform.parent = handler.transform;
            cannon.name = "DefaultCannon";
        }
        if (GUILayout.Button("Instantiate ManualRotateCannon Cannon at Origin"))
        {
            GameObject coin = Instantiate(handler.ManualRotateCannon, Vector3.zero, Quaternion.identity);
            coin.transform.parent = handler.transform;
            coin.name = "ManualRotateCannon";
        }
        if (GUILayout.Button("Instantiate ChooseDirectionCannon Cannon at Origin"))
        {
            GameObject coin = Instantiate(handler.ChooseDirectionCannon, Vector3.zero, Quaternion.identity);
            coin.transform.parent = handler.transform;
            coin.name = "ChooseDirectionCannon";
        }
        if (GUILayout.Button("Instantiate autoCannon Cannon at Origin"))
        {
            GameObject coin = Instantiate(handler.AutoCannon, Vector3.zero, Quaternion.identity);
            coin.transform.parent = handler.transform;
            coin.name = "AutoCannon";
        }
    }
}
#endif

