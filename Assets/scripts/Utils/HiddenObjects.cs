using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    { 
        foreach (Transform child in transform)
        {
            GameManager.Instance.instantiateAppearEffect(child, 1);
            child.gameObject.SetActive(true);
        }
    }

}
