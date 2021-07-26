using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OracleInfo : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text hourText;

    public void InitUI(Sprite itemSprite, int index)
    {
        itemImage.sprite = itemSprite;
        int hour = OracleManager.Instance.GetHour(index);
        hourText.text = $"{hour}:00 - {hour + 3}:00";
    }
}
