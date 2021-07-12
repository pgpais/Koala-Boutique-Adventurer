
[System.Serializable]
public class Quest
{
    public string Item { get; private set; }
    public int Amount { get; private set; }
    public int GoldReward { get; private set; }
    public string StartDay { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsChecked { get; private set; }

    public Quest(string item, int amount, int goldReward, string startDay, bool isCompleted = false, bool isChecked = false)
    {
        Item = item;
        Amount = amount;
        GoldReward = goldReward;
        StartDay = startDay;
        IsCompleted = isCompleted;
        IsChecked = isChecked;
    }

    public bool CanCompleteQuest(string itemName, int amount)
    {
        return IsChecked && itemName == Item && amount == Amount;
    }

    public void CompleteQuest()
    {
        IsCompleted = true;
    }
}