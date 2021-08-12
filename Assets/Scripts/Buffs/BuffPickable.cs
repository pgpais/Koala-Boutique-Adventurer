using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class BuffPickable : PickableItem
{

    public Buff buffToGive;

    [SerializeField] bool canBePicked = true;
    [SerializeField] StringKey tooltipOverrideStringKey;
    [SerializeField] string tooltipOverride => Localisation.Get(tooltipOverrideStringKey);
    [SerializeField] bool isTooltipOverride = false;
    [SerializeField] BuffUI tooltip;
    [SerializeField] LayerMask playerDetectionLayer;
    [SerializeField] float toolTipRange = 3f;

    protected override void Start()
    {
        base.Start();
        if (buffToGive != null)
        {
            Model.GetComponentInChildren<SpriteRenderer>().sprite = buffToGive.icon;
        }
        tooltip.gameObject.SetActive(true);
        if (!isTooltipOverride)
        {
            tooltip.Initialize(buffToGive);
        }
        else
        {
            tooltip.Initialize(tooltipOverride);
        }
        tooltip.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (tooltip != null)
        {
            bool inRange = Physics2D.OverlapCircle(transform.position, toolTipRange, playerDetectionLayer);
            tooltip.gameObject.SetActive(inRange);
        }
    }

    protected override bool CheckIfPickable() => canBePicked && base.CheckIfPickable();

    protected override void Pick(GameObject picker)
    {

        LogsManager.SendLogDirectly(new Log(
            LogType.BuffCollected,
            new Dictionary<string, string>(){
                {"buff", buffToGive.name}
            }
        ));

        CharacterClass characterClass = picker.GetComponent<CharacterClass>();
        characterClass.AddBuff(buffToGive);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, toolTipRange);
    }
}
