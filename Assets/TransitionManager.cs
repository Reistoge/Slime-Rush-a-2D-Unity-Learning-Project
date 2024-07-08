using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    // this 
    private void Start()
    {
        if (instance == null)
        {
            // if the object "instance does not exist", the object instance equals to gamemanager class
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            //if already exist, destroy everytime an scene is created
            Destroy(gameObject);
        }

    }
    public void ExecuteTransition1()
    {
        gameObject.GetComponent<Animator>().SetTrigger("trans1");



    }
    public void ResetTriggerTrans1()
    {
        gameObject.GetComponent<Animator>().ResetTrigger("trans1");




    }


}

