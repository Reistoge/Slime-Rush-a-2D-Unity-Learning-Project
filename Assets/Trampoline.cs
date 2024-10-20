using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f; // Adjust this value to control the bounce strength
    public Transform trampolineTransform; // Reference to the trampoline's transform for deformation
    public float deformationAmount = 0.2f; // Amount of deformation
    public float deformationDuration = 0.2f; // Duration of the deformation

    private Vector3 originalScale;
    bool isDeforming;
    bool inPlatform;

    private void Start()
    {
        originalScale = trampolineTransform.localScale;
    }
    void OnCollisionExit2D( Collision2D col){
        if (col.gameObject.CompareTag("Player")){
            inPlatform=false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inPlatform=true;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                StartCoroutine(trampolineBehaviour(rb));
            }
        }
    }


    IEnumerator trampolineBehaviour(Rigidbody2D rb)
    {

        // Deform the trampoline
        if (isDeforming == false)
        {
            isDeforming = true;
            StartCoroutine(deformTrampoline());
            yield return new WaitUntil(() => isDeforming == false);
            if(inPlatform){
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);

            }

        }


    }
    
   
    private IEnumerator deformTrampoline()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y - deformationAmount, originalScale.z);

        while (elapsedTime < deformationDuration)
        {
            trampolineTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / deformationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trampolineTransform.localScale = targetScale;

        // Wait for a moment before returning to the original scale
        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < deformationDuration)
        {
            trampolineTransform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / deformationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trampolineTransform.localScale = originalScale;
        isDeforming = false;
    }
}
