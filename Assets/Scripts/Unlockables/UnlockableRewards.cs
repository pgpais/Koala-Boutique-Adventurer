using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UnlockableReward
{
    bool Unlocked { get; }

    void Unlock();
}
