using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHUD : MonoBehaviour
{
    [SerializeField] BuffHUDItem buffHUDPrefab;

    private void Awake()
    {
        CharacterClass.GotNewBuff.AddListener(AddNewBuff);

        // foreach (Buff buff in CharacterClass.instance.CurrentBuffs)
        // {
        //     Instantiate(buffHUDPrefab, transform).Init(buff);
        // }
    }

    private void AddNewBuff(Buff buff)
    {
        if (buff.icon != null)
        {
            Instantiate(buffHUDPrefab, transform).Init(buff);
        }
    }
}
