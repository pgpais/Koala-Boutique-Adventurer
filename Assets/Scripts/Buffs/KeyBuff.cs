using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Ye Olde Shop/Key", order = 0)]
public class KeyBuff : Buff
{
    public override void Initialize(CharacterClass characterClass)
    {
        characterClass.DamagedEnemy.AddListener((health) =>
        {
            Exit exit = health.GetComponent<Exit>();
            if (exit != null)
            {
                exit.EnableExit(true);
            }
        });
    }
}
