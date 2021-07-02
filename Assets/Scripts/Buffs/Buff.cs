using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Ye Olde Shop/Buff", order = 0)]
abstract public class Buff : ScriptableObject, UnlockableReward
{
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public string buffName { get; private set; }
    [field: SerializeField] public string description { get; private set; }
    [field: SerializeField] public bool StartsUnlocked { get; private set; } = false;
    public bool AlreadySpawned = false;

    public bool Unlocked => unlocked;
    private bool unlocked = false;

    public abstract void Initialize(CharacterClass characterClass);

    public void Unlock()
    {
        unlocked = true;
    }

    private void OnEnable()
    {
        unlocked = StartsUnlocked;
        AlreadySpawned = false;
    }
}
