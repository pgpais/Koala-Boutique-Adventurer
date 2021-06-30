using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lucky Charm Buff", menuName = "Ye Olde Shop/Buffs/LuckyCharmBuff", order = 0)]
public class LuckyCharmBuff : Buff
{
    public override void Initialize(CharacterClass characterClass)
    {
        InventoryManager.instance.doubleDrops = true;
    }
}
