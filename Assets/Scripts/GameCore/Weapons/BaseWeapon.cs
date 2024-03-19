using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    
    
    [SerializeField] private ParticleSystem muzzleFlashParticle;
    [SerializeField] private ParticleSystem hitEffectParticle;
    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastDestination;
    
    [SerializeField] private float fireRate = 25f;
    [SerializeField] private float bulletSpeed = 1000f;
    [SerializeField] private float bulletDrop  = 0.0f;
    
    public bool isFiring { get; private set; } = false;
        
    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;
    
    private List<Bullet> bullets = new List<Bullet>();
    private float maxLifetime = 3.0f;

    private Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        Vector3 distance = bullet.initialVelocity * (bullet.time) + gravity * (0.5f * (bullet.time) * (bullet.time));
        return bullet.initialPosition + distance;
    }
    
    private Bullet CreateBullet(Vector3 initialPosition, Vector3 initialVelocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = initialPosition;
        bullet.initialVelocity = initialVelocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(tracerEffect, initialPosition,Quaternion.identity);
        bullet.tracer.AddPosition(initialPosition);
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0f;
        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1f / fireRate;
        while (accumulatedTime >= 0)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }
    
    private void FireBullet()
    {
        muzzleFlashParticle.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        Vector3 initialPosition = raycastOrigin.position;
        Bullet bullet = CreateBullet(initialPosition, velocity);
        bullets.Add(bullet);

    }

    public void UpdateBulletsSimilation(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestoryBullets();
    }

    private void DestoryBullets()
    {
        bullets.RemoveAll(bullet => bullet.time > maxLifetime);
    }

    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0= GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(bullet, p0, p1);
        });
    }

    private void RaycastSegment(Bullet bullet, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        
        if (Physics.Raycast(ray, out hitInfo,distance))
        {
            hitEffectParticle.transform.position = hitInfo.point;
            hitEffectParticle.transform.forward = hitInfo.normal;
            hitEffectParticle.Emit(1);
            
            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifetime;
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }
    
    
    public void StopFiring()
    {
        isFiring = false;
    }
}
