using System.Collections;
using System.Collections.Generic;
using Cheese;
using UnityEngine;

[CreateAssetMenu(fileName = "Iron Boots Buff", menuName = "Ye Olde Shop/Buffs/IronBootsBuff", order = 0)]
public class IronBootsBuff : Buff
{
    public override void Initialize(CharacterClass characterClass)
    {
        Health health = characterClass.GetComponent<Health>();
        health.hasIronBoots = true;
    }
}
