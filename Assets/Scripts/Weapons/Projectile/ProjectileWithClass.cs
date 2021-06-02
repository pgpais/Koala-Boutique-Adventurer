using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class ProjectileWithClass : Projectile
{
    private int oldDamage;

    protected override void Awake()
    {
        base.Awake();
        oldDamage = _damageOnTouch.DamageCaused;
    }

    public override void SetOwner(GameObject newOwner)
    {
        _damageOnTouch.DamageCaused = oldDamage;

        base.SetOwner(newOwner);

        var characterClass = newOwner.GetComponent<CharacterClass>();
        if (characterClass != null)
        {
            characterClass.AddModifier(_damageOnTouch);
        }
    }
}
