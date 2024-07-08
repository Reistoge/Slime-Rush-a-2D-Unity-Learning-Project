using UnityEngine;

public class CoinLeftRight : MonoBehaviour
{
    // Start is called before the first frame update
    // Adjust this to change speed
    public float speed = 2f;

    // Adjust this to change how high it goes
    public float width = 0.5f;

    // The initial offset of the object
    private float offset;

    bool back;
    void Start()
    {
        speed = Random.Range(1, 3);
        width = Random.Range(1, 2);
        offset = transform.position.x;
    }


    // Update is called once per frame
    void Update()
    {
        // Get the current position of the object
        Vector3 pos = transform.position;

        // Calculate the new y position using a sine function
        float newx = offset + width * Mathf.Sin(Time.time * speed);

        // Set the new position of the object
        //if you put the x on the y axis it makes thing weirds.
        transform.position = new Vector3(newx, pos.y, pos.z);
    }

}
