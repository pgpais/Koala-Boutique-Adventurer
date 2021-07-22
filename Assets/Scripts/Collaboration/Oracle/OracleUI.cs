using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OracleUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;
    [SerializeField] Image m_Image;

    private OracleData data;

    // void Awake()
    // {
    //     if (gameObject.activeSelf)
    //     {
    //         Hide();
    //     }
    // }

    internal void Show(OracleData data)
    {
        this.data = data;
        Item item = ItemManager.instance.itemsData.GetItemByName(data.itemName);

        SetText(item.ItemName);
        SetSprite(item.sprite);
        gameObject.SetActive(true);
    }

    internal void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetText(string text)
    {
        int hour = OracleManager.Instance.GetHour();

        m_Text.text = $"{hour}:00 - {hour + 3}:00";
    }

    private void SetSprite(Sprite itemSprite)
    {
        m_Image.sprite = itemSprite;
    }
}