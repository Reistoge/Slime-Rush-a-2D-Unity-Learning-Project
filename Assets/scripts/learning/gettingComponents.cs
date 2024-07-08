using UnityEngine;

public class gettingComponents : MonoBehaviour
{
    // Start is called before the first frame update



    private Transform Transform;
    void Start()
    {
        Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale += Vector3.one*Time.deltaTime;
    }
}
