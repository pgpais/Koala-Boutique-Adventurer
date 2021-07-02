using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class UnlockableCharacter : Character
{
    [SerializeField] UnlockableCharacterData data;

    public bool IsUnlocked() => data.Unlocked;
}
