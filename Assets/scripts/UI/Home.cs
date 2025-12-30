using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LoadSceneWithTransition loadScene;
    public void goHome()
    {
        // print("hi");
        if (PauseController.isGamePaused)
        {
            // Time.timeScale = 1.0f;
            PauseController.isGamePaused = false;
        }
        GameManager.Instance.destroyRuntimeData();
        loadScene.LoadSceneTransition();


    }


}
