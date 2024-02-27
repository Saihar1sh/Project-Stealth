using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> muzzleFlashParticles;
    [SerializeField] private Transform raycastOrigin;
    
    
    private bool isFiring = false;

    private Ray ray;
    private RaycastHit hitInfo;
    
    public void StartFiring()
    {
        isFiring = true;
        foreach (var particle in muzzleFlashParticles)
        {
            particle.Emit(1);
        }
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;
        if (Physics.Raycast(ray,out hitInfo))
        {
            Debug.DrawLine(ray.origin,hitInfo.point,Color.red,1f);
        }
    }
    public void StopFiring()
    {
        isFiring = false;
    }
}
