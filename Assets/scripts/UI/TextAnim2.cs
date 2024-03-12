using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnim2 : MonoBehaviour
{
    [SerializeField] GameObject firstBarrelReference;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int time=3;
    // Start is called before the first frame update

    
    private void OnEnable()
    {
        BarrelScript.OnEnterFirstBarrel += startCounter;
    }
    private void OnDisable()
    {
        BarrelScript.OnEnterFirstBarrel -= startCounter;
    }
    // Update is called once per frame
    private void Start()
    {
        if( GameObject.FindGameObjectsWithTag("barrel") != null)
        {
             GameObject[] Barrels= GameObject.FindGameObjectsWithTag("barrel");
             for(int i = 0; i <  Barrels.Length; i++)
            {
                if (Barrels[i].GetComponent<BarrelScript>().Is_First)
                {
                    firstBarrelReference = Barrels[i];
                    break;
                }
            }
        }
        
    }
    void Update()
    {
         // if the player wants to speed the process or skip the counter
          if((Input.GetKeyDown(KeyCode.Space) )) {

            
            if(firstBarrelReference != null)
            {
                firstBarrelReference.GetComponent<FirstCannon>().FirstShoot();
                Destroy(gameObject);
            }
          }
    }
    IEnumerator Counter(int time)
    {
        print(time);
        
        for(int i=time; i>=0; i--)
        {
            if (i != 0)
            {
                text.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            else {
                text.text = "GO !!!";
                yield return new WaitForSeconds(1);
            }
            
        }
        if (firstBarrelReference != null)
        {
            firstBarrelReference.GetComponent<FirstCannon>().FirstShoot();
            Destroy (gameObject);
        }
        

    }
    void startCounter()
    {
        StartCoroutine(Counter(3));
    }
}
