using UnityEngine;

public class FollowCamera2 : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D ObjectToFollow;
    Vector3 temPos;

    // Start is called before the first frame update
    void Start()
    {
        ObjectToFollow = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }


    void LateUpdate()
    {

        if (ObjectToFollow != null)
        {


            temPos = transform.position;
            temPos.y = ObjectToFollow.position.y;
            transform.position = temPos;



        }
    }


}
