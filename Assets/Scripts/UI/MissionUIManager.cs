using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class MissionUIManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [SerializeField] TMP_Text seedText;
    [SerializeField] TMP_Text timeRemainingText;

    [SerializeField] GameObject hud;
    [SerializeField] MissionEndScreen endScreen;
    [SerializeField] MissionEndScreen failedScreen;


    private float timeRemaining;
    // Start is called before the first frame update

    private void Awake()
    {
        MissionManager.MissionStarted.AddListener((time) => timeRemaining = time);
        MissionManager.MissionEnded.AddListener(ShowMissionEndScreen);
        hud.SetActive(true);
    }
    void Start()
    {
        endScreen.gameObject.SetActive(false);
        failedScreen.gameObject.SetActive(false);

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
        string timeString = $"{(int)timeRemaining / 60}:{((int)timeRemaining % 60).ToString("00")}";
        timeRemainingText.text = timeString;
    }

    void ShowMissionEndScreen()
    {
        endScreen.gameObject.SetActive(true);
    }

    void ShowMissionFailedScreen()
    {
        failedScreen.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        LevelManager.Instance.Players[0]._health.OnDeath += ShowMissionFailedScreen;
    }
}
