using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static string adventurerReferenceName = "adventurerQuest";
    public static string managerReferenceName = "managerQuest";
    public static QuestManager instance;

    AdventurerQuest adventurerQuest;
    private ManagerQuest managerQuest;

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

    public bool TryToCompleteAdventurerQuest(Dictionary<string, int> itemsGathered)
    {
        Debug.Log("trying adventurer quest");
        if (adventurerQuest.CanCompleteQuest(itemsGathered))
        {
            adventurerQuest.CompleteQuest();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AdventurerQuestExists()
    {
        return adventurerQuest != null;
    }
}