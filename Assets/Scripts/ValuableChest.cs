using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableChest : MonoBehaviour
{

    [SerializeField] Item chestLoot;
    [SerializeField] int itemQuantity;

    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpen;

    bool wasOpened = false;





    // Start is called before the first frame update
    public void Interact()
    {
        if (wasOpened)
        {
            return;
        }

        Debug.Log("Interacted!");

        InventoryManager.instance.AddItem(chestLoot.ItemName, itemQuantity);
        Debug.Log("Collected chest!");
        chestClosed.SetActive(false);
        chestOpen.SetActive(true);
    }
}
