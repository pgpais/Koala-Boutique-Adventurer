using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHUD : MonoBehaviour
{
    public string ItemNameKey => item.ItemNameKey;

    [SerializeField] Image image;
    [SerializeField] TMP_Text label;

    private Item item;
    private int quantity;

    public void Init(Item item, int quantity)
    {
        this.item = item;

        this.quantity = quantity;
        label.text = quantity.ToString();

        image.sprite = item.sprite;
    }

    public void UpdateQuantity(int quantity)
    {
        this.quantity += quantity;
        label.text = this.quantity.ToString();
    }
}
