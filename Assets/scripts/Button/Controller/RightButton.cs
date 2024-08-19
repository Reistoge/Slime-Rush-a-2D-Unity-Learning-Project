using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class RightButton : ControllerButton
{
    
    // 
    new void Update()
    {
        base.Update();

    }
     public override void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.MovXButtons=1;    
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.MovXButtons=0;
    }

 
}
