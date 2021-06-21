using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public static UnityEvent<string> ItemAdded = new UnityEvent<string>();

    public Dictionary<string, int> ItemQuantity => itemQuantity;


    [SerializeField] ItemsList itemsData;

    private Dictionary<string, int> itemQuantity;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        itemQuantity = new Dictionary<string, int>();
    }

    public void AddItem(string itemName, int amount)
    {
        if (itemsData.ContainsByName(itemName))
        {
            Item item = itemsData.GetItemByName(itemName);

            if (itemQuantity.ContainsKey(itemName))
            {
                itemQuantity[itemName] += amount;
                item.ItemUpdated.Invoke(itemQuantity[itemName]);
            }
            else
            {
                itemQuantity.Add(itemName, amount);
            }

            ItemAdded.Invoke(itemName);
            Debug.Log("Added item " + itemName + "!");
        }
    }

    public void AddRandomValuable()
    {
        Item item = ItemManager.instance.GetRandomValuable();
        AddItem(item.ItemName, 1);
    }

    public void AddInventoryToGlobalItems()
    {
        Debug.Log("inventory > global");
        Debug.Log("Local inventory:");
        foreach (var itemName in itemQuantity.Keys)
        {
            int quantity = itemQuantity[itemName];
            Debug.Log($"{itemName}: {quantity}");
        }
        ItemManager.instance.AddItemsAfterGetting(itemQuantity);

    }
}
