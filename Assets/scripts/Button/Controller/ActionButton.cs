using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.EventSystems;


public class ActionButton : ControllerButton,IPointerClickHandler
{
    private float initG;
    private float initD;

    
    new void Update() { base.Update(); }
    
    public static bool onPressActionButton;
    public override void OnPointerUp(PointerEventData eventData)
    {
        onPressActionButton = true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        
        onPressActionButton = false;
    }
  

    public void OnPointerClick(PointerEventData eventData)
    {
        
         
    }
    
 
}
