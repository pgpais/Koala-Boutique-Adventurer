using System.Collections;
using System.Collections.Generic;
using Cheese;
using UnityEngine;

[CreateAssetMenu(fileName = "Deflecting Armor Buff", menuName = "Ye Olde Shop/Buffs/DeflectingArmorBuff", order = 0)]
public class DeflectingArmorBuff : Buff
{
    public override void Initialize(CharacterClass characterClass)
    {
        Health health = characterClass.GetComponent<Health>();
        health.hasDeflectingArmor = true;
    }
}
