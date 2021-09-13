using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class UnlockablesManager : MonoBehaviour
{
    private const string referenceName = "techs";
    public static UnityEvent OnGotUnlockables = new UnityEvent();
    public static UnlockablesManager instance;

    [SerializeField] UnlockablesList unlockablesData;

    public Dictionary<string, Unlockable> Unlockables => unlockables;
    public bool GotUnlockables => gotUnlockables;

    public List<Unlockable> Unlocked => unlockables.Values.Where((unlockable) => unlockable.Unlocked).ToList();

    Dictionary<string, Unlockable> unlockables;

    bool gotUnlockables;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);

        unlockables = new Dictionary<string, Unlockable>();

        foreach (var unlockable in unlockablesData.unlockables)
        {
            unlockable.InitializeEvent();
            unlockables.Add(unlockable.UnlockableNameKey, unlockable);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnLoggedIn()
    {
        SetupUnlockablesListener();
        // GetUnlockedUnlockables();
    }

    private void SetupUnlockablesListener()
    {
        FirebaseCommunicator.instance.SetupListenForValueChangedEvents(referenceName, (obj, args) =>
        {
            string json = args.Snapshot.GetRawJsonValue();

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("No Unlockables");
            }
            else
            {
                Debug.Log(json);
                string[] unlockedNames = JsonConvert.DeserializeObject<string[]>(json);
                foreach (var unlockableName in unlockedNames)
                {
                    if (unlockables.ContainsKey(unlockableName))
                    {
                        unlockables[unlockableName].Unlock();
                    }
                }

            }
            gotUnlockables = true;
            OnGotUnlockables.Invoke();
        });
    }

    void GetUnlockedUnlockables()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
            }

            if (task.IsCompleted)
            {
                Debug.Log("yey got unlocks");
                string[] unlockedNames = JsonHelper.DeserializeArray<string>(task.Result.GetRawJsonValue());
                foreach (var unlockableName in unlockedNames)
                {
                    if (unlockables.ContainsKey(unlockableName))
                    {
                        unlockables[unlockableName].Unlock();
                    }
                }
                gotUnlockables = true;
                OnGotUnlockables.Invoke();
            }
        });
    }

    // internal void Unlock(Unlockable unlockable)
    // {
    //     bool success = true;

    //     foreach (var requirement in unlockable.Requirements)
    //     {
    //         if (!requirement.Unlocked)
    //             success = false;
    //     }

    //     foreach (var cost in unlockable.Cost)
    //     {
    //         if (!ItemManager.instance.HasEnoughItem(cost.Key.ItemName, cost.Value))
    //         {
    //             success = false;
    //         }
    //     }

    //     if (success)
    //     {
    //         foreach (var cost in unlockable.Cost)
    //         {
    //             ItemManager.instance.RemoveItem(cost.Key.ItemName, cost.Value);
    //         }

    //         unlockable.Unlock();
    //         SaveUnlockOnCloud();
    //     }
    // }

    void SaveUnlockOnCloud()
    {
        List<string> list = unlockables.Where(u => u.Value.Unlocked).Select(u => u.Value.UnlockableNameKey).ToList();

        string json = "[";
        foreach (var item in list)
        {
            json += "\"" + item + "\"" + ",";
        }
        json = json.Substring(0, json.Length - 1);
        json += "]";

        Debug.Log(json);

        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
            }

            if (task.IsCompleted)
            {
                Debug.Log("yey unlocked sync");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsUnlockableUnlocked(string unlockableName)
    {
        if (!unlockables.ContainsKey(unlockableName))
        {
            Debug.LogError("Unlockable with name " + unlockableName + " does not exist!");

            return false;
        }

        return unlockables[unlockableName].Unlocked;
    }
}
