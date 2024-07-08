using UnityEngine;

public class TapScreen : MonoBehaviour
{

    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("game_started");
            Time.timeScale = 1f;
            Destroy(gameObject);

        }
    }

}
