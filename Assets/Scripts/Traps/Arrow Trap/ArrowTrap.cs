using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] Arrow arrowPrefab;

    [SerializeField] float projectileInitialVelocity = 5f;
    [SerializeField] float shotCooldown = 0.5f;

    [SerializeField] Transform shootingPoint;
    [SerializeField] float range;
    [SerializeField] LayerMask targetLayer;

    private float nextShotTime;

    private void Awake()
    {
        nextShotTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextShotTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, shootingPoint.right, range, targetLayer);
            if (hit.collider != null)
            {
                Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation).Init(projectileInitialVelocity);
                Debug.Log("Shot an arrow!");
                nextShotTime = Time.time + shotCooldown;
            }
        }
    }
}
