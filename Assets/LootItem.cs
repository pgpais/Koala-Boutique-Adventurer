using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [SerializeField] Item commonDrop;

    public void OnInstantiate()
    {
        InventoryManager.instance.AddItem(commonDrop.ItemName, 1);

        Destroy(gameObject);
    }
}
