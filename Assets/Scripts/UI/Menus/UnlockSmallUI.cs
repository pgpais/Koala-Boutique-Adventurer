using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSmallUI : MonoBehaviour, IComparable<UnlockSmallUI>
{
    public bool IsUnlocked => unlock.Unlocked;

    [SerializeField] Image image;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Color notUnlockedColor;

    Unlockable unlock;

    public int CompareTo(UnlockSmallUI other)
    {
        return this.unlock.CompareTo(other.unlock);
    }

    public void Init(Unlockable unlockable)
    {
        gameObject.name = unlockable.name;

        unlock = unlockable;

        nameText.text = unlockable.UnlockableName;
        image.sprite = unlockable.UnlockableIcon;

        if (!unlockable.Unlocked)
        {
            image.color = notUnlockedColor;

            unlockable.UnlockableUpdated.AddListener(OnUnlock);
        }
        else
        {
            image.color = Color.white;
        }
    }

    void OnUnlock(Unlockable unlockable)
    {
        if (unlockable.Unlocked)
        {
            image.color = Color.white;
        }
    }
}
