using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] GameObject askForIDParent;
    [SerializeField] TMP_InputField askForIDInputField;
    [SerializeField] Toggle englishToggle;
    [SerializeField] Toggle portugueseToggle;
    [SerializeField] Button askForIDButton;

    [Space]

    [SerializeField] GameObject menuObject;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject classSelectScreen;
    [SerializeField] GameObject unlocksProgressScreen;
    [SerializeField] Button playButton;
    [SerializeField] Button selectClassButton;
    [SerializeField] Button selectClassBackButton;
    [SerializeField] Button logoutButton;

    [Header("Level Info")]
    [SerializeField] GameObject levelInfo;
    [SerializeField] TMP_Text levelInfoTitle;
    [SerializeField] TMP_Text levelInfoNumber;
    [SerializeField] TMP_Text levelInfoDifficulty;

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

        if (!FirebaseCommunicator.instance.IsLoggedIn)
        {
            menuObject.SetActive(false);
            askForIDParent.SetActive(true);
            FirebaseCommunicator.LoggedIn.AddListener(() =>
            {
                ShowMenuScreen();
                FirebaseCommunicator.instance.StartGame();
            });
        }
        else
        {
            ShowMenuScreen();
        }

        // GameManager.NoMissionExists.AddListener(OnNoMissionExists);
        GameManager.NewMissionAdded.AddListener(OnNewMissionAdded);

        if (Localisation.currentLanguage == Language.Portuguese)
        {
            portugueseToggle.isOn = true;
        }

        portugueseToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Localisation.SetLanguage(Language.Portuguese);
            }
        });


        englishToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Localisation.SetLanguage(Language.English);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        askForIDButton.onClick.AddListener(() => OnSubmitFamilyID(askForIDInputField.text));
        askForIDInputField.onSubmit.AddListener(OnSubmitFamilyID);

        playButton.onClick.AddListener(OnPlayButton);
        logoutButton.onClick.AddListener(OnLogout);
        selectClassButton.onClick.AddListener(ShowClassSelectScreen);
        selectClassBackButton.onClick.AddListener(ShowMenuScreen);

        classSelectScreen.SetActive(false);
        // menuScreen.SetActive(true);
    }

    void OnNoMissionExists()
    {
        playButton.interactable = false;
    }

    void OnNewMissionAdded()
    {
        playButton.interactable = true;
    }

    internal void ShowUnlocksProgessPopup()
    {
        unlocksProgressScreen.SetActive(true);
    }

    void OnPlayButton()
    {
        playButton.interactable = false;
        logoutButton.interactable = false;

        GameManager.instance.StartRun();
    }

    void OnSubmitFamilyID(string familyId)
    {
        if (string.IsNullOrEmpty(familyId))
        {
            return;
        }

        FileUtils.DeleteFile(FileUtils.GetPathToPersistent(FirebaseCommunicator.familyIDSavePath));
        FileUtils.WriteStringToFile(FileUtils.GetPathToPersistent(FirebaseCommunicator.familyIDSavePath), familyId);

        FirebaseCommunicator.instance.LoginAnonymously(familyId);

        ShowMenuScreen();
    }

    void OnLogout()
    {
        askForIDParent.SetActive(true);
        menuObject.SetActive(false);

        FirebaseCommunicator.instance.Logout();

        SceneManager.LoadScene(0);
    }

    void ShowClassSelectScreen()
    {
        askForIDParent.SetActive(false);
        classSelectScreen.SetActive(true);
        // menuScreen.SetActive(false);
        SetSelectedMenuItem(selectClassBackButton.gameObject);
    }

    void ShowMenuScreen()
    {
        askForIDParent.SetActive(false);
        classSelectScreen.SetActive(false);
        menuScreen.SetActive(true);
        SetSelectedMenuItem(playButton.gameObject);
    }

    void SetSelectedMenuItem(GameObject obj)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(obj);
    }

    public void ShowLevelInfo()
    {
        levelInfo.SetActive(true);
        levelInfoTitle.text = Localisation.Get(StringKey.UI_LevelInfo_Title);
        levelInfoNumber.text = GameManager.instance.NumberOfMissions.ToString();
        MissionDifficulty difficulty = GameManager.instance.DifficultyRank;

        switch (difficulty)
        {
            case MissionDifficulty.Easy:
                levelInfoDifficulty.text = Localisation.Get(StringKey.UI_LevelInfo_Easy);
                break;
            case MissionDifficulty.Medium:
                levelInfoDifficulty.text = Localisation.Get(StringKey.UI_LevelInfo_Medium);
                break;
            case MissionDifficulty.Hard:
                levelInfoDifficulty.text = Localisation.Get(StringKey.UI_LevelInfo_Hard);
                break;
            default:
                levelInfoDifficulty.text = "???";
                break;
        }
    }

    public void HideLevelInfo()
    {
        levelInfo.SetActive(false);
    }
}
