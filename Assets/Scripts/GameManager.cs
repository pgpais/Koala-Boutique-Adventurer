using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string diseasedItemReferenceName = "diseasedItems";
    public static UnityEvent NoMissionExists = new UnityEvent();
    public static UnityEvent NewMissionAdded = new UnityEvent();

    public static GameManager instance;

    public FamilyStats stats;

    public CharacterClassData currentSelectedClass;

    public Mission CurrentMission => currentMission;
    private Mission currentMission;

    public string DiseasedItemName => diseasedItemName;
    [SerializeField] string diseasedItemName;


    private bool performingMission;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }


        DontDestroyOnLoad(this);

        LoadSceneAdditivelyAndSetActive(1);

        FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
        FirebaseCommunicator.GameStarted.AddListener(OnGameStarted);
    }

    private void Start()
    {

        // Setup listener for new missions
        FirebaseCommunicator.instance.SetupListenForChildAddedEvents(new string[] { Mission.firebaseReferenceName, FirebaseCommunicator.instance.FamilyId.ToString() }, OnMissionAdded);
    }

    private void OnLoggedIn()
    {
        stats = new FamilyStats();
    }

    private void OnGameStarted()
    {
        GetMissions();
        GetDiseasedItem();
    }

    void GetMissions()
    {
        FirebaseCommunicator.instance.GetObject(MissionManager.referenceName, (task) =>
              {
                  if (task.IsFaulted)
                  {
                      Debug.LogError("smth went wrong. " + task.Exception.ToString());
                  }

                  if (task.IsCompleted)
                  {
                      Debug.Log("yey got mission");
                      string json = task.Result.GetRawJsonValue();
                      if (json == null)
                      {
                          OnNoMissionExists();
                          return;
                      }

                      currentMission = JsonConvert.DeserializeObject<Mission>(json);
                      NewMissionAdded.Invoke();
                  }
              });
    }

    void GetDiseasedItem()
    {
        FirebaseCommunicator.instance.GetObject(new string[] { diseasedItemReferenceName, DateTime.Today.ToString("yyyyMMdd") }, (task) =>
              {
                  if (task.IsFaulted)
                  {
                      Debug.LogError("smth went wrong. " + task.Exception.ToString());
                  }

                  if (task.IsCompleted)
                  {
                      Debug.Log("yey got mission");
                      string json = task.Result.GetRawJsonValue();
                      if (json == null)
                      {
                          Debug.LogError("No Diseased Item today!");
                          return;
                      }

                      diseasedItemName = JsonConvert.DeserializeObject<string>(json);

                  }
              });
    }

    public void StartRun()
    {
        performingMission = true;

        SceneManager.UnloadSceneAsync(1);

        LoadSceneAdditivelyAndSetActive(2);
    }

    public void FinishLevel(bool playerDied)
    {
        if (currentMission != null)
        {
            currentMission.completed = true;

            FirebaseCommunicator.instance.SendObject(JsonUtility.ToJson(currentMission), "missions", (task) =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Level finished!");

                    SceneManager.UnloadSceneAsync(2);
                    LoadSceneAdditivelyAndSetActive(1);
                }
            });
        }
        else
        {
            SceneManager.UnloadSceneAsync(2);
            LoadSceneAdditivelyAndSetActive(1);
        }

        if (playerDied)
        {
            FailedMission();
        }
        else
        {
            SuccessfulMission();
        }
    }

    void LoadSceneAdditivelyAndSetActive(int buildIndex)
    {
        var parameters = new LoadSceneParameters();
        parameters.loadSceneMode = LoadSceneMode.Additive;

        SceneManager.LoadScene(buildIndex, parameters);

        SceneManager.sceneLoaded += SetActiveScene;
    }

    void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    void OnNoMissionExists()
    {
        Debug.Log("Mission is null! Disable gameplay until we get a new mission");
        NoMissionExists.Invoke();
    }

    void OnMissionAdded(object sender, ChildChangedEventArgs args)
    {
        Debug.Log("Mission added!");
        GetMissions();
    }

    public void FailedMission()
    {
        stats.stats.numberOfMissions++;
        stats.stats.numberOfDeaths++;

        UpdateStats();
    }

    public void SuccessfulMission()
    {
        stats.stats.numberOfMissions++;
        stats.stats.numberOfSuccessfulMissions++;

        UpdateStats();
    }

    private void UpdateStats()
    {
        stats.UpdateStats();
    }
}
