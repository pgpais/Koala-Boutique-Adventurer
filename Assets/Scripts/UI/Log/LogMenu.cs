using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OfferingManager;

public class LogMenu : MonoBehaviour
{
    [SerializeField] Button closeButton;

    [Header("Daily Quest")]
    [SerializeField] ItemSmallUI itemSmallUIPrefab;
    [SerializeField] LayoutGroup dailyQuestLayout;
    [SerializeField] TMP_Text questReward;
    [SerializeField] GameObject questCompleteImage;

    [Header("King's Offering")]
    [SerializeField] ItemSmallUI kingsOfferingItemSmallUIPrefab;
    [SerializeField] LayoutGroup offeringLayout;

    [Header("Oracle")]
    [SerializeField] SimpleScrollSnap scrollSnap;
    [SerializeField] OracleInfo oracleInfoPrefab;
    [SerializeField] Transform pageParent;
    [SerializeField] Transform paginationParent;
    [SerializeField] GameObject paginationPrefab;


    private void Awake()
    {
        closeButton.onClick.AddListener(HideMenu);
    }

    private void Start()
    {
        ShowOracleInfo();
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideMenu();
        }
    }

    private void OnEnable()
    {
        LogsManager.SendLogDirectly(new Log(
            LogType.NotebookInteracted,
            null
        ));

        HandleShowDailyQuest();

        HandleShowKingOffering();

        // ShowOracleInfo(); // Can't be here the it is implemented
    }

    private void HideDailyQuest()
    {

    }

    private void HandleShowDailyQuest()
    {
        // destroy childs of dailyQuestLayout
        foreach (Transform item in dailyQuestLayout.transform)
        {
            Destroy(item.gameObject);
        }

        if (!QuestManager.instance.ExistsManagerQuest())
        {
            HideDailyQuest();
        }
        else
        {
            QuestManager.instance.CheckManagerQuest();

            // create childs of dailyQuestLayout
            foreach (var itemQuantity in QuestManager.instance.ManagerQuestItems)
            {
                Item item = ItemManager.instance.itemsData.GetItemByName(itemQuantity.Key);

                ItemSmallUI itemUI = Instantiate(itemSmallUIPrefab);
                itemUI.transform.SetParent(dailyQuestLayout.transform, false);
                itemUI.Init(item, itemQuantity.Value);
            }

            questCompleteImage.SetActive(QuestManager.instance.IsManagerQuestComplete);
        }
    }

    private void HandleShowKingOffering()
    {

        // destroy childs of offeringLayout
        foreach (Transform item in offeringLayout.transform)
        {
            Destroy(item.gameObject);
        }

        if (OfferingManager.instance != null && OfferingManager.instance.OfferingExists())
        {
            //Show current offering
            Offering offering = OfferingManager.instance.GetCurrentOffering();

            string offeringListLog = "";
            foreach (var offeringItem in offering.itemsToOffer)
            {
                offeringListLog += offeringItem + ", ";
            }
            LogsManager.SendLogDirectly(new Log(
                LogType.KingOfferingChecked,
                new Dictionary<string, string>(){
                    { "offering", offeringListLog},
                    { "endDay", DateTime.Today.ToString("dd-MM-yyyy")}
                }
            ));

            foreach (var itemName in offering.itemsToOffer)
            {
                Item itemToOffer = ItemManager.instance.itemsData.GetItemByName(itemName);
                ItemSmallUI itemUI = Instantiate(kingsOfferingItemSmallUIPrefab);
                itemUI.transform.SetParent(offeringLayout.transform, false);
                itemUI.InitWithoutQuantity(itemToOffer);
            }
        }
        else
        {
            //Show offering not available info
        }
    }

    public void ShowOracleInfo()
    {
        foreach (Transform child in pageParent.transform)
        {
            Destroy(child.gameObject);
        }

        // destroy childs of paginationParent
        foreach (Transform child in paginationParent.transform)
        {
            Destroy(child.gameObject);
        }



        List<OracleData> oracleDataLog = OracleManager.Instance.OracleDataLog;

        foreach (OracleData oracleData in oracleDataLog)
        {
            OracleInfo info = Instantiate(oracleInfoPrefab);
            GameObject pag = Instantiate(paginationPrefab);

            pag.transform.SetParent(paginationParent, false);
            info.transform.SetParent(pageParent, false);

            scrollSnap.AddToBack(info.gameObject);

            var item = ItemManager.instance.itemsData.GetItemByName(oracleData.itemName);
            info.InitUI(item.sprite, oracleData.bestPriceIndex);
        }
    }
}
