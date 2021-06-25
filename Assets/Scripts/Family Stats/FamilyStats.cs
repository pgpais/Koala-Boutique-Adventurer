using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class FamilyStats
{
    public static string referenceName = "stats";

    [Serializable]
    public struct Stats
    {
        public int numberOfMissions;
        public int numberOfSuccessfulMissions;
        public int numberOfDeaths;
        public int buffsCollected;

        public Stats(int numberOfMissions, int numberOfSuccessfulMissions, int numberOfDeaths, int buffsCollected)
        {
            this.numberOfMissions = numberOfMissions;
            this.numberOfSuccessfulMissions = numberOfSuccessfulMissions;
            this.numberOfDeaths = numberOfDeaths;
            this.buffsCollected = buffsCollected;
        }
    }

    public Stats stats;

    public FamilyStats()
    {
        if (FirebaseCommunicator.instance != null)
        {
            GetFamilyStats();
        }
        else
        {
            InitializeStats();
        }
    }

    void GetFamilyStats()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
            }

            if (task.IsCompleted)
            {
                Debug.Log("yey got " + referenceName);
                string json = task.Result.GetRawJsonValue();
                if (json == null)
                {
                    Debug.LogError("No stats to show!");
                    InitializeStats();
                    return;
                }

                stats = JsonConvert.DeserializeObject<Stats>(json);
            }
        });
    }

    void InitializeStats()
    {
        stats = new Stats(0, 0, 0, 0);
    }

    internal void UpdateStats()
    {
        string json = JsonConvert.SerializeObject(stats);
        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed sending stats!");
            }

            if (task.IsCompleted)
            {
                Debug.Log("Successfully sent stats");
            }
        });
    }
}
