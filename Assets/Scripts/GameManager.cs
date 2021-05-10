using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Mission CurrentMission => currentMission;
    private Mission currentMission;

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
    }

    private void Start()
    {
        FirebaseCommunicator.LoggedIn.AddListener(() => LoadSceneAdditivelyAndSetActive(1));

        FirebaseCommunicator.instance.GetObject("missions", (task) =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("smth went wrong. " + task.Exception.ToString());
                }

                if (task.IsCompleted)
                {
                    Debug.Log("yey got mission");
                    currentMission = JsonConvert.DeserializeObject<Mission>(task.Result.GetRawJsonValue());
                }
            });
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(1);

        LoadSceneAdditivelyAndSetActive(2);
    }

    public void FinishLevel()
    {


        currentMission.successfulRun = true;
        FirebaseCommunicator.instance.SendObject(JsonUtility.ToJson(currentMission), "missions", (task) =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("yey!");

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
}
