using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BrokeRockScript : MonoBehaviour
{
    [SerializeField] BrokeRockFX rockEffects;
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;
    [SerializeField] ParticleSystem ps3;
    [SerializeField] UnityEvent OnRockBroken;
    [SerializeField] bool dashRequierement = true;
    [SerializeField] bool requiereFinalCannon = true;




    void Start()
    {
        //ps1.gameObject.SetActive(true);
        // ps2.gameObject.SetActive(true);

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (dashRequierement == false)
            {
                rockEffects.playDestroyAnim();
                ps3.Play();
            }
            else if (requiereFinalCannon)
            {
                if (col.GetComponent<PlayerScript>().IsDashing && GameManager.Instance.LastUsedBarrel && GameManager.Instance.LastUsedBarrel.GetComponent<Cannon>().IsFinal)
                {

                    // Time.timeScale = 0.4f;
                    //ps1.gameObject.SetActive(false);
                    // ps2.gameObject.SetActive(false);
                    rockEffects.playDestroyAnim();
                    ps3.Play();

                    if (SceneManager.GetActiveScene().name == "Tutorial") GameManager.Instance.loadSceneWithTransition("Menu,1");



                }
                else
                {
                    Utils.stopObject(col.gameObject);
                }
            }
            else
            {
                Utils.stopObject(col.gameObject);
            }



        }




    }




}
