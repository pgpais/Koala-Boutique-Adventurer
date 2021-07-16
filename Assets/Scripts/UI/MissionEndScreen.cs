using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionEndScreen : MonoBehaviour
{
    [SerializeField] ItemUI itemUIPrefab;
    [Space]
    [SerializeField] Transform itemsLootedLayout;
    [SerializeField] Transform offeringParent;
    [SerializeField] Transform offeringLayout;
    [SerializeField] Button finishButton;
    [SerializeField] bool playerDied;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            FinishMission();
        }
    }

    private void OnEnable()
    {
        MoreMountains.TopDownEngine.GameManager.Instance.Paused = true;

        Cursor.visible = true;

        if (!playerDied)
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

            // Show offering
            // OfferingManager.Offering offering = OfferingManager.instance.GetCurrentOffering();

            // offeringParent.gameObject.SetActive(!offering.wasNotified);
            // if (!offering.wasNotified)
            // {

            //     foreach (string itemName in offering.itemsToOffer)
            //     {
            //         Item item = itemsList.GetItemByName(itemName);
            //         Instantiate(itemUIPrefab, offeringLayout).Init(item.sprite, item.ItemName, 1);
            //     }

            //     OfferingManager.instance.OfferingNotified();
            // }
        }

        // button listener

    }

    private void OnDisable()
    {

    }



    void FinishMission()
    {
        if (GameManager.instance != null)
        {
            MoreMountains.TopDownEngine.GameManager.Instance.Paused = false;
            GameManager.instance.FinishLevel(playerDied);
        }
    }
}
