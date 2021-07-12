using System;
using Newtonsoft.Json;
using UnityEngine;

public class CollaborationManager : MonoBehaviour
{
    public static string referenceName = "collaboration";

    public struct CollaborationUnlocks
    {
        public bool dailyQuest;
        public bool disease;
        public bool oracle;
        public bool kingOffering;
        public bool secretDoor;

        public CollaborationUnlocks(bool dailyQuest, bool disease, bool oracle, bool kingOffering, bool secretDoor)
        {
            this.dailyQuest = dailyQuest;
            this.disease = disease;
            this.oracle = oracle;
            this.kingOffering = kingOffering;
            this.secretDoor = secretDoor;
        }
    }

    public CollaborationUnlocks unlocks { get; private set; }

    public void UnlockCollaboration(bool dailyQuest, bool disease, bool oracle, bool kingOffering, bool secretDoor)
    {
        unlocks = new CollaborationUnlocks(
            unlocks.dailyQuest || dailyQuest,
            unlocks.disease || disease,
            unlocks.oracle || oracle,
            unlocks.kingOffering || kingOffering,
            unlocks.secretDoor || secretDoor
        );

        UpdateCollaborationUnlocks();
    }
    private void Awake()
    {
        FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
    }

    void OnLoggedIn()
    {
        GetCollaborationUnlocks();
    }

    void GetCollaborationUnlocks()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to get collaborations! message: " + task.Exception.Message);
                return;
            }
            else if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    unlocks = new CollaborationUnlocks(true, false, false, false, false);
                    UpdateCollaborationUnlocks();
                }
                else
                {
                    unlocks = JsonConvert.DeserializeObject<CollaborationUnlocks>(json);
                }

            }
        });
    }

    void UpdateCollaborationUnlocks()
    {
        string json = JsonConvert.SerializeObject(unlocks);
        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update collaboration! message: " + task.Exception.Message);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("CLOUD: Updated Collaboration!");
            }
        });
    }



}