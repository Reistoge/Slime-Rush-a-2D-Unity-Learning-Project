using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public void spawn()
    {
        GameManager.instance.spawnPlayer(Vector3.zero, "normal");
    }
}
