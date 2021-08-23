using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class NewItemsManager : MonoBehaviour
{
    public static NewItemsManager instance;

    private const string alreadySeenItemsReferenceName = "alreadySeenItems";

    [SerializeField] List<Item> alreadySeenItemsDefault = new List<Item>();

    List<string> alreadySeenItems = new List<string>();

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

        if (FirebaseCommunicator.instance != null && FirebaseCommunicator.instance.IsLoggedIn)
        {
            OnLoggedIn();
        }
        else
        {
            FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
        }
    }

    public void OnNewItemAdded(string itemNameKey, int amount)
    {
        Debug.Log("have seen itme befor? " + HasSeenItemBefore(itemNameKey));
        if (!HasSeenItemBefore(itemNameKey))
        {
            alreadySeenItems.Add(itemNameKey);
            SendAlreadySeenItems();
        }
    }

    private void OnLoggedIn()
    {
        GetAlreadySeenItems();
    }

    private void SendAlreadySeenItems()
    {
        string json = JsonConvert.SerializeObject(alreadySeenItems);
        FirebaseCommunicator.instance.SendObject(json, alreadySeenItemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error sending already seen items: " + task.Exception.InnerException.Message);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Sent already seen items");
            }
        });
    }

    private void GetAlreadySeenItems()
    {
        FirebaseCommunicator.instance.GetObject(alreadySeenItemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error getting already seen items: " + task.Exception.InnerException.Message);
            }
            else if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();
                if (string.IsNullOrEmpty(json))
                {
                    Debug.Log("No already seen items found");
                }
                else
                {
                    Debug.Log("Already seen items: " + json);
                    alreadySeenItems = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
        });
    }

    public bool HasSeenItemBefore(Item item)
    {
        return alreadySeenItems.Contains(item.ItemNameKey) || alreadySeenItemsDefault.Contains(item);
    }

    public bool HasSeenItemBefore(string ItemNameKey)
    {
        return alreadySeenItems.Contains(ItemNameKey) || alreadySeenItemsDefault.Any((item) => item.ItemNameKey == ItemNameKey);
    }
}
