using UnityEngine;

public class GoldManager
{
    public static string referenceName = "gold";

    public static void GetGoldSendWithModifier(int modifier)
    {
        Debug.Log("sick items");

        FirebaseCommunicator.instance.GetObject(referenceName, (task) =>
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

                UploadGold(gold);
            }
        });
    }

    public static void UploadGold(int gold)
    {
        FirebaseCommunicator.instance.SendObject(gold.ToString(), referenceName, (task) =>
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
}