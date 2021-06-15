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
    public static UnityEvent NoMissionExists = new UnityEvent();
    public static UnityEvent NewMissionAdded = new UnityEvent();

    public static GameManager instance;


    public CharacterClassData currentSelectedClass;

    public Mission CurrentMission => currentMission;
    private Mission currentMission;


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
    }

    private void Start()
    {
        FirebaseCommunicator.GameStarted.AddListener(OnGameStarted);

        // Setup listener for new missions
        FirebaseCommunicator.instance.SetupListenForChildAddedEvents(new string[] { Mission.firebaseReferenceName, FirebaseCommunicator.instance.FamilyId.ToString() }, OnMissionAdded);
    }

    private void OnGameStarted()
    {
        GetMissions();
    }

    void GetMissions()
    {
        FirebaseCommunicator.instance.GetObject("missions", (task) =>
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

    public void StartRun()
    {
        performingMission = true;

        SceneManager.UnloadSceneAsync(1);

        LoadSceneAdditivelyAndSetActive(2);
    }

    public void FinishLevel()
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
}
