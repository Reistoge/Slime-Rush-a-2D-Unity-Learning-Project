using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D ObjectToFollow;
    Vector3 temPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        // transform.pos.x is constant that says that it cant be changed, so
        // how can we move the camera to the same pos of the object ?
        // we can assign the constant to a variable, we can change pos but
        // not pos.x of the camera or any other object so to this we create a
        // vector variable that can store variables and can be changeable
        // so we assign the vector to the pos of the camera and then we assign the pos.x of that vector to the pos.x of the object 
        // and finally we assign the pos of the camera to the tempos.
        // why we can cange pos.x of the tempPos ?
        // because temPos is simply a variable with data is not the gameobject.
        // Tempos not only is the pos of the object, also Tempos has the methods

        if (ObjectToFollow != null)
        {

            // how we can acces to PlayerMovement variables ??.
            temPos = transform.position;
            temPos.x = ObjectToFollow.position.x;
            transform.position = temPos;



            /*
            temPos.x=Object.position.x;
            temPos.y=transform.position.y;
            temPos.z=transform.position.z;
            transform.position=temPos;
            */
        }
    }


}
