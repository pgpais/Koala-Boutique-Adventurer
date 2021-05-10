using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] GameObject menuObject;
    [SerializeField] Button playButton;

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

        playButton.onClick.AddListener(OnPlayButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayButton()
    {
        playButton.gameObject.SetActive(false);

        GameManager.instance.StartGame();
    }
}
