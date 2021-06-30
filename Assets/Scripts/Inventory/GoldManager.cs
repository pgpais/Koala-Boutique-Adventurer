using UnityEngine;

public class GoldManager
{
    public static string goldReferenceName = "gold";
    public static string gemsReferenceName = "gems";

    public static void GetGoldSendWithModifier(int modifier)
    {
        Debug.Log("sick items");

        FirebaseCommunicator.instance.GetObject(goldReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
                return;
            }

            if (task.IsCompleted)
            {

                Debug.Log("yey got gold");
                string json = task.Result.GetRawJsonValue();
                Debug.Log("gold: " + json);

                int gold = 0;
                if (!string.IsNullOrEmpty(json))
                {
                    gold = int.Parse(json);
                }

                UploadGold(gold + modifier);
            }
        });
    }

    public static void UploadGold(int gold)
    {
        FirebaseCommunicator.instance.SendObject(gold.ToString(), goldReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("yey updated gold");
            }
        });
    }

    public static void GetGemsAndSendWithModifier(int modifier)
    {
        FirebaseCommunicator.instance.GetObject(gemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
                return;
            }

            if (task.IsCompleted)
            {

                Debug.Log("yey got gems");
                string json = task.Result.GetRawJsonValue();
                Debug.Log("gems: " + json);

                int gems = 0;
                if (!string.IsNullOrEmpty(json))
                {
                    gems = int.Parse(json);
                }

                UploadGems(gems + modifier);
            }
        });
    }

    public static void UploadGems(int gems)
    {
        FirebaseCommunicator.instance.SendObject(gems.ToString(), gemsReferenceName, (task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("smth went wrong. " + task.Exception.ToString());
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("yey updated gems");
            }
        });
    }
}