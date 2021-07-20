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
}
