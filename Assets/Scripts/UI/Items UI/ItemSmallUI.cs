using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSmallUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text quantityLabel;

    public void Init(Item item, int quantity)
    {
        image.sprite = item.sprite;
        quantityLabel.text = quantity.ToString();
    }

    public void InitWithoutQuantity(Item item)
    {
        image.sprite = item.sprite;
    }

    internal void Init(string unlockableName, Sprite unlockableIcon)
    {
        quantityLabel.text = unlockableName;
        image.sprite = unlockableIcon;
    }

    internal void Init(string unlockableName, Sprite unlockableIcon, Color color)
    {
        quantityLabel.text = unlockableName;
        image.sprite = unlockableIcon;
        image.color = color;
    }
}
