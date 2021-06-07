using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class BuffPickable : PickableItem
{

    public Buff buffToGive;

    [SerializeField] BuffUI tooltip;
    [SerializeField] LayerMask playerDetectionLayer;
    [SerializeField] float toolTipRange = 3f;

    protected override void Start()
    {
        base.Start();
        Model.GetComponent<SpriteRenderer>().sprite = buffToGive.icon;
        tooltip.gameObject.SetActive(false);
        tooltip.Initialize(buffToGive);
    }

    private void FixedUpdate()
    {
        if (tooltip != null)
        {
            bool inRange = Physics2D.OverlapCircle(transform.position, toolTipRange, playerDetectionLayer);
            tooltip.gameObject.SetActive(inRange);
        }
    }

    protected override void Pick(GameObject picker)
    {
        CharacterClass characterClass = picker.GetComponent<CharacterClass>();
        characterClass.AddBuff(buffToGive);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, toolTipRange);
    }
}
