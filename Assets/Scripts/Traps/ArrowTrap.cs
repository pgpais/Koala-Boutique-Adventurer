using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] Transform shootingPoint;
    [SerializeField] float range;
    [SerializeField] LayerMask targetLayer;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, shootingPoint.right, range, targetLayer);
        if (hit.collider != null)
        {
            // hit.collider.GetComponent<
            Debug.Log("Shot an arrow!");
        }
    }
}
