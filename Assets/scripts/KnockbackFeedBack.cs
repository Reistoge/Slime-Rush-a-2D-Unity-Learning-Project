using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedBack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float strength=16, delay=0.15f;
    public UnityEvent OnBegin,OnDone;
    public void PlayFeedBack(Vector2 sender){
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = ((Vector2)transform.position - sender).normalized;
        rb.AddForce(direction*strength,ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }
    IEnumerator Reset(){
        yield  return new WaitForSeconds(delay);
        rb.velocity=Vector2.zero;
        OnDone?.Invoke();

    }
   
}
