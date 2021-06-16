using UnityEngine;

[CreateAssetMenu(fileName = "RoomTypeToIcon", menuName = "Ye Olde Shop/RoomTypeToIcon", order = 0)]
public class RoomTypeToIcon : ScriptableObject
{
    public SerializableDictionary<RoomEntrances.RoomType, Sprite> TypeToIcon;
}
