using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private GhostPoolManager ghostPoolManager; // Reference to the Ghost Pool Manager
    [SerializeField] private float spawnInterval;
    [SerializeField] private float ghostLifetime;
    private Color ghostColor;
    [SerializeField] SpriteRenderer spriteRenderer;

    private Coroutine ghostRoutine;
    void Start(){
        if(ghostPoolManager == null){
         ghostPoolManager= GameManager.instance.GhostPoolManager;
        }
    }

    public void startGhosting()
    {
        if (ghostRoutine == null)
            ghostRoutine = StartCoroutine(spawnGhosts());
    }

    public void stopGhosting()
    {
        if (ghostRoutine != null)
        {
            
            StopCoroutine(ghostRoutine);
            ghostRoutine = null;
        }
    }

    private IEnumerator spawnGhosts()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            createGhost();
        }
    }

    private void createGhost()
    {
        GameObject ghost = ghostPoolManager.GetGhost();
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
         

        // Set up the ghost's sprite and color
        SpriteRenderer originalSprite = spriteRenderer;
        SpriteRenderer ghostSprite = ghost.GetComponent<SpriteRenderer>();
        ghostSprite.sprite = originalSprite.sprite;
        ghostSprite.color = ghostColor;

        // Return the ghost to the pool after its lifetime
        StartCoroutine(returnGhostToPool(ghost));
    }

    private IEnumerator returnGhostToPool(GameObject ghost)
    {
        yield return new WaitForSeconds(ghostLifetime);
        ghostPoolManager.ReturnGhost(ghost);
    }
}
