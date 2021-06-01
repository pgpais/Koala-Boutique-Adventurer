using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class MeleeWeaponFixedAim : MeleeWeapon
{
    public override void WeaponUse()
    {
        _aimableWeapon.enabled = false;
        base.WeaponUse();
        _aimableWeapon.enabled = true;
    }
}
