using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [Space]
    [SerializeField] TMP_Text windowTitleText;
    [SerializeField] StringKey windowTitleStringKey;
    [SerializeField] TMP_Text collectedItemsText;
    [SerializeField] StringKey collectedItemsStringKey;
    [SerializeField] TMP_Text offeringText;
    [SerializeField] StringKey offeringStringKey;
    [SerializeField] TMP_Text continueText;
    [SerializeField] StringKey continueStringKey;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FinishMission();
        }
    }

    private void OnEnable()
    {
        windowTitleText.text = Localisation.Get(windowTitleStringKey);
        collectedItemsText.text = Localisation.Get(collectedItemsStringKey);
        offeringText.text = Localisation.Get(offeringStringKey);
        continueText.text = Localisation.Get(continueStringKey);

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
            Instantiate(itemUIPrefab, itemsLootedLayout).Init(item.sprite, item.ItemNameKey, itemQuantity.Value);
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
            string offeringListLog = "";
            foreach (var offeringItem in offering.itemsToOffer)
            {
                offeringListLog += offeringItem + ", ";
            }
            LogsManager.SendLogDirectly(new Log(
                LogType.KingOfferingReceived,
                new Dictionary<string, string>(){
                    { "offering", offeringListLog},
                    { "endDay", DateTime.Today.ToString("dd-MM-yyyy")}
                }
            ));

            offeringParent.gameObject.SetActive(true);

            foreach (Transform child in offeringLayout)
            {
                Destroy(child.gameObject);
            }

            foreach (string itemName in offering.itemsToOffer)
            {
                Item item = ItemManager.instance.itemsData.GetItemByName(itemName);
                Instantiate(itemUIPrefab, offeringLayout).Init(item.sprite, item.ItemNameKey, 1);
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
