using MoreMountains.TopDownEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "Ye-Olde-Shop-Adventurer/Class Data", order = 0)]
public class CharacterClassData : ScriptableObject
{
    [Tooltip("How much health this class adds (or subtracts) to base value")]
    public int healthModifier = 0;

    [Tooltip("How much melee damage this class adds (or subtracts) to base value")]
    public int meleeAttackDamageModifier = 0;

    [Tooltip("How much melee damage this class adds (or subtracts) to base value")]
    public int rangedAttackDamageModifier = 0;

    [Tooltip("The multiplayer for base movement speed from this class")]
    public float movementSpeedMultiplier = 0;

    public Weapon initialWeapon;
}
