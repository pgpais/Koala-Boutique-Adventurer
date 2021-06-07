using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text nameText;
    [SerializeField] TMPro.TMP_Text DescriptionText;

    public void Initialize(Buff buff)
    {
        nameText.text = buff.buffName;
        DescriptionText.text = buff.description;
    }
}
