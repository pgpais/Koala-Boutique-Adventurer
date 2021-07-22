using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public static UnityEvent<string, int> ItemAdded = new UnityEvent<string, int>();
    public static UnityEvent ItemsAddedToGlobalInventory = new UnityEvent();

    public Dictionary<string, int> ItemQuantity => itemQuantity;
    public bool doubleDrops = false;


    [SerializeField] ItemsList itemsData;
    [SerializeField] int diseaseModifier = -10;

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
        MissionManager.MissionStarted.AddListener((_) => itemQuantity.Clear());
    }

    public void AddItem(string itemName, int amount)
    {
        if (itemsData.ContainsByName(itemName))
        {
            Item item = itemsData.GetItemByName(itemName);

            if (doubleDrops)
            {
                amount *= 2;
            }

            if (itemQuantity.ContainsKey(itemName))
            {
                itemQuantity[itemName] += amount;
                item.ItemUpdated.Invoke(itemQuantity[itemName]);
            }
            else
            {
                itemQuantity.Add(itemName, amount);
            }

            ItemAdded.Invoke(itemName, amount);
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

        if (QuestManager.instance != null && QuestManager.instance.enabled)
        {
            QuestManager.instance.TryToCompleteAdventurerQuest(itemQuantity);
        }

        if (DiseasedManager.instance != null && DiseasedManager.instance.enabled && DiseasedManager.instance.DiseasedItem != null)
        {
            HandleDiseasedItem(DiseasedManager.instance.DiseasedItem);
        }

        foreach (var itemName in itemQuantity.Keys)
        {
            int quantity = itemQuantity[itemName];
            Debug.Log($"{itemName}: {quantity}");
        }

        // TODO: remove this hardcode
        if (itemQuantity.ContainsKey("Gem"))
        {
            int gemAmount = itemQuantity["Gem"];
            GoldManager.GetGemsAndSendWithModifier(gemAmount);
            itemQuantity.Remove("Gem");
        }


        ItemManager.instance.AddItemsAfterGetting(itemQuantity);
        ItemsAddedToGlobalInventory.Invoke();
    }

    private void HandleDiseasedItem(Item diseasedItem)
    {
        string itemName = diseasedItem.ItemName;
        if (itemQuantity.ContainsKey(itemName))
        {
            int quantity = itemQuantity[itemName];
            itemQuantity.Remove(itemName);

            GoldManager.GetGoldSendWithModifier(diseaseModifier * quantity);
        }
    }
}

struct MissionStats
{
    //TODO: Info to log on the mission so it can be shown on the Manager's UI
}
