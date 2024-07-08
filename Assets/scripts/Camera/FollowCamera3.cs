using UnityEngine;

public class FollowCamera3 : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform Target;
    [SerializeField]
    private Transform offsetPoint;
    [SerializeField]
    bool start = false;
    float offset = 0;
    [SerializeField]
    float speed = 5;
    private bool followObject;
    public bool GetFollowObject()
    {
        return followObject;

    }
    public void SetFollowObject(bool state)
    {
        followObject = state;
    }


    // reference to the instance of barrel





    public bool switcher(bool boolean)
    {
        return !boolean;
    }

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = offsetPoint.transform.position.y;
        followObject = true;
    }

    private void Update()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    //more smooth because the vector creates after or because the greater or equal than ?
    {
        if (followObject)
        {
            if (Target.position.y == offsetPoint.transform.position.y)
            {
                // when the object reach the offset position, i want to start following the object
                start = true;

            }
            /// the player has to reach a certain position first and then the camera follows :)
            /// for this script the object has to be in the principle of the game when the player spawns in above the camera inital position and the offset and first barrel should have the same pos



            // solve camera problem with the barrels

            if (Target.position.y > transform.position.y)
            {
                Vector3 newPos = new Vector3(transform.position.x, Target.position.y + (Mathf.Abs(offset)), transform.position.z);
                // Use Vector3.Lerp to interpolate between the current position and the new position by a fraction of speed * Time.deltaTime
                transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);


            }


            if (Target.position.y < transform.position.y && start)
            {
                Vector3 newPos = new Vector3(transform.position.x, Target.position.y + (Mathf.Abs(offset)), transform.position.z);

                // Use Vector3.Lerp to interpolate between the current position and the new position by a fraction of speed * Time.deltaTime
                transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);

            }



        }

















    }

}
