using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableRewards : ScriptableObject
{
    public bool Unlocked => unlocked;
    private bool unlocked;
    [SerializeField] bool startsUnlocked = false;

    private void OnEnable()
    {
        unlocked = startsUnlocked;
    }

    public void Unlock()
    {
        unlocked = true;
    }
}
