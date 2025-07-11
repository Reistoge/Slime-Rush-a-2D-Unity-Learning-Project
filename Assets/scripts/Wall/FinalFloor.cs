using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFloor : MonoBehaviour
{
    Coroutine coroutine;
    [SerializeField] float offset = 340f;


    public void triggerDangerZone()
    {
        GameManager.Instance.getLevelObjectManager().generateRandomDangerZoneLevel(this.transform, offset);

    }
 
}
