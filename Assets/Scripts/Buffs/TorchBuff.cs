using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TorchBuff", menuName = "Ye Olde Shop/Buffs/TorchBuff", order = 0)]
public class TorchBuff : Buff
{
    [SerializeField] float lightAmountToAdd = 5f;

    public override void Initialize(CharacterClass characterClass)
    {
        PlayerLantern lantern = characterClass.GetComponent<PlayerLantern>();
        lantern.AddLight(lightAmountToAdd);

    }
}
