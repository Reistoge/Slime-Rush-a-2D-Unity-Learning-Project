using System.Collections.Generic;
using UnityEngine;

public class GhostPoolManager : MonoBehaviour
{
    [Header("Object Pool Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> ghostPool;

    void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        ghostPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform);
            ghost.SetActive(false);
            ghostPool.Enqueue(ghost);
        }
    }

    public GameObject GetGhost()
    {
        if (ghostPool.Count > 0)
        {
            GameObject ghost = ghostPool.Dequeue();
            
            ghost.SetActive(true);
            return ghost;
        }

        // If the pool is exhausted, create a new ghost (optional)
        GameObject newGhost = Instantiate(ghostPrefab);
        newGhost.SetActive(true);
        return newGhost;
    }

    public void ReturnGhost(GameObject ghost)
    {
        ghost.SetActive(false);
        ghostPool.Enqueue(ghost);
    }
}
