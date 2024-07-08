using System.Collections;
using UnityEngine;

public class coroutines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("message");
        //StopCoroutine("message");
        //StopAllCoroutines();

    }
    int bruh(int a, int b)
    {
        int sum = a + b;
        return sum;


    }
    IEnumerator message()
    {
        Debug.Log("This is a coroutine examples");
        yield return new WaitForSeconds(2);
        Debug.Log("This message is shown after 2 seconds");
        yield return new WaitForSeconds(4);
        Debug.Log("Great, you wait for 6 seconds");
        yield return new WaitForSeconds(4);
        bruh(4, 4);
        Debug.Log("The sum of two numbers is " + bruh(2, 4));
        yield return new WaitForSeconds(4);
        Debug.Log("what numbers do you think they are?");




    }


    // Update is called once per frame
    void Update()
    {

    }
}
