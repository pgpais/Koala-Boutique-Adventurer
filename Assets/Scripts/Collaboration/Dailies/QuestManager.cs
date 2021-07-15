using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private const int managerQuestNumberOfItems = 4;
    private const int managerQuestSellPerItem = 4;
    private const string dateFormat = "yyyyMMdd";
    public static string adventureReferenceName = "adventurerQuest";
    public static string managerReferenceName = "managerQuest";

    public static QuestManager instance;

    AdventurerQuest adventurerQuest;
    ManagerQuest managerQuest;
    DateTime questDate;

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

        FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
    }

    void OnLoggedIn()
    {
        GetAdventurerQuest();
        GetManagerQuest();

        MissionManager.MissionStarted.AddListener(OnMissionStarted);
        // MissionManager.MissionEnded.AddListener(OnMissionEnd);
    }

    void GetAdventurerQuest()
    {
        FirebaseCommunicator.instance.GetObject(adventureReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting daily quest! message: " + task.Exception.Message);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("yey got daily quest!");

                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("Failed getting daily quest! message: " + task.Exception.Message);
                }
                else
                {
                    adventurerQuest = JsonConvert.DeserializeObject<AdventurerQuest>(json);

                    questDate = DateTime.ParseExact(adventurerQuest.StartDay, dateFormat, null);
                }
            }
        });
    }

    void GetManagerQuest()
    {
        FirebaseCommunicator.instance.GetObject(managerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting manager quest! message: " + task.Exception.Message);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("yey got manager quest!");
                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("Failed getting manager quest! message: " + task.Exception.Message);
                }
                else
                {
                    managerQuest = JsonConvert.DeserializeObject<ManagerQuest>(json);
                }
            }
        });
    }

    private void Update()
    {
        int daysSinceQuest = (DateTime.Now - questDate).Days;
        if (adventurerQuest != null && daysSinceQuest > 0)
        {
            GenerateNewQuest();
        }
    }

    void GenerateNewQuest()
    {
        string questItem = ItemManager.instance.itemsData.GetRandomUnlockedItem().ItemName;
        int amount = 5;
        int goldReward = 100;
        string dateTimeString = DateTime.Now.ToString(dateFormat);

        adventurerQuest = new AdventurerQuest(questItem, amount, goldReward, dateTimeString);

        string json = JsonConvert.SerializeObject(adventurerQuest);
        FirebaseCommunicator.instance.SendObject(json, adventureReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting daily quest! message: " + task.Exception.Message);
                // set daily quest date to the past so we get again next time
                questDate = DateTime.Now.AddDays(-1);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("CLOUD: update daily quest!");
            }
        });
    }

    void OnMissionStarted(float time)
    {
        InventoryManager.ItemsAddedToGlobalInventory.AddListener(OnItemsAdded);

        if (managerQuest == null)
            CreateManagerQuest();
    }

    //todo: this is a hack, we should be able to check the quest from an event
    public void OnMissionEnd()
    {
        managerQuest.Check();

        SendManagerQuest();
    }

    private void OnItemsAdded()
    {
        foreach (var item in InventoryManager.instance.ItemQuantity.Keys)
        {
            if (adventurerQuest.CanCompleteQuest(item, InventoryManager.instance.ItemQuantity[item]))
            {
                adventurerQuest.CompleteQuest();
            }
        }
        InventoryManager.ItemsAddedToGlobalInventory.RemoveListener(OnItemsAdded);
    }

    void CreateManagerQuest()
    {
        Dictionary<string, int> items = new Dictionary<string, int>();
        string dateTimeString = DateTime.Now.ToString(dateFormat);

        for (int i = 0; i < managerQuestNumberOfItems; i++)
        {
            string itemName = ItemManager.instance.itemsData.GetRandomUnlockedItem().ItemName;

            while (items.ContainsKey(itemName))
            {
                itemName = ItemManager.instance.itemsData.GetRandomUnlockedItem().ItemName;
            }

            int amount = managerQuestSellPerItem;
            items.Add(itemName, amount);
        }

        managerQuest = new ManagerQuest(items, dateTimeString);

        SendManagerQuest();
    }

    void SendManagerQuest()
    {
        string json = JsonConvert.SerializeObject(managerQuest);

        FirebaseCommunicator.instance.SendObject(json, managerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting manager quest! message: " + task.Exception.Message);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("CLOUD: update manager quest!");
            }
        });
    }

}
