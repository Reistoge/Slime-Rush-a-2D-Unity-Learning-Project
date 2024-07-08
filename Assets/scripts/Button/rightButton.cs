using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class rightButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool rightButtonPress;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (rightButtonPress)
        {
            GameManager.instance.MovXButtons = 1;
        }

    }
    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        print("right button pressed");
        rightButtonPress = true;
    }
    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        rightButtonPress = false;
        GameManager.instance.MovXButtons = 0;
    }
}
