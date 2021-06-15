using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffList", menuName = "Ye Olde Shop/BuffList", order = 0)]
public class BuffList : ScriptableObject
{
    [field: SerializeField] public List<Buff> buffs { get; private set; }

    public List<Buff> GetUnlockedBuffs() => buffs.FindAll((buff) => buff.Unlocked);

    public Buff GetBuffByName(string buffName) => buffs.First((buff) => string.Equals(buffName, buff.buffName));
}
