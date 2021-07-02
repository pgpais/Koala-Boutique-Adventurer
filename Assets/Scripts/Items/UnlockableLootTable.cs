using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableLootDefinition", menuName = "Ye Olde Shop/Loot Definition")]
public class UnlockableLootTable : MMLootTableGameObjectSO
{

    public override GameObject GetLoot()
    {
        return GetUnlockedLootTable().GetLoot()?.Loot;
    }

    public override void ComputeWeights()
    {


        GetUnlockedLootTable().ComputeWeights();
    }

    private MMLootTableGameObject GetUnlockedLootTable()
    {
        MMLootTableGameObject lootTable = new MMLootTableGameObject();
        List<MMLootGameObject> unlockedLoots = LootTable.ObjectsToLoot.FindAll((loot) =>
        {
            return loot.Loot.GetComponent<LootItem>().Item.Unlocked;
        });

        lootTable.ObjectsToLoot = new List<MMLootGameObject>();

        foreach (var unlockedLoot in unlockedLoots)
        {
            lootTable.ObjectsToLoot.Add(unlockedLoot);
        }
        return lootTable;
    }
}
