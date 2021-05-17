using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public static UnityEvent<string> ItemAdded = new UnityEvent<string>();

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

    public void AddRandomItem()
    {
        Item item = ItemManager.instance.GetRandomItem();
        AddItem(item.ItemName, 1);
    }

    public void AddInventoryToGlobalItems()
    {
        ItemManager.instance.AddItemsAfterGetting(itemQuantity);
    }
}
