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
    [SerializeField] LayerMask hitLayer;


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
            RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, shootingPoint.right, range, hitLayer);
            var gameObj = hit.collider.gameObject;
            if (hit.collider != null && gameObj.CompareTag("Player"))
            {
                Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation).Init(projectileInitialVelocity);
                Debug.Log("Shot an arrow!");
                nextShotTime = Time.time + shotCooldown;
            }
        }
    }
}