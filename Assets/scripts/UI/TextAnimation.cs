using TMPro; // Import the TextMesh Pro namespace
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    public TextMeshPro text; // Reference to the TextMesh Pro component
    public string[] messages; // Array of messages to display
    public float delay; // Delay between messages in seconds
    bool skip;
    [SerializeField]
    private int index; // Index of the current message
    private float timer; // Timer for the delay
    private bool Switch; // for the array
    private bool activate_bool;

    private void OnEnable()
    {
        // Cannon.OnEnterFirstBarrel += activate;
    }
    private void OnDisable()
    {
        // Cannon.OnEnterFirstBarrel -= activate;
    }
    void Start()
    {


        index = 0; // Start with the first message
        timer = delay; // Set the timer to the delay
        text.text = messages[index]; // Display the first message
    }

    void Update()
    {


        if (Input.GetKeyUp(KeyCode.Space))
        {

            Destroy(gameObject);

        }
        // if scale is lower
        if (activate_bool)
        {



            //at the first time when it 4 it changes because the 1 changes to fast at the start so basically it works !!
            // note: if i want to start from 3 the first in the string should be a 4

            if (transform.localScale.x <= 1f && Switch)
            {
                //change the index
                index++;
                if (index == messages.Length)
                {
                    // if the index surpases the length of the index (it ends the message) then rest 1 to index(prevent error)
                    // and also desactivate the object because we dont need them anymore
                    index = index - 1;

                    Destroy(gameObject);

                }
                // change the text from the object 
                text.text = messages[index];

                // we execute this when switch is again true
                Switch = false;
            }

            if (transform.localScale.x >= 1)
            {
                // we just want to execute one time if not this thing will execute "update" times.
                Switch = true;
            }
        }




    }

    void activate()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<TextMeshPro>().enabled = true;
        activate_bool = true;



    }
}
