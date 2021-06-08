using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffList", menuName = "Ye Olde Shop/BuffList", order = 0)]
public class BuffList : ScriptableObject
{
    [field: SerializeField] public List<Buff> buffs { get; private set; }

    public List<Buff> GetUnlockedBuffs() => buffs.FindAll((buff) => buff.Unlocked);

}
