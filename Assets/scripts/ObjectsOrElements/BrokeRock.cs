using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokeRockScript : MonoBehaviour
{
    [SerializeField] BrokeRockFX rockEffects;
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;


    void Start()
    {
        ps1.gameObject.SetActive(true);
        ps2.gameObject.SetActive(true);

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerScript>() != null && col.GetComponent<PlayerScript>().IsDashing)
        {
            
            Time.timeScale = 0.4f;
            ps1.gameObject.SetActive(false);
            ps2.gameObject.SetActive(false);
            rockEffects.playDestroyAnim();

        }


    }




}
