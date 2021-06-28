using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class MeleeWeaponWithClass : MeleeWeaponFixedAim
{
    private int oldDamage;

    public override void Initialization()
    {
        base.Initialization();

        oldDamage = _damageOnTouch.DamageCaused;

        if (Owner != null)
        {
            var characterClass = Owner.GetComponent<CharacterClass>();
            if (characterClass != null)
            {
                // TODO: Check if correctly applied
                _damageOnTouch.DamageCaused = oldDamage;
                characterClass.ApplyDamageModifier(_damageOnTouch, false);
            }
        }
    }

    protected override void CreateDamageArea()
    {
        _damageArea = new GameObject();
        _damageArea.name = this.name + "DamageArea";
        _damageArea.transform.position = this.transform.position;
        _damageArea.transform.rotation = this.transform.rotation;
        _damageArea.transform.SetParent(this.transform);
        _damageArea.layer = this.gameObject.layer;

        if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
        {
            _boxCollider2D = _damageArea.AddComponent<BoxCollider2D>();
            _boxCollider2D.offset = AreaOffset;
            _boxCollider2D.size = AreaSize;
            _damageAreaCollider2D = _boxCollider2D;
            _damageAreaCollider2D.isTrigger = true;
        }
        if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
        {
            _circleCollider2D = _damageArea.AddComponent<CircleCollider2D>();
            _circleCollider2D.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
            _circleCollider2D.radius = AreaSize.x / 2;
            _damageAreaCollider2D = _circleCollider2D;
            _damageAreaCollider2D.isTrigger = true;
        }

        if ((DamageAreaShape == MeleeDamageAreaShapes.Rectangle) || (DamageAreaShape == MeleeDamageAreaShapes.Circle))
        {
            Rigidbody2D rigidBody = _damageArea.AddComponent<Rigidbody2D>();
            rigidBody.isKinematic = true;
            rigidBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        if (DamageAreaShape == MeleeDamageAreaShapes.Box)
        {
            _boxCollider = _damageArea.AddComponent<BoxCollider>();
            _boxCollider.center = AreaOffset;
            _boxCollider.size = AreaSize;
            _damageAreaCollider = _boxCollider;
            _damageAreaCollider.isTrigger = true;
        }
        if (DamageAreaShape == MeleeDamageAreaShapes.Sphere)
        {
            _sphereCollider = _damageArea.AddComponent<SphereCollider>();
            _sphereCollider.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
            _sphereCollider.radius = AreaSize.x / 2;
            _damageAreaCollider = _sphereCollider;
            _damageAreaCollider.isTrigger = true;
        }

        if ((DamageAreaShape == MeleeDamageAreaShapes.Box) || (DamageAreaShape == MeleeDamageAreaShapes.Sphere))
        {
            Rigidbody rigidBody = _damageArea.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }

        _damageOnTouch = _damageArea.AddComponent<DamageOnTouchWithEvents>();
        _damageOnTouch.SetGizmoSize(AreaSize);
        _damageOnTouch.SetGizmoOffset(AreaOffset);
        _damageOnTouch.TargetLayerMask = TargetLayerMask;
        _damageOnTouch.DamageCaused = DamageCaused;
        _damageOnTouch.DamageCausedKnockbackType = Knockback;
        _damageOnTouch.DamageCausedKnockbackForce = KnockbackForce;
        _damageOnTouch.InvincibilityDuration = InvincibilityDuration;
        _damageOnTouch.HitDamageableFeedback = HitDamageableFeedback;
        _damageOnTouch.HitNonDamageableFeedback = HitNonDamageableFeedback;

        if (!CanDamageOwner && (Owner != null))
        {
            _damageOnTouch.IgnoreGameObject(Owner.gameObject);
        }
    }
}
