using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class leftButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool leftButtonPress;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (leftButtonPress)
        {

            GameManager.instance.MovXButtons = -1;
        }
         
    }
    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        print("Button left pressed");
        leftButtonPress = true;
    }
    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
         leftButtonPress= false;
         GameManager.instance.MovXButtons = 0;
    }
}
