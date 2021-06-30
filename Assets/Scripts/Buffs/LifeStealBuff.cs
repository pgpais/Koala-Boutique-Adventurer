using MoreMountains.TopDownEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "Life Steal Buff", menuName = "Ye Olde Shop/Buffs/LifeStealBuff", order = 0)]
public class LifeStealBuff : Buff
{
    [Range(0f, 1f)]
    [SerializeField] float LifeStealAmount = 0.15f;

    public override void Initialize(CharacterClass characterClass)
    {
        var characterHandleWeapon = characterClass.Character.FindAbility<CharacterHandleWeapon>();
        if (characterHandleWeapon.CurrentWeapon is MeleeWeapon)
        {
            MeleeWeapon weapon = characterHandleWeapon.CurrentWeapon as MeleeWeapon;

            characterClass.DamagedEnemy.AddListener((health) =>
            {
                characterClass.Character._health.GetHealth((int)(weapon.DamageCaused * LifeStealAmount), characterClass.gameObject);
            });
        }
    }
}