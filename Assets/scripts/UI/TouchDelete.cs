using UnityEngine;

public class TouchDelete : MonoBehaviour
{
    [SerializeField] private GameObject coinSpawn;
    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collision.transform.position = coinSpawn.transform.position;

    }

}
