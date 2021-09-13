using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectScreen : MonoBehaviour
{
    [SerializeField] TMP_Text classSelectTitleText;
    [SerializeField] StringKey classSelectTitleTextKey;
    [SerializeField] ItemSmallUI classSelectButton;
    [SerializeField] Transform layoutGroup;
    [SerializeField] Button exitButton;
    [SerializeField] Sprite lockedClassSprite;
    [SerializeField] Color lockedIconColor;

    [SerializeField] List<CharacterClassData> possibleStartingClasses;

    private void Start()
    {
        // exitButton.onClick.AddListener(HideMenu);
        // foreach (Transform child in layoutGroup)
        // {
        //     Destroy(child.gameObject);
        // }

        // foreach (var characterClass in possibleStartingClasses)
        // {

        //     var button = Instantiate(buttonPrefab, layoutGroup).GetComponent<Button>();
        //     if (!characterClass.Unlocked)
        //     {
        //         button.interactable = false;
        //     }
        //     button.GetComponentInChildren<TMPro.TMP_Text>().text = characterClass.className;
        //     button.onClick.AddListener(() =>
        //     {
        //         GameManager.instance.currentSelectedClass = characterClass;
        //         weaponPreview.sprite = characterClass.initialWeapon.GetComponentInChildren<SpriteRenderer>().sprite;
        //         weaponPreview.color = Color.white;
        //     });
        // }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     HideMenu();
        // }
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // exitButton.gameObject.SetActive(true);
        foreach (Transform child in layoutGroup)
        {
            Destroy(child.gameObject);
        }

        foreach (var characterClass in possibleStartingClasses)
        {
            var classSelect = Instantiate(classSelectButton, layoutGroup);

            var toggle = classSelect.GetComponent<Toggle>();
            if (!characterClass.Unlocked)
            {
                classSelect.Init(characterClass.ClassName, lockedClassSprite, lockedIconColor);
                toggle.interactable = false;
            }
            else
            {
                classSelect.Init(characterClass.ClassName, characterClass.initialWeapon.GetComponentInChildren<SpriteRenderer>().sprite);
                toggle.interactable = true;

                if (GameManager.instance.currentSelectedClass == characterClass)
                {
                    toggle.isOn = true;
                }
            }

            toggle.group = layoutGroup.GetComponent<ToggleGroup>();

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    LogsManager.SendLogDirectly(new Log(
                        LogType.ClassSelected,
                        new Dictionary<string, string>(){
                        {"Class", characterClass.ClassName}
                        }
                    ));

                    GameManager.instance.currentSelectedClass = characterClass;
                }
            });
        }

        classSelectTitleText.text = Localisation.Get(classSelectTitleTextKey);
    }
}
