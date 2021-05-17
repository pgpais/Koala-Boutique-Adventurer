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

    public Item GetRandomitem()
    {
        var valuables = items.FindAll(item => item.Type == Item.ItemType.Valuable);
        return valuables[Random.Range(0, valuables.Count)];
    }
}