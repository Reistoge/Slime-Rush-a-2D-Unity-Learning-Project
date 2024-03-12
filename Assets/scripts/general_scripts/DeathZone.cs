using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathZone : MonoBehaviour

{
    public float death_zone_speed;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        moveMap();
        
    }
    public void moveMap()
    {
        
        transform.Translate(Vector3.up*Time.deltaTime*death_zone_speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BounceZone")==false)
        {
            collision.gameObject.SetActive(false);
            print("destroyed "+ collision.gameObject.name);
        }
         
    }
    
}
