using System;
using MoreMountains.TopDownEngine;
using Newtonsoft.Json;
using UnityEngine;

public class SecretDoor : ButtonActivated
{
    private const string dateFormat = "yyyyMMdd";
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
        if (doorTime.IsDecrypted())
        {
            if (doorTime.CorrectCode(code))
            {
                Unlock();
                return true;
            }
            else
            {
                return false;
            }
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

        teleporter.TriggerButtonAction();

        Destroy(this);
    }

    private void Start()
    {
        teleporter.Activable = false;
        // EnableZone();
        GetDoorTime();

        secretDoorUI = FindObjectOfType<SecretDoorUI>(true);
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
                else
                {
                    doorTime = JsonConvert.DeserializeObject<DoorTime>(json);
                }


                if (doorTime.HasExpired())
                {
                    Debug.Log("Door has expired");
                    DeleteRequest();
                }

                if (doorTime.unlocked)
                {
                    Unlock();
                }
            }
        });
    }
    protected override void ActivateZone()
    {
        base.ActivateZone();

        if (!doorTime.IsDecrypted())
        {
            // Door still hasn't been requested
            if (!doorTime.IsValid())
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

    //Delete Request
    void DeleteRequest()
    {
        doorTime = new DoorTime(null, null, false);
        SendDoorTime(doorTime);

        // ItemManager.instance.RemoveDoorItem();
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

        public bool HasExpired()
        {
            if (this.interactDate == null)
            {
                return true;
            }

            DateTime interactDate = DateTime.ParseExact(this.interactDate, dateFormat, null);
            DateTime today = DateTime.Today;

            return (today - interactDate).Days >= 2;
        }

        public bool IsValid()
        {
            return interactDate != null;
        }

        public bool IsDecrypted()
        {
            return code != null;
        }

        internal bool CorrectCode(int code)
        {
            return this.code == code.ToString();
        }
    }
}