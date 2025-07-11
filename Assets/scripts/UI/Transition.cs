using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class Transition : MonoBehaviour
{
    // Start is called before the first frame update
    bool sceneCanBeLoaded=false;
    PlayableDirector playableDirector;
    void OnEnable()
    {
        SceneManager.sceneLoaded += resumeTransition;

    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= resumeTransition;
    }
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        if (playableDirector != null)
        {
            
            playableDirector.Play();

        }
    }
 
    public void resumeTransition(Scene scene, LoadSceneMode mode)
    {
        if (playableDirector != null)
        {
            
            StartCoroutine(waitForTransition());
        }
   
    }
    IEnumerator waitForTransition()
    {
        playableDirector.Pause();
        yield return new WaitForSecondsRealtime(2f);
        if (playableDirector != null)
        {
            CancelInvoke();
            playableDirector.Resume();
        }
        


    }

    public void destroy()
    {
        Destroy(this.gameObject);
    }
    public bool SceneCanBeLoaded { get => sceneCanBeLoaded; set => sceneCanBeLoaded = value; }
}
