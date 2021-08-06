using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static string newItemsReferenceName = "newItems";
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

        int diseasedGoldLoss = 0;

        if (DiseasedManager.instance != null && DiseasedManager.instance.enabled && DiseasedManager.instance.DiseasedItem != null)
        {
            diseasedGoldLoss = HandleDiseasedItem(DiseasedManager.instance.DiseasedItem);
        }

        AddToNewItemsLog(diseasedGoldLoss);

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

    private void AddToNewItemsLog(int diseasedGoldLoss)
    {
        NewItemsList currentNewItemsList = new NewItemsList(diseasedGoldLoss, itemQuantity, DiseasedManager.instance.DiseasedItem.ItemName);

        GetNewItemsListAndAddCurrent(currentNewItemsList);
    }

    private void GetNewItemsListAndAddCurrent(NewItemsList currentNewItemsList)
    {
        FirebaseCommunicator.instance.GetObject(newItemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error getting new items list: " + task.Exception.InnerException.Message);
            }
            else
            {
                NewItemsList newItemsListToSend;
                string json = task.Result.GetRawJsonValue();
                if (string.IsNullOrEmpty(json))
                {
                    newItemsListToSend = currentNewItemsList;
                }
                else
                {
                    NewItemsList oldNewItemsList = JsonConvert.DeserializeObject<NewItemsList>(json);
                    newItemsListToSend = currentNewItemsList.Merge(oldNewItemsList);
                }

                SendNewItemsList(newItemsListToSend);
            }
        });
    }

    private void SendNewItemsList(NewItemsList newItemsListToSend)
    {
        string json = JsonConvert.SerializeObject(newItemsListToSend);
        FirebaseCommunicator.instance.SendObject(json, newItemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error sending new items list: " + task.Exception.InnerException.Message);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("New items list sent!");
            }
        });
    }

    private int HandleDiseasedItem(Item diseasedItem)
    {
        string itemName = diseasedItem.ItemName;
        if (itemQuantity.ContainsKey(itemName))
        {
            int quantity = itemQuantity[itemName];
            itemQuantity.Remove(itemName);

            int diseasedGoldLoss = diseaseModifier * quantity;
            GoldManager.GetGoldSendWithModifier(diseasedGoldLoss);
            return diseasedGoldLoss;
        }

        return 0;
    }



    class NewItemsList
    {
        public int diseasedGoldLoss;
        public string diseasedItemName;
        public Dictionary<string, int> lootedItems;

        public NewItemsList(int diseasedGoldLoss, Dictionary<string, int> lootedItems, string diseasedItemName)
        {
            this.diseasedGoldLoss = diseasedGoldLoss;
            this.lootedItems = lootedItems;
            this.diseasedItemName = diseasedItemName;
        }

        internal NewItemsList Merge(NewItemsList oldNewItemsList)
        {
            diseasedGoldLoss += oldNewItemsList.diseasedGoldLoss;
            foreach (var lootedItem in oldNewItemsList.lootedItems)
            {
                if (lootedItems.ContainsKey(lootedItem.Key))
                {
                    lootedItems[lootedItem.Key] += lootedItem.Value;
                }
                else
                {
                    lootedItems.Add(lootedItem.Key, lootedItem.Value);
                }
            }
            return this;
        }
    }

    // yes it's disgusting. don't care.
    bool wasHalved = false;
    internal void HalfInventory()
    {
        if (!wasHalved)
        {
            List<string> keys = itemQuantity.Keys.ToList();
            foreach (var itemName in keys)
            {
                int quantity = itemQuantity[itemName];
                quantity /= 2;
                itemQuantity[itemName] = quantity;
            }
            wasHalved = true;
        }
    }

    struct MissionStats
    {
        //TODO: Info to log on the mission so it can be shown on the Manager's UI
    }
}
