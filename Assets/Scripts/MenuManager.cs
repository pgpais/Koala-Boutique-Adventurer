using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] GameObject askForIDParent;
    [SerializeField] TMP_InputField askForIDInputField;
    [SerializeField] Button askForIDButton;

    [Space]

    [SerializeField] GameObject menuObject;
    [SerializeField] Button playButton;
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

        menuObject.SetActive(false);
        askForIDParent.SetActive(true);
        FirebaseCommunicator.LoggedIn.AddListener(() =>
        {
            menuObject.SetActive(true);
            askForIDParent.SetActive(false);
            FirebaseCommunicator.instance.StartGame();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        askForIDButton.onClick.AddListener(() => OnSubmitFamilyID(askForIDInputField.text));
        askForIDInputField.onSubmit.AddListener(OnSubmitFamilyID);

        playButton.onClick.AddListener(OnPlayButton);
        logoutButton.onClick.AddListener(OnLogout);
    }

    // Update is called once per frame
    void Update()
    {

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

        menuObject.SetActive(true);
        askForIDParent.SetActive(false);
    }

    void OnLogout()
    {
        PlayerPrefs.DeleteKey(PlayerSettingsKeys.familyId);
        FileUtils.DeleteFile(FileUtils.GetPathToPersistent(FirebaseCommunicator.familyIDSavePath));

        askForIDParent.SetActive(true);
        menuObject.SetActive(false);
    }
}
