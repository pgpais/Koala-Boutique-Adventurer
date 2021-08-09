using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class NewUnlocksPanel : MonoBehaviour
{
    public static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    public static string unlocksLogReferenceName = "unlocksLog";

    [SerializeField] SimpleScrollSnap scrollSnap;
    [SerializeField] Transform contentParent;
    [SerializeField] Transform paginationParent;
    [SerializeField] GameObject paginationPrefab;
    [SerializeField] LayoutGroup layoutGroupPrefab;
    [SerializeField] int maxItemsPerPage = 6;
    [SerializeField] ItemSmallUI itemSmallUI;

    private LayoutGroup currentLayoutGroup;
    private int currentItemAmount = 0;
    private List<UnlockLog> unlockLogs;
    private UnlockLog asd;

    private void Start()
    {
        unlockLogs = new List<UnlockLog>();

        if (FirebaseCommunicator.instance.IsLoggedIn)
        {
            SetupUnlocksLogListener();
        }
        else
        {

            FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
        }
    }

    private void OnLoggedIn()
    {
        SetupUnlocksLogListener();
    }

    void SetupUnlocksLogListener()
    {
        FirebaseCommunicator.instance.SetupListenForValueChangedEvents(unlocksLogReferenceName, (obj, args) =>
        {
            string json = args.Snapshot.GetRawJsonValue();
            Debug.Log(json);

            if (json == null)
            {
                return;
            }
            else
            {
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, UnlockLog>>(json);
                foreach (var item in dictionary)
                {
                    Debug.Log(item.Key);
                    unlockLogs.Add(item.Value);
                    asd = item.Value;
                }

                SortLogList();

                AddLogsToPanel();

            }
        });
    }

    void AddLogsToPanel()
    {
        currentLayoutGroup = Instantiate(layoutGroupPrefab);
        GameObject pag = Instantiate(paginationPrefab);

        pag.transform.SetParent(paginationParent, false);
        currentLayoutGroup.transform.SetParent(contentParent, false);

        scrollSnap.AddToBack(currentLayoutGroup.gameObject);

        // NewItemsPanel panel = Instantiate(diseasedNewItemsPanel);

        // Item diseasedItem = ItemManager.instance.itemsData.GetItemByName(newItemsList.diseasedItemName);

        // GameObject pag = Instantiate(pagination);
        // pag.transform.SetParent(paginationParent, false);

        // panel.transform.SetParent(contentParent, false);
        // scrollSnap.AddToBack(panel.gameObject);
        // panel.Init(newItemsList.diseasedGoldLoss, diseasedItem, itemCounts);


        foreach (var unlockLog in unlockLogs)
        {
            var instantiatedObject = Instantiate(itemSmallUI);

            Unlockable unlockable = UnlockablesManager.instance.Unlockables[unlockLog.UnlockableName];


            instantiatedObject.transform.SetParent(currentLayoutGroup.transform, false);
            instantiatedObject.Init(unlockable.UnlockableName, unlockable.UnlockableIcon);
            currentItemAmount++;

            if (currentItemAmount == maxItemsPerPage)
            {
                currentLayoutGroup = Instantiate(layoutGroupPrefab);
                pag = Instantiate(paginationPrefab);

                pag.transform.SetParent(paginationParent, false);
                currentLayoutGroup.transform.SetParent(contentParent, false);

                scrollSnap.AddToBack(currentLayoutGroup.gameObject);
                currentItemAmount = 0;
            }
        }
    }

    private void SortLogList()
    {
        unlockLogs.Sort((logA, logB) =>
                {
                    if (logA.UnlockDateTime() < logB.UnlockDateTime())
                    {
                        return 1;
                    }
                    else if (logA.UnlockDateTime() == logB.UnlockDateTime())
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                });
    }
}

[Serializable]
struct UnlockLog
{
    [JsonProperty]
    public string UnlockableName { get; set; }
    [JsonProperty]
    public string UnlockDate { get; set; }

    public DateTime UnlockDateTime()
    {
        return DateTime.ParseExact(UnlockDate, NewUnlocksPanel.dateTimeFormat, null);
    }

    [JsonConstructor]
    public UnlockLog(string unlockableName, string unlockDate)
    {
        UnlockableName = unlockableName;
        UnlockDate = unlockDate;
    }

    public UnlockLog(string unlockableName, DateTime unlockDate)
    {
        UnlockableName = unlockableName;
        UnlockDate = unlockDate.ToString(NewUnlocksPanel.dateTimeFormat);
    }
}