using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealingZone : MonoBehaviour
{
    [SerializeField] int healingAmount = 1;
    [SerializeField] float timeBetweenHeals = 0.2f;

    Health targetHealth;
    float nextHealTime;

    private void Start()
    {
        nextHealTime = Time.time;
    }

    void HealTarget()
    {
        if (Time.time >= nextHealTime)
        {
            targetHealth.GetHealth(healingAmount, gameObject);
            nextHealTime = Time.time + timeBetweenHeals;
        }
    }

    private void Update()
    {
        if (targetHealth != null)
            HealTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetHealth == null)
        {
            targetHealth = other.GetComponent<Health>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (targetHealth == null)
        {
            targetHealth = other.GetComponent<Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        targetHealth = null;
    }
}
