using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Ye-Olde-Shop-Adventurer/Buff", order = 0)]
abstract public class Buff : UnlockableRewards
{
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public string buffName { get; private set; }
    [field: SerializeField] public string description { get; private set; }

    public abstract void Initialize(CharacterClass characterClass);
}
