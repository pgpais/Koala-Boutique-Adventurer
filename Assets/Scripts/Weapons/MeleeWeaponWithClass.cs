using System.Collections;
using System.Collections.Generic;
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
                characterClass.AddModifier(_damageOnTouch, false);
            }
        }
    }
}
