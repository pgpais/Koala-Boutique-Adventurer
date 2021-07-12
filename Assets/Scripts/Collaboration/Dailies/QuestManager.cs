using System;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private const string dateFormat = "yyyyMMdd";
    public static string referenceName = "adventurerQuest";

    public static QuestManager instance;

    Quest dailyQuest;
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
        GetQuest();

        MissionManager.MissionStarted.AddListener(OnMissionStarted);
        MissionManager.MissionEnded.AddListener(OnMissionEnd);
    }

    void GetQuest()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
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
                    GenerateNewQuest();
                    GetQuest();
                }
                else
                {
                    dailyQuest = JsonConvert.DeserializeObject<Quest>(json);

                    questDate = DateTime.ParseExact(dailyQuest.StartDay, dateFormat, null);
                }
            }
        });
    }

    private void Update()
    {
        int daysSinceQuest = (DateTime.Now - questDate).Days;
        if (dailyQuest != null && daysSinceQuest > 0)
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

        dailyQuest = new Quest(questItem, amount, goldReward, dateTimeString);

        string json = JsonConvert.SerializeObject(dailyQuest);
        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting daily quest! message: " + task.Exception.Message);
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
    }

    void OnMissionEnd()
    {

    }

    private void OnItemsAdded()
    {
        foreach (var item in InventoryManager.instance.ItemQuantity.Keys)
        {
            if (dailyQuest.CanCompleteQuest(item, InventoryManager.instance.ItemQuantity[item]))
            {
                dailyQuest.CompleteQuest();
            }
        }
        InventoryManager.ItemsAddedToGlobalInventory.RemoveListener(OnItemsAdded);
    }
}