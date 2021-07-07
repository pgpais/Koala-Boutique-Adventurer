using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFeedbackGroup : MonoBehaviour
{
    [SerializeField] LootFeedback lootFeedbackPrefab;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        InventoryManager.ItemAdded.AddListener((itemName, amount) =>
        {
            Item item = ItemManager.instance.itemsData.GetItemByName(itemName);

            Instantiate(lootFeedbackPrefab, transform).Initialize(itemName, amount, item.sprite);
        });
    }
}
