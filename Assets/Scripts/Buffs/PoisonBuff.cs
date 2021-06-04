using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PoisonBuff : Buff
{
    [field: SerializeField] public float poisonCooldown { get; private set; } = 0.2f;

    private IEnumerator DoPoison(Health health)
    {
        // TODO: do loop
        DoDamage(health);
        yield return new WaitForSeconds(poisonCooldown);
    }

    private void DoDamage(Health health)
    {
        throw new NotImplementedException();
    }
}
