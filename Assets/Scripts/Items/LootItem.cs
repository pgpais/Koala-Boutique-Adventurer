using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class LootItem : PickableItem
{
    public Item Item => item;
    [SerializeField] Item item;
    [SerializeField] int amount = 1;

    // private void Start()
    // {
    // }

    protected override void Pick(GameObject picker)
    {
        base.Pick(picker);
        InventoryManager.instance.AddItem(item.ItemNameKey, amount);

        LogsManager.SendLogDirectly(new Log(
            LogType.LootCollected,
            new Dictionary<string, string>(){
                {"Item", item.ItemNameKey},
                {"Amount", amount.ToString()}
            }
        ));

        Destroy(gameObject);
    }
}
