
using System;
using System.Collections.Generic;

[System.Serializable]
public class AdventurerQuest
{
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

        return IsChecked && correctItemQuantity;
    }

    public void CompleteQuest()
    {
        IsCompleted = true;
    }
}

[System.Serializable]
internal class ManagerQuest
{
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
}