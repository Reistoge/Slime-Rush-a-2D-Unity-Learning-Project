using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFloor : MonoBehaviour
{
    public void triggerDangerZone(){
        
        GameManager.Instance.getLevelObjectManager().generateRandomDangerZoneLevel(this.transform);
    }
}
