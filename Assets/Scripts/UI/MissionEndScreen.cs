using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionEndScreen : MonoBehaviour
{
    [SerializeField] ItemUI itemUIPrefab;
    [Space]
    [SerializeField] Transform itemsLootedLayout;
    [SerializeField] Button finishButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        foreach (Transform child in itemsLootedLayout)
        {
            Destroy(child.gameObject);
        }

        ItemsList itemsList = ItemManager.instance.itemsData;

        // Show items caught
        foreach (var itemQuantity in InventoryManager.instance.ItemQuantity)
        {
            Item item = itemsList.GetItemByName(itemQuantity.Key);
            Instantiate(itemUIPrefab, itemsLootedLayout).Init(item.sprite, item.ItemName, itemQuantity.Value);
        }

        // Show enemies defeated

        // Show time remaining

        // Other stats

        // button listener
        finishButton.onClick.AddListener(FinishMission);
    }

    private void OnDisable()
    {
        finishButton.onClick.RemoveListener(FinishMission);
    }



    void FinishMission()
    {
        InventoryManager.instance.AddInventoryToGlobalItems();

        if (GameManager.instance != null)
        {
            GameManager.instance.FinishLevel();
        }
    }
}
