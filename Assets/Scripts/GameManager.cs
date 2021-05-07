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

    public void StartGame()
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
                currentMission = JsonConvert.DeserializeObject<Mission>(task.Result.GetRawJsonValue());
                SceneManager.LoadScene(1);
            }
        });
    }

    public void FinishLevel()
    {
        currentMission.successfulRun = true;
        FirebaseCommunicator.instance.SendObject(JsonUtility.ToJson(currentMission), "missions", (task) =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("yey!");
            }
        });
    }
}
