using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFloor : MonoBehaviour
{
    public void triggerDangerZone(){
        
        GameManager.instance.getLevelObjectManager().generateRandomDangerZoneLevel(this.transform);
    }
}
