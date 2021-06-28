using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Buff", menuName = "Ye Olde Shop/Buffs/Stat Buff", order = 0)]
public class StatBuff : Buff
{

    public int movementSpeedModifier;
    public int healthModifier;
    public int damageModifier;

    public override void Initialize(CharacterClass characterClass)
    {
        characterClass.ModifyMovementSpeed(movementSpeedModifier);
        characterClass.ModifyMaximumHealth(healthModifier);
        characterClass.ModifyWeaponDamage(damageModifier);
    }
}
