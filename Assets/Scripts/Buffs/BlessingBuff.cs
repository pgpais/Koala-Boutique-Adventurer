using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "Blessing Buff", menuName = "Ye Olde Shop/Buffs/BlessingBuff", order = 0)]
public class BlessingBuff : Buff
{
    [SerializeField] float healthModifier = 0.75f;

    public override void Initialize(CharacterClass characterClass)
    {
        var enemies = FindObjectsOfType<AIBrain>();
        foreach (var enemy in enemies)
        {
            var health = enemy.GetComponent<Health>();
            health.MaximumHealth = (int)(health.MaximumHealth * healthModifier);
            health.ResetHealthToMaxHealth();
        }
    }
}