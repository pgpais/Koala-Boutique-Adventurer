using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFeedbackGroup : MonoBehaviour
{
    [SerializeField] LootFeedback lootFeedbackPrefab;
    [SerializeField] LootFeedback firstTimeLootFeedbackPrefab;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        InventoryManager.ItemAdded.AddListener((itemName, amount) =>
        {
            Item item = ItemManager.instance.itemsData.GetItemByName(itemName);

            if (IsFirstTimeItem(item))
            {
                var lootFeedback = Instantiate(firstTimeLootFeedbackPrefab, transform);
                lootFeedback.Initialize(item.ItemName, amount, item.sprite);
                lootFeedback.transform.SetParent(transform, false);

            }
            else
            {
                var lootFeedback = Instantiate(lootFeedbackPrefab);
                lootFeedback.Initialize(item.ItemName, amount, item.sprite);
                lootFeedback.transform.SetParent(transform, false);
            }
        });
    }

    bool IsFirstTimeItem(Item item)
    {
        return !NewItemsManager.instance.HasSeenItemBefore(item);
    }
}
