using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

[CreateAssetMenu(fileName = "Spread Buff", menuName = "Ye Olde Shop/Spread Buff", order = 0)]
public class SpreadBuff : Buff
{
    public override void Initialize(CharacterClass characterClass)
    {
        var characterHandleWeapon = characterClass.Character.FindAbility<CharacterHandleWeapon>();
        if (characterHandleWeapon.CurrentWeapon is ProjectileWeapon)
        {
            var projectileWeapon = characterHandleWeapon.CurrentWeapon as ProjectileWeapon;

            projectileWeapon.ProjectilesPerShot = 3;
            projectileWeapon.Spread = new Vector3(0f, 0f, 5f);
        }
    }
}
