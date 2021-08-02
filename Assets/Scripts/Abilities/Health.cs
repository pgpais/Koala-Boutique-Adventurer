using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace Cheese
{
    public class Health : MoreMountains.TopDownEngine.Health
    {

        public bool hasIronBoots = false;
        public bool hasDeflectingArmor = false;

        [SerializeField] MMFeedbacks DOTFeedbacks;


        // public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration, Vector3 damageDirection)
        {
            if (instigator.CompareTag("Turret Shot") && hasDeflectingArmor && Random.Range(0, 1f) < 0.5f)
                return;

            if (instigator.CompareTag("Spike Trap") && hasIronBoots)
                return;

            if (Invulnerable)
            {
                return;
            }

            // if we're already below zero, we do nothing and exit
            if ((CurrentHealth <= 0) && (InitialHealth != 0))
            {
                return;
            }

            // we decrease the character's health by the damage
            float previousHealth = CurrentHealth;
            CurrentHealth -= damage;

            if (OnHit != null)
            {
                OnHit();
            }

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            // we prevent the character from colliding with Projectiles, Player and Enemies
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // we trigger a damage taken event
            MMDamageTakenEvent.Trigger(_character, instigator, CurrentHealth, damage, previousHealth);

            if (_animator != null)
            {
                _animator.SetTrigger("Damage");
            }

            bool isDoT = invincibilityDuration == 0; // this is not a good definer, but it works
            if (isDoT)
            {
                DOTFeedbacks?.PlayFeedbacks(this.transform.position);
            }
            else
            {
                DamageMMFeedbacks?.PlayFeedbacks(this.transform.position);
            }

            // we update the health bar
            UpdateHealthBar(true);

            // if health has reached zero
            if (CurrentHealth <= 0)
            {
                // we set its health to zero (useful for the healthbar)
                CurrentHealth = 0;

                Kill();
            }
        }
    }
}
