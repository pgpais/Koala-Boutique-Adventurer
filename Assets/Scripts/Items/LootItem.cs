using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class LootItem : PickableItem
{
    [SerializeField] Item item;

    SpriteRenderer spriteRen;

    private void Awake()
    {
        spriteRen = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        if (spriteRen != null)
        {
            spriteRen.sprite = item.sprite;
        }
    }

    // private void Start()
    // {
    // }

    protected override void Pick(GameObject picker)
    {
        base.Pick(picker);
        InventoryManager.instance.AddItem(item.ItemName, 1);

        Destroy(gameObject);
    }
}
