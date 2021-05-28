using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;

    [Space]
    [SerializeField] Transform shootingPoint;
    [SerializeField] float shotVelocity = 7f;
    [SerializeField] float shotCooldown = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, shootingPoint.right, range, targetLayer);
        // if (hit.collider != null)
        // {
        //     Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation).Init(projectileInitialVelocity);
        //     Debug.Log("Shot an arrow!");
        //     nextShotTime = Time.time + shotCooldown;
        // }
    }
}
