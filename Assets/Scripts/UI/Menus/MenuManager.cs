using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] GameObject askForIDParent;
    [SerializeField] TMP_InputField askForIDInputField;
    [SerializeField] Button askForIDButton;

    [Space]

    [SerializeField] GameObject menuObject;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject classSelectScreen;
    [SerializeField] Button playButton;
    [SerializeField] Button selectClassButton;
    [SerializeField] Button selectClassBackButton;
    [SerializeField] Button logoutButton;

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

    void OnPlayButton()
    {
        playButton.interactable = false;
        logoutButton.interactable = false;

        GameManager.instance.StartRun();
    }

    void OnSubmitFamilyID(string familyId)
    {
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
    }

    void ShowClassSelectScreen()
    {
        askForIDParent.SetActive(false);
        classSelectScreen.SetActive(true);
        menuScreen.SetActive(false);
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
}
