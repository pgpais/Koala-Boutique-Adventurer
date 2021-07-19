using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogMenu : MonoBehaviour
{
    [SerializeField] Button closeButton;

    [Header("Daily Quest")]
    [SerializeField] ItemSmallUI itemSmallUIPrefab;
    [SerializeField] LayoutGroup dailyQuestLayout;
    [SerializeField] TMP_Text questReward;
    [SerializeField] GameObject questCompleteImage;

    private void Awake()
    {
        closeButton.onClick.AddListener(HideMenu);
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
        HandleShowDailyQuest();
    }

    private void HandleShowDailyQuest()
    {
        // destroy childs of dailyQuestLayout
        foreach (Transform item in dailyQuestLayout.transform)
        {
            Destroy(item.gameObject);
        }

        // create childs of dailyQuestLayout
        foreach (var itemQuantity in QuestManager.instance.ManagerQuestItems)
        {
            Item item = ItemManager.instance.itemsData.GetItemByName(itemQuantity.Key);

            ItemSmallUI itemUI = Instantiate(itemSmallUIPrefab);
            itemUI.transform.SetParent(dailyQuestLayout.transform, false);
            itemUI.Init(item, itemQuantity.Value);
        }

        questCompleteImage.SetActive(QuestManager.instance.IsQuestComplete);
    }
}
