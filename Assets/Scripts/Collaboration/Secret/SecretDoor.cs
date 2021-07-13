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

    internal bool SubmitCode(int code)
    {
        if (code.ToString() == doorTime.code)
        {
            Unlock();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Unlock()
    {
        doorTime.unlocked = true;
        teleporter.Activable = true;
        SendDoorTime(doorTime);

        Destroy(this);
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

                if (string.IsNullOrEmpty(json))
                {
                    doorTime = new DoorTime(null, null, false);
                }

                doorTime = JsonConvert.DeserializeObject<DoorTime>(json);

                DateTime requestDate = DateTime.ParseExact(doorTime.interactDate, dateFormat, null);
                //if today is after the date of the interact plus 2 days, create new request
                if (DateTime.Now > requestDate.AddDays(2))
                {
                    CreateNewRequest();
                }
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
                CreateNewRequest();

                SendDoorTime(doorTime);
            }
            else
            {
                Debug.Log("Already requested!");
            }
        }

        if (!doorTime.unlocked)
        {
            secretDoorUI.Init(this);
        }
    }

    void CreateNewRequest()
    {
        doorTime = new DoorTime(null, DateTime.Now.ToString(dateFormat));
    }

    void SendDoorTime(DoorTime doorTime)
    {
        //TODO: send item for processing
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