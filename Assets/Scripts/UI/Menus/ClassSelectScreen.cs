using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectScreen : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform layoutGroup;
    [SerializeField] Image weaponPreview;

    [SerializeField] List<CharacterClassData> possibleStartingClasses;

    private void Start()
    {
        weaponPreview.sprite = null;
        weaponPreview.color = Color.clear;

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


    private void OnEnable()
    {
        foreach (Transform child in layoutGroup)
        {
            Destroy(child.gameObject);
        }

        foreach (var characterClass in possibleStartingClasses)
        {

            var button = Instantiate(buttonPrefab, layoutGroup).GetComponent<Button>();
            if (!characterClass.Unlocked)
            {
                button.interactable = false;
            }
            button.GetComponentInChildren<TMPro.TMP_Text>().text = characterClass.className;
            button.onClick.AddListener(() =>
            {
                GameManager.instance.currentSelectedClass = characterClass;
                weaponPreview.sprite = characterClass.initialWeapon.GetComponentInChildren<SpriteRenderer>().sprite;
                weaponPreview.color = Color.white;
            });
        }
    }
}
