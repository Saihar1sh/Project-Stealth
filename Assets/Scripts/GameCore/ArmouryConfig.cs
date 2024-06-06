using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmouryConfig", menuName = "Scriptable Objects/ArmouryConfig")]
public class ArmouryConfig : ScriptableObject
{
    public List<WeaponData> weapons;
}
