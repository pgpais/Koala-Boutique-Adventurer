using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class InventoryHUD : MonoBehaviour
{
    [SerializeField] ItemHUD itemHUDPrefab;

    private List<ItemHUD> itemHUDs = new List<ItemHUD>();

    private void Awake()
    {
        // Destroy every child object
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        InventoryManager.ItemAdded.AddListener(OnItemLooted);
    }

    private void OnDisable()
    {
        InventoryManager.ItemAdded.RemoveListener(OnItemLooted);
    }

    void OnItemLooted(string itemName, int quantity)
    {
        Item item = ItemManager.instance.itemsData.GetItemByName(itemName);

        ItemHUD itemHUD = itemHUDs.Find((itemHUD) => itemHUD.ItemName == item.ItemName);
        if (itemHUD != null)
        {
            itemHUD.UpdateQuantity(quantity);
        }
        else
        {
            itemHUD = Instantiate(itemHUDPrefab);
            itemHUD.transform.SetParent(transform, false);
            itemHUD.Init(item, quantity);
            itemHUDs.Add(itemHUD);
        }
    }
}
