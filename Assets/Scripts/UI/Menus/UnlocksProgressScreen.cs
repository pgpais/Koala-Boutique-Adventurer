using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlocksProgressScreen : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<UnlockCategory, Transform> categoryParents = new Dictionary<UnlockCategory, Transform>();

    [SerializeField] Slider progressSlider;
    [SerializeField] TMP_Text progressText;
    [SerializeField] UnlockSmallUI prefab;
    [SerializeField] Button closePopup;

    [SerializeField] GameObject logScreen;

    List<UnlockSmallUI> unlockUIs = new List<UnlockSmallUI>();

    private void Awake()
    {
        foreach (var categoryParent in categoryParents.Values)
        {
            foreach (Transform child in categoryParent)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var unlock in UnlockablesManager.instance.Unlockables.Values)
        {
            var unlockUI = Instantiate(prefab);
            unlockUIs.Add(unlockUI);
            unlockUI.transform.SetParent(categoryParents[unlock.Category], false);
            unlockUI.Init(unlock);
        }

        SortUI();

        closePopup.onClick.AddListener(HideUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideUI();
        }
    }

    private void OnEnable()
    {
        closePopup.gameObject.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        UpdateSlider();
        SortUI();
    }

    private void UpdateSlider()
    {
        int unlockedCount = 0;
        foreach (var unlockUI in unlockUIs)
        {
            if (unlockUI.IsUnlocked)
            {
                unlockedCount++;
            }
        }
        progressSlider.value = unlockedCount / (float)unlockUIs.Count;
        progressText.text = unlockedCount + "/" + unlockUIs.Count;
    }

    private void OnDisable()
    {
        closePopup.gameObject.SetActive(false);

        logScreen.SetActive(true);
    }

    private void SortUI()
    {
        unlockUIs.Sort((a, b) => a.CompareTo(b));

        for (int i = 0; i < unlockUIs.Count; i++)
        {
            unlockUIs[i].transform.SetSiblingIndex(i);
        }
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
