using System;
using Gameplay.Player;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerAiming player = other.GetComponent<PlayerAiming>();
        if (player)
        {
            BaseWeapon weapon = Instantiate(weaponData.weaponPrefab);
            weapon.InitValues(weaponData);
            player.EquipWeapon(weapon,weaponData);
        }
    }
}
