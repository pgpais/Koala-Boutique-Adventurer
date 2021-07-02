using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableCharacterData", menuName = "Ye Olde Shop/UnlockableCharacterData", order = 0)]
public class UnlockableCharacterData : ScriptableObject, UnlockableReward
{
    public bool Unlocked => unlocked;
    private bool unlocked;
    [SerializeField] bool startsUnlocked;

    public void Unlock()
    {
        unlocked = true;
    }
    private void OnEnable()
    {
        unlocked = startsUnlocked;
    }
}