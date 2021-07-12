using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ItemsList", menuName = "Ye Olde Shop/ItemsList", order = 0)]
public class ItemsList : ScriptableObject
{
    [SerializeField] List<Item> items;

    public bool ContainsByName(string itemName)
    {
        return items.Any(item => item.ItemName.Equals(itemName));
    }

    public Item GetItemByName(string itemName)
    {
        return items.Find(item => item.ItemName.Equals(itemName));
    }

    internal void InitializeEvents()
    {
        foreach (var item in items)
        {
            item.InitializeEvent();
        }
    }

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }

    public Item GetRandomUnlockedItem()
    {
        var unlocked = items.FindAll(item => item.Unlocked);
        return unlocked[Random.Range(0, unlocked.Count)];
    }
}