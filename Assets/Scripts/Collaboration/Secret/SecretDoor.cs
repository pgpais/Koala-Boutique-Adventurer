using System;
using MoreMountains.TopDownEngine;
using Newtonsoft.Json;
using UnityEngine;

public class SecretDoor : ButtonActivated
{
    private const string dateFormat = "yyyyMMdd HH";
    public static string referenceName = "secretDoor";

    private SecretDoorUI secretDoorUI;
    private Teleporter teleporter;
    private DoorTime doorTime;

    private void Awake()
    {
        teleporter = GetComponent<Teleporter>();
    }

    private void Start()
    {
        teleporter.Activable = false;
        // EnableZone();
        GetDoorTime();

        secretDoorUI = FindObjectOfType<SecretDoorUI>();
    }

    void GetDoorTime()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed getting door time");
                return;
            }
            else if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();
                doorTime = JsonConvert.DeserializeObject<DoorTime>(json);
            }
        });
    }

    protected override void ActivateZone()
    {
        base.ActivateZone();

        if (string.IsNullOrEmpty(doorTime.code))
        {
            // Door still hasn't been requested
            if (doorTime.interactDate == null)
            {
                doorTime = new DoorTime(null, DateTime.Now.ToString(dateFormat));

                SendNewDoorRequest(doorTime);
            }
            else
            {
                Debug.Log("Already requested!");
            }
        }
        else if (!doorTime.unlocked)
        {
            // TODO: show passcode ui
        }
    }

    void SendNewDoorRequest(DoorTime doorTime)
    {
        string json = JsonConvert.SerializeObject(doorTime);
        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to send door request");
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Door request sent");
            }
        });
    }

    [System.Serializable]
    struct DoorTime
    {
        public string code;
        public string interactDate;
        public bool unlocked;

        public DoorTime(string code, string interactDate, bool unlocked = false)
        {
            this.code = code;
            this.interactDate = interactDate;
            this.unlocked = unlocked;
        }
    }
}