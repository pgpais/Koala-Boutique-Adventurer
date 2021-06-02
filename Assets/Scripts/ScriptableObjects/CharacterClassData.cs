using MoreMountains.TopDownEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "Ye-Olde-Shop-Adventurer/Class Data", order = 0)]
public class CharacterClassData : ScriptableObject
{
    [Tooltip("How much health this class adds (or subtracts) to base value")]
    public int healthModifier = 0;

    [Tooltip("How much damage this class adds (or subtracts) to base value")]
    public int attackDamageModifier = 0;

    [Tooltip("How much movement speed this class adds (or subtracts) to base value")]
    public float movementSpeedModifier = 0;

    public Weapon initialWeapon;
}
