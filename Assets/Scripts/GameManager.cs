using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        FirebaseCommunicator.instance.GetMissionByFamilyId((mission) =>
        {
            currentMission = mission;

            SceneManager.LoadScene(1);
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
