using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text seedText;
    [SerializeField] TMP_Text timeRemainingText;

    private float timeRemaining;
    // Start is called before the first frame update

    private void Awake()
    {
        MissionManager.MissionStarted.AddListener((time) => timeRemaining = time);
    }
    void Start()
    {
        seedText.text = "Seed: " + MissionManager.instance.Seed.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            CountdownTime();
            PresentTimeLeft();
        }
    }

    void CountdownTime()
    {
        timeRemaining -= Time.deltaTime;
    }

    void PresentTimeLeft()
    {
        string timeString = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60}";
        timeRemainingText.text = timeString;
    }
}
