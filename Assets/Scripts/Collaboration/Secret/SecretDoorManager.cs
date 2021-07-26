using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SecretDoorManager : MonoBehaviour
{
    public static SecretDoorManager instance;

    public ButtonPrompt buttonPromptPrefab;

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
    }
}
