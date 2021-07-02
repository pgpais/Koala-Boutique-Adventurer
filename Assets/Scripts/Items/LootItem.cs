using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class LootItem : PickableItem
{
    public Item Item => item;
    [SerializeField] Item item;

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
