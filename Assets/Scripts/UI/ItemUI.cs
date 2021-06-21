using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemText;

    public void Init(Sprite itemImage, string itemName, int quantity)
    {
        this.itemImage.sprite = itemImage;
        this.itemText.text = $"{quantity}x {itemName}";
    }
}
