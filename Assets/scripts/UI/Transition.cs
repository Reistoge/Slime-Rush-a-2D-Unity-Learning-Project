using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Transition : MonoBehaviour
{
    // Start is called before the first frame update
    bool transitionFinished;

    void OnDisable(){
        
         

    }
    void Start(){
        GetComponent<PlayableDirector>().Play();
    }
    public void destroy(){
        Destroy(this.gameObject);
    }
    public bool TransitionFinished { get => transitionFinished; set => transitionFinished = value; }
}
