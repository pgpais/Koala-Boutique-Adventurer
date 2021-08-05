using System;
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

        if (!playerDied)
        {

            // Show enemies defeated

            // Show time remaining

            // Other stats

            ShowOfferingIfNotNotified();

        }

        // button listener

    }

    private void ShowOfferingIfNotNotified()
    {
        OfferingManager.Offering offering = OfferingManager.instance.GetCurrentOffering();

        if (!offering.wasNotified)
        {
            offeringParent.gameObject.SetActive(true);

            foreach (Transform child in offeringLayout)
            {
                Destroy(child.gameObject);
            }

            foreach (string itemName in offering.itemsToOffer)
            {
                Item item = ItemManager.instance.itemsData.GetItemByName(itemName);
                Instantiate(itemUIPrefab, offeringLayout).Init(item.sprite, item.ItemName, 1);
            }
        }
        else
        {
            offeringParent.gameObject.SetActive(false);
        }
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
