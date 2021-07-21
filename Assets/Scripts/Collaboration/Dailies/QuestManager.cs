using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static string completedAdventurerReferenceName = "completedAdventurerQuests";
    public static string completedManagerReferenceName = "completedManagerQuests";
    public static string adventurerReferenceName = "adventurerQuest";
    public static string managerReferenceName = "managerQuest";
    public static string dateFormat = "yyyyMMdd";
    public static QuestManager instance;

    public Dictionary<string, int> ManagerQuestItems => managerQuest.Items;

    public bool IsQuestComplete => adventurerQuest.IsCompleted;
    public int CompletedAdventurerQuests => completedAdventurerQuests;

    public bool testCompleteAdventurerQuest = false;

    AdventurerQuest adventurerQuest;
    private ManagerQuest managerQuest;
    private int completedAdventurerQuests = 0;

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
        GetCompletedAdventurerQuests();
    }

    private void Update()
    {
        if (testCompleteAdventurerQuest)
        {
            testCompleteAdventurerQuest = false;
            adventurerQuest.CompleteQuest();
            SendAdventurerQuest();
        }
    }

    internal void CheckManagerQuest()
    {
        managerQuest.Check();
        SaveManagerQuest();
    }

    private void GetCompletedAdventurerQuests()
    {
        FirebaseCommunicator.instance.GetObject(completedAdventurerReferenceName, (task) =>
        {
            string json = task.Result.GetRawJsonValue();
            if (string.IsNullOrEmpty(json))
            {
                completedAdventurerQuests = 0;
            }
            else
            {
                completedAdventurerQuests = JsonConvert.DeserializeObject<int>(json);
            }
        });
    }

    void GetAdventurerQuest()
    {
        FirebaseCommunicator.instance.GetObject(adventurerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to get adventurer quest");
                return;
            }
            else if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogWarning("No adventurer quest exists");
                    adventurerQuest = null;
                }
                else
                {
                    adventurerQuest = JsonConvert.DeserializeObject<AdventurerQuest>(json);
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
                Debug.LogError("Failed to get manager quest");
                return;
            }
            else if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("No manager quest exists");
                }
                else
                {
                    managerQuest = JsonConvert.DeserializeObject<ManagerQuest>(json);
                }
            }
        });
    }

    public bool ExistsManagerQuest()
    {
        return managerQuest != null && !managerQuest.IsOld();
    }

    void CreateManagerQuest()
    {
        Dictionary<string, int> questItems = new Dictionary<string, int>();
        List<Item> items = ItemManager.instance.itemsData.Items;

        for (int i = 0; i < ManagerQuest.amountOfItems; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, items.Count);
            Item item = items[randomIndex];
            while (questItems.ContainsKey(item.ItemName))
            {
                randomIndex = (randomIndex + 1) % items.Count;
                item = items[randomIndex];
            }

            questItems.Add(item.ItemName, UnityEngine.Random.Range(1, ManagerQuest.maxItemQuantity));
        }

        managerQuest = new ManagerQuest(questItems, DateTime.Today.ToString(dateFormat));
    }

    void SaveManagerQuest()
    {
        string json = JsonConvert.SerializeObject(managerQuest);
        FirebaseCommunicator.instance.SendObject(json, managerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to save manager quest");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Manager quest saved");
            }
        });
    }

    public bool TryToCompleteAdventurerQuest(Dictionary<string, int> itemsGathered)
    {
        if (!AdventurerQuestExists())
        {
            return false;
        }

        Debug.Log("trying adventurer quest");
        if (adventurerQuest.CanCompleteQuest(itemsGathered))
        {
            adventurerQuest.CompleteQuest();
            SendAdventurerQuest();
            completedAdventurerQuests++;
            SendCompletedAdventurerQuests();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SendCompletedAdventurerQuests()
    {
        string json = JsonConvert.SerializeObject(completedAdventurerQuests);
        FirebaseCommunicator.instance.SendObject(json, completedAdventurerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to send completed adventurer quest");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Completed adventurer quest sent");
            }
        });
    }

    public void SendAdventurerQuest()
    {
        string json = JsonConvert.SerializeObject(adventurerQuest);
        FirebaseCommunicator.instance.SendObject(json, adventurerReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to send adventurer quest");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sent adventurer quest");
            }
        });
    }

    public bool AdventurerQuestExists()
    {
        return adventurerQuest != null && adventurerQuest.IsChecked && !adventurerQuest.IsOld();
    }

    internal void OnLevelFinished()
    {
        if (!ExistsManagerQuest())
        {
            CreateManagerQuest();
            SaveManagerQuest();
        }
    }
}