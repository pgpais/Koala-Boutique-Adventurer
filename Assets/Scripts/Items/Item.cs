using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Item", menuName = "Ye Olde Shop/Item", order = 0)]
public class Item : ScriptableObject, UnlockableReward
{
    public enum ItemType
    {
        Gatherable,
        Lootable,
        Valuable,
        Processed
    }


    public UnityEvent<int> ItemUpdated { get; private set; }
    public string ItemNameKey { get => Localisation.Get(_itemNameStringKey, Language.English); }
    public string ItemName { get => Localisation.Get(_itemNameStringKey); }
    public StringKey _itemNameStringKey;
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }

    public int NumberOfInteractions => numberOfInteractions;
    [ShowIf("@this.Type == ItemType.Gatherable")]
    [Tooltip("How many interactions needed before the player can collect the gatherable")]
    [SerializeField] int numberOfInteractions = 3;

    public Item ProcessResult => processResult;


    [HideIf("@this.Type == ItemType.Valuable || this.Type == ItemType.Processed")]
    [SerializeField] Item processResult;

    public bool Unlocked => unlocked;
    bool unlocked;
    [SerializeField] bool startsUnlocked = true;
    private void OnEnable()
    {
        unlocked = startsUnlocked;
    }

    public void InitializeEvent()
    {
        ItemUpdated = new UnityEvent<int>();
    }


    public void Unlock()
    {
        unlocked = true;
    }

    internal bool IsSellable()
    {
        return ItemNameKey != "Gem" && processResult == null;
    }
}