using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffHUDItem : MonoBehaviour
{
    [SerializeField] Image buffImage;

    public void Init(Buff buff)
    {
        buffImage.sprite = buff.icon;
    }
}