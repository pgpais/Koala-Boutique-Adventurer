using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OracleUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;
    [SerializeField] Image m_Image;
    [SerializeField] GameObject tooltip;

    private OracleAltar oracleAltar;

    void Start()
    {
        Hide();
    }

    internal void Show(OracleAltar oracleAltar)
    {
        this.oracleAltar = oracleAltar;
        SetText(OracleManager.Instance.GetItemName());
        SetSprite(OracleManager.Instance.GetItemIcon());
        tooltip.SetActive(true);
    }

    internal void Hide()
    {
        tooltip.SetActive(false);
    }

    private void SetText(string text)
    {
        int hour = OracleManager.Instance.GetHour();

        m_Text.text = $"Best time to sell {text} is between {hour}h and {hour + 3}h";
    }

    private void SetSprite(Sprite itemSprite)
    {
        m_Image.sprite = itemSprite;
    }
}