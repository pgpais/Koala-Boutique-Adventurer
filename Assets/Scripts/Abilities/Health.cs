using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cheese
{
    public class Health : MoreMountains.TopDownEngine.Health
    {

        public bool hasIronBoots = false;
        public bool hasDeflectingArmor = false;

        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
        {
            if (instigator.CompareTag("Turret Shot") && hasDeflectingArmor && Random.Range(0, 1f) < 0.5f)
                return;

            if (instigator.CompareTag("Spike Trap") && hasIronBoots)
                return;

            base.Damage(damage, instigator, flickerDuration, invincibilityDuration);
        }
    }
}
