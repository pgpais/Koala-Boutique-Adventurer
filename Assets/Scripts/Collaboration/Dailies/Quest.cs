
using System;
using System.Collections.Generic;

[System.Serializable]
public class AdventurerQuest
{
    public static int amountOfItems = 3;
    public static int maxItemQuantity = 10;
    public static string dateFormat = "yyyyMMdd";
    public Dictionary<string, int> itemQuantity;
    public int GoldReward { get; private set; }
    public string StartDay { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsChecked { get; private set; }

    public AdventurerQuest(Dictionary<string, int> itemQuantity, int goldReward, string startDay, bool isCompleted = false, bool isChecked = false)
    {
        this.itemQuantity = itemQuantity;
        GoldReward = goldReward;
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
        IsCompleted = true;
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
    }

    public bool IsOld()
    {
        DateTime today = DateTime.Today;
        DateTime startDay = DateTime.ParseExact(StartDay, dateFormat, null);

        return (today - startDay).Days != 0;
    }
}