using System.Collections.Generic;
using UnityEngine;
using static RoomEntrances;

[CreateAssetMenu(fileName = "RoomPrefabs", menuName = "Ye Olde Shop/RoomPrefabs", order = 0)]
public class RoomPrefabs : ScriptableObject
{
    public List<GameObject> topDoorRooms;
    public List<GameObject> bottomDoorRooms;
    public List<GameObject> leftDoorRooms;
    public List<GameObject> rightDoorRooms;

    public List<GameObject> GetRoomListFromDirection(ExitDirection direction)
    {
        switch (direction)
        {
            case ExitDirection.Top:
                return topDoorRooms;
            case ExitDirection.Bot:
                return bottomDoorRooms;
            case ExitDirection.Left:
                return leftDoorRooms;
            case ExitDirection.Right:
                return rightDoorRooms;
            default:
                Debug.LogError("Weird ExitDirection. Can't answer to this! ExitDirection: " + direction);
                return null;
        }
    }
}