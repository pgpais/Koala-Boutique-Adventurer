using MoreMountains.TopDownEngine;
using UnityEngine;

public class DamageOnTouchWithEvents : DamageOnTouch
{
    CharacterClass characterClass;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void OnCollideWithDamageable(Health health)
    {
        if (characterClass == null)
        {
            characterClass = Owner.GetComponent<CharacterClass>();
        }
        if (!health.Invulnerable)
        {
            characterClass.DamagedEnemy.Invoke(health);
        }

        base.OnCollideWithDamageable(health);
    }
}