using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "Poison Buff", menuName = "Ye Olde Shop/Poison Buff", order = 0)]
public class PoisonBuff : Buff
{
    [field: SerializeField] public int poisonDamage { get; private set; } = 1;
    [field: SerializeField] public float poisonCooldown { get; private set; } = 0.2f;

    [field: SerializeField] public float poisonTime { get; private set; } = 1f;

    private CharacterClass characterClass;

    private Dictionary<Health, float> poisonEndTime;

    public override void Initialize(CharacterClass characterClass)
    {
        poisonEndTime = new Dictionary<Health, float>();
        this.characterClass = characterClass;
        characterClass.DamagedEnemy.AddListener((health) =>
        {
            if (!poisonEndTime.ContainsKey(health))
            {
                characterClass.StartCoroutine(DoPoison(health));
            }
        });
    }

    private IEnumerator DoPoison(Health health)
    {
        poisonEndTime[health] = Time.time + poisonTime;
        while (Time.time <= poisonEndTime[health])
        {
            DoDamage(health);
            yield return new WaitForSeconds(poisonCooldown);
        }
        poisonEndTime.Remove(health);
    }

    private void DoDamage(Health health)
    {
        bool damageEnabled = !health.Invulnerable;
        if (damageEnabled)
        {
            health.Damage(poisonDamage, characterClass.gameObject, 0, 0);
        }
        else
        {
            health.DamageEnabled();
            health.Damage(poisonDamage, characterClass.gameObject, 0, 0);
            health.DamageDisabled();
        }

    }
}
