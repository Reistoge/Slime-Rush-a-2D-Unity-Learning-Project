using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesAnimHandler : MonoBehaviour
{
    [SerializeField] Spikes spike;
    


    public void desactivateCollider(){
        spike.desactivateCollider();
    }
    public void activateCollider(){
        spike.activateCollider();
    }
}
