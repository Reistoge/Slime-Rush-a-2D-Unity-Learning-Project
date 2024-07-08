using UnityEngine;

public class GetIndexName : MonoBehaviour
{
    // function that will load the scene this script attached to the button.

    public void GetIndex()
    {
        int clickObject = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        // clickObject = int.Parse(clickObject); cant do this because clickobject is
        //

        GameManager.instance.Char_Index = clickObject;




    }
}
