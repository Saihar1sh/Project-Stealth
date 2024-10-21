using System.Reflection.Emit;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    
    [SerializeReference] public TrailRenderer tracerEffect;
    public float fireRate = 25f;
    public float bulletSpeed = 1000f;
    public float bulletDrop  = 0.0f;
    public float bulletLifeTime = 3.0f;

    public float maxRange = 100f;

    [Tooltip("required for animation")]public string weaponName;

    public BaseWeapon weaponPrefab;

    private const string WeaponAnimationNamePrefix = "equip_";
    
    public string GetWeaponAnimationName()
    {
        return WeaponAnimationNamePrefix + weaponName;
    }

}
public enum WeaponType
{
    None,
    Melee,
    HandGun,
    Rifle,
    Shotgun
    
}