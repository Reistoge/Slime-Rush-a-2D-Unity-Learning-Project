using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.EventSystems;


public class ActionButton : ControllerButton,IPointerClickHandler
{
   
    
    new void Update() { 
        
        base.Update(); 
    
    }
    
    public static bool onPressActionButton;
    // for some reason everytime you use this variable you have to manually  set it to false after the first use
   
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
