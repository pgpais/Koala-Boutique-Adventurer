
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class AdventurerQuest
{
    public static int amountOfItems = 3;
    public static int maxItemQuantity = 10;
    public static string dateFormat = "yyyyMMdd";
    public Dictionary<string, int> itemQuantity;
    public string UnlockableRewardName { get; private set; }
    public string StartDay { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsChecked { get; private set; }

    public AdventurerQuest(Dictionary<string, int> itemQuantity, string unlockableRewardName, string startDay, bool isCompleted = false, bool isChecked = false)
    {
        this.itemQuantity = itemQuantity;
        UnlockableRewardName = unlockableRewardName;
        StartDay = startDay;
        IsCompleted = isCompleted;
        IsChecked = isChecked;
    }

    public bool CanCompleteQuest(Dictionary<string, int> itemQuantity)
    {
        if (!IsChecked)
        {
            return false;
        }

        bool correctItemQuantity = true;

        foreach (KeyValuePair<string, int> item in this.itemQuantity)
        {
            if (itemQuantity.ContainsKey(item.Key))
            {
                if (item.Value != itemQuantity[item.Key])
                {
                    correctItemQuantity = false;
                    break;
                }
            }
            else
            {
                correctItemQuantity = false;
                break;
            }
        }

        return correctItemQuantity;
    }

    public void CompleteQuest()
    {
        LogsManager.SendLogDirectly(new Log(
            LogType.AdventurerQuestSuccess,
            new Dictionary<string, string>(){
                {"questItems", JsonConvert.SerializeObject(itemQuantity)}
            }
        ));

        IsCompleted = true;
        IsChecked = false;
    }

    internal bool IsOld()
    {
        DateTime start = DateTime.ParseExact(StartDay, dateFormat, null);
        // return true if day is before today
        return (DateTime.Today - start).Days > 0;
    }
}

[System.Serializable]
internal class ManagerQuest
{
    public static int amountOfItems = 3;
    public static int maxItemQuantity = 10;
    public static string dateFormat = "yyyyMMdd";
    public Dictionary<string, int> Items { get; private set; }
    public string StartDay { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsChecked { get; private set; }

    public ManagerQuest(Dictionary<string, int> items, string startDay, bool isCompleted = false, bool isChecked = false)
    {
        Items = items;
        StartDay = startDay;
        IsCompleted = isCompleted;
        IsChecked = isChecked;
    }

    public bool OnSoldItem(string itemName, int amount)
    {
        if (Items.ContainsKey(itemName))
        {
            Items[itemName] -= amount;
            if (Items[itemName] <= 0)
            {
                Items.Remove(itemName);
                if (Items.Count == 0)
                {
                    IsCompleted = true;
                }
            }
        }

        return IsCompleted;
    }

    internal void Check()
    {
        IsChecked = true;
        LogsManager.SendLogDirectly(new Log(
            LogType.ManagerQuestChecked,
            new Dictionary<string, string>(){
                {"StartDay", StartDay}
            }
        ));
    }

    public bool IsOld()
    {
        DateTime today = DateTime.Today;
        DateTime startDay = DateTime.ParseExact(StartDay, dateFormat, null);

        return (today - startDay).Days != 0;
    }
}