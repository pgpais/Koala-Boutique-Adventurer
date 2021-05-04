using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExit : MonoBehaviour
{
    void FinishLevel()
    {
        MissionManager.instance.FinishLevel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FinishLevel();
        }
    }
}
