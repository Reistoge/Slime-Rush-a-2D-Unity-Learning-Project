using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a roulette wheel of random items.
/// Handles item selection and disappearing animations for unpurchased items.
/// </summary>
public class Roulette : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject autoCannon;

    [Range(30, 360)]
    [SerializeField]
    [Tooltip("Rate at which unpurchased items disappear (degrees per second)")]
    private float dissappearRate = 60;

    private Dictionary<RandomItemBox, CircularMotionMovement> itemDictionary = new Dictionary<RandomItemBox, CircularMotionMovement>();

    private void OnEnable()
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

    /// <summary>
    /// Handles the purchase of an item from the roulette.
    /// Stops the wheel and breaks unpurchased items.
    /// </summary>
    /// <param name="purchasedItem">The item that was purchased</param>
    public void itemPurchase(RandomItemBox purchasedItem)
    {
        foreach (var pair in itemDictionary)
        {
            pair.Value.stopMovement();
            StartCoroutine(playInSeconds(pair.Key.breakObject, pair.Value.InitAngle / dissappearRate));

            // Make unpurchased items unpurchasable
            if (pair.Key != purchasedItem)
            {
                pair.Key.Price = int.MaxValue;
            }
        }
    }

    /// <summary>
    /// Executes an action after a delay.
    /// </summary>
    /// <param name="func">The action to execute</param>
    /// <param name="seconds">Delay in seconds</param>
    private IEnumerator playInSeconds(Action func, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        func?.Invoke();
    }
}
