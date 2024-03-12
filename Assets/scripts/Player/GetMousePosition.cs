using UnityEngine;
 

public class GetMousePosition : MonoBehaviour
{
    void Update()
    {
        

        if (Input.GetMouseButtonDown(0)){

             
            // get the mouse position in screen space
            Vector3 mouseScreen = Input.mousePosition;
            // add the camera's near clip plane distance to the z value
            mouseScreen.z = Camera.main.nearClipPlane;
            // convert the screen position to world space
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            // print the mouse position in world space
            Debug.Log("Mouse position in world space: " + mouseWorld);


           

        }

    }
}
