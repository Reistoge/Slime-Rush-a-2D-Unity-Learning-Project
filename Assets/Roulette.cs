using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Roulette : MonoBehaviour
{
    [SerializeField]
    List<GameObject> items;
    [SerializeField] GameObject autoCannon;

    [Range(30, 360)][SerializeField] float dissappearRate = 60;


    Dictionary<RandomItemBox, CircularMotionMovement> dic = new Dictionary<RandomItemBox, CircularMotionMovement>();



    // Start is called before the first frame update
    void OnEnable()
    {

        foreach (Transform t in transform)
        {
            items.Add(t.gameObject);
            t.TryGetComponent(out CircularMotionMovement c);
            t.TryGetComponent(out RandomItemBox r);
            dic.Add(r, c);

        }
    }
    void Start()
    {
        foreach (var pair in dic)
        {
            pair.Key.OnItemPurchased.AddListener(() =>
            {
                itemPurchase(pair.Key);
            });
        }

        // items.ForEach((GameObject g) => print(g.name));
        // circularBehaviours.ForEach((CircularMotionMovement c) => print(c.Angle));
        // itemBoxes.ForEach((RandomItemBox r) => print("randomItemBox Ok\n"));

    }
    public void itemPurchase(RandomItemBox r)
    {
        // if (!autoCannon.activeInHierarchy)
        // {
        //     autoCannon.SetActive(true);
            
        // }
        foreach (var pair in dic)
        {
            pair.Value.stopMovement();
            StartCoroutine(playInSeconds(pair.Key.breakObject, pair.Value.InitAngle / dissappearRate));
            // pair.Key.breakObject();
            if (pair.Key != r)
            {
                pair.Key.Price = int.MaxValue;



            }
        }
    }
    IEnumerator playInSeconds(Action func, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        func?.Invoke();
    }
}
