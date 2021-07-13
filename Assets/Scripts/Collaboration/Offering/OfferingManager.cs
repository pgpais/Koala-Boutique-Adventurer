using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class OfferingManager : MonoBehaviour
{
    public static string referenceName = "offering";
    public static OfferingManager instance;

    public int numberOfItems = 4;
    private const string dateFormat = "yyyyMMdd";

    Offering offering;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        FirebaseCommunicator.LoggedIn.AddListener(OnLoggedIn);
    }

    void OnLoggedIn()
    {
        GetOffering();
    }

    void GetOffering()
    {
        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error: " + task.Exception.InnerException.Message);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Success getting offering: " + task.Result);
                string json = task.Result.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.Log("No offering found, creating a new one");
                    CreateOffering();
                }
                else
                {
                    Debug.Log("Found offering, deserializing");
                    this.offering = JsonConvert.DeserializeObject<Offering>(json);
                    if (this.offering.HasExpired())
                    {
                        CreateOffering();
                    }
                }
            }
        });
    }

    void CreateOffering()
    {
        Offering offering = new Offering();
        offering.itemsToOffer = new List<string>();
        for (int i = 0; i < numberOfItems; i++)
        {
            offering.itemsToOffer.Add(ItemManager.instance.itemsData.GetRandomUnlockedItem().ItemName);
        }

        offering.offerStartDate = DateTime.Today.ToString(dateFormat);

        SendOffering(offering);
    }

    void SendOffering(Offering offering)
    {
        string json = JsonConvert.SerializeObject(offering);
        FirebaseCommunicator.instance.SendObject(json, referenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error: " + task.Exception.InnerException.Message);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Success sending offering");
                this.offering = offering;
            }
        });
    }

    public struct Offering
    {
        public List<string> itemsToOffer;
        public string offerStartDate;
        public bool wasNotified;

        public Offering(List<string> itemsToOffer, string offerStartDate, bool wasNotified = false)
        {
            this.itemsToOffer = itemsToOffer;
            this.offerStartDate = offerStartDate;
            this.wasNotified = wasNotified;
        }

        internal bool HasExpired()
        {
            return DateTime.Today >= DateTime.ParseExact(offerStartDate, dateFormat, null).AddDays(2);
        }
    }
}