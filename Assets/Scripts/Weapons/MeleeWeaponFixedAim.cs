using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class MeleeWeaponFixedAim : MeleeWeapon
{
    protected override IEnumerator MeleeWeaponAttack()
    {
        // Debug.Log("Disabling aim", _aimableWeapon);
        // _aimableWeapon.enabled = false;
        yield return base.MeleeWeaponAttack();
        // _aimableWeapon.enabled = true;
        // Debug.Log("Enabling aim", _aimableWeapon);

    }
}
