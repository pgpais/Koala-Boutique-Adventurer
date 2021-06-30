using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Afraid Buff", menuName = "Ye Olde Shop/Buffs/AfraidBuff", order = 0)]
public class AfraidBuff : Buff
{
    [SerializeField] int speedBuffAmount = 5;
    [SerializeField] float buffTime = 3f;

    private CharacterClass characterClass;

    public override void Initialize(CharacterClass characterClass)
    {
        this.characterClass = characterClass;

        RoomEntrances.RoomEntered.AddListener(OnRoomEntered);
    }

    void OnRoomEntered(RoomEntrances entrances)
    {
        entrances.StartCoroutine(OnRoomEnteredRoutine());
    }

    private IEnumerator OnRoomEnteredRoutine()
    {
        characterClass.ModifyMovementSpeed(speedBuffAmount);
        yield return new WaitForSeconds(buffTime);
        characterClass.ModifyMovementSpeed(-speedBuffAmount);
    }
}