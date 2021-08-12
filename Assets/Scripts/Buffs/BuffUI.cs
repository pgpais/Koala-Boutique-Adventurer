using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    // [SerializeField] TMPro.TMP_Text nameText;
    [SerializeField] TMPro.TMP_Text descriptionText;

    public void Initialize(Buff buff)
    {
        // nameText.text = buff.buffName;
        descriptionText.text = buff.Description;
    }

    public void Initialize(string tooltip)
    {
        // if (nameText != null)
        // {
        //     nameText.text = tooltip;
        // }
        descriptionText.text = tooltip;
    }
}
