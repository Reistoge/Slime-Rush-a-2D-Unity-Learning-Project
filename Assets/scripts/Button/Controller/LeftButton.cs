using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftButton : ControllerButton
{

    // Start is called before the first frame update

    // Update is called once per frame
    Vector3 mousePos;
    bool isBeingPress;
    void Update()
    {
        base.Update();

        //logic 3
        // if(isBeingPress){
        //     followSlider();
        // }

        // logic2
        // if (isBeingPress)
        // {
        //     mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     GameManager.instance.PlayerInScene.transform.position = new Vector3(mousePos.x, GameManager.instance.PlayerInScene.transform.position.y, GameManager.instance.PlayerInScene.transform.position.z);
        // }
        

    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        // logic1
        GameManager.instance.MovXButtons = 0;

        // logic 2
        // isBeingPress = false;

        // logic 3
        // isBeingPress=false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // logic 3
        // isBeingPress=true;

        // logic 2
        // if (CheckMouseNearPlayerXPosition())
        // {
        //     isBeingPress = true;

        // }


        // logic 1
        GameManager.instance.MovXButtons = -1;

    }


}
