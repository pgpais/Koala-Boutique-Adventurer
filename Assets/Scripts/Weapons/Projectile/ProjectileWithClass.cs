using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class ProjectileWithClass : Projectile
{
    private int oldDamage;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        oldDamage = _damageOnTouch.DamageCaused;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void SetOwner(GameObject newOwner)
    {
        _damageOnTouch.DamageCaused = oldDamage;

        base.SetOwner(newOwner);

        var characterClass = newOwner.GetComponent<CharacterClass>();
        if (characterClass != null)
        {
            characterClass.ApplyDamageModifier(_damageOnTouch, true);
        }
    }

    public override void Movement()
    {
        base.Movement();
        transform.right = Direction;
    }
}
