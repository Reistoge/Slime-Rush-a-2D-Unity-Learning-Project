using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualEffectHandler : MonoBehaviour
{
    // now add the 
    [SerializeField]  GameObject particleEffectPrefab; // Prefab for the particle effect
    ParticleSystem[] particles;
    [SerializeField] private bool destroyAfterEffect = true; // Auto-destroy after playing the effect
    void Start(){
        if(particleEffectPrefab.activeInHierarchy==false){
            particles = Instantiate(particleEffectPrefab,transform.parent.transform.parent).GetComponentsInChildren<ParticleSystem>();
        }
    }
    public void TriggerEffect(Vector3 position)
    {   
    
        if (particleEffectPrefab == null) return;
        foreach (ParticleSystem particle in particles){
            particle.Play();
        }
 
    }
 
}
