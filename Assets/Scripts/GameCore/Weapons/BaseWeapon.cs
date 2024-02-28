using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem muzzleFlashParticle;
    [SerializeField] private ParticleSystem hitEffectParticle;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastDestination;
    
    
    private bool isFiring = false;

    private Ray ray;
    private RaycastHit hitInfo;
    
    public void StartFiring()
    {
        isFiring = true;
        
        muzzleFlashParticle.Emit(1);
        
        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;
        if (Physics.Raycast(ray,out hitInfo))
        {
            hitEffectParticle.transform.position = hitInfo.point;
            hitEffectParticle.transform.forward = hitInfo.normal;
            hitEffectParticle.Emit(1);
            //Debug.DrawLine(ray.origin,hitInfo.point,Color.red,1f);
        }
    }
    public void StopFiring()
    {
        isFiring = false;
    }
}
