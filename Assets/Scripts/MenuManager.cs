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
    }

    // Start is called before the first frame update
    void Start()
    {
        // menuObject.SetActive(false);
        // FirebaseCommunicator.LoggedIn.AddListener(() => menuObject.SetActive(true));

        bool hasId = PlayerPrefs.HasKey(PlayerSettingsKeys.familyId);

        menuObject.SetActive(hasId);
        askForIDParent.SetActive(!hasId);

        if (hasId)
        {
            FirebaseCommunicator.instance.StartGame();
        }

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
        playButton.gameObject.SetActive(false);

        GameManager.instance.StartRun();
    }

    void OnSubmitFamilyID(string familyId)
    {
        PlayerPrefs.SetInt(PlayerSettingsKeys.familyId, int.Parse(familyId));
        FirebaseCommunicator.instance.StartGame();

        menuObject.SetActive(true);
        askForIDParent.SetActive(false);
    }

    void OnLogout()
    {
        PlayerPrefs.DeleteKey(PlayerSettingsKeys.familyId);

        askForIDParent.SetActive(true);
        menuObject.SetActive(false);
    }
}
