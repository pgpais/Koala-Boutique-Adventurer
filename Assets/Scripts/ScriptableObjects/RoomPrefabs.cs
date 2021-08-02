using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RoomEntrances;

[CreateAssetMenu(fileName = "RoomPrefabs", menuName = "Ye Olde Shop/RoomPrefabs", order = 0)]
public class RoomPrefabs : ScriptableObject
{
    public List<RoomEntrances> roomEntrances;

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

    private void OnValidate()
    {
        topDoorRooms = roomEntrances.Where(roomEntrance =>
        {

            List<Exit> roomExits = roomEntrance.Exits;
            return roomExits.Any(exit => exit.ExitDirection == ExitDirection.Top && exit.gameObject.activeSelf);
        }).Select(roomEntrance => roomEntrance.gameObject).ToList();

        bottomDoorRooms = roomEntrances.Where(roomEntrance =>
        {
            List<Exit> roomExits = roomEntrance.Exits;
            return roomExits.Any(exit => exit.ExitDirection == ExitDirection.Bot && exit.gameObject.activeSelf);
        }).Select(roomEntrance => roomEntrance.gameObject).ToList();

        leftDoorRooms = roomEntrances.Where(roomEntrance =>
        {
            List<Exit> roomExits = roomEntrance.Exits;
            return roomExits.Any(exit => exit.ExitDirection == ExitDirection.Left && exit.gameObject.activeSelf);
        }).Select(roomEntrance => roomEntrance.gameObject).ToList();

        rightDoorRooms = roomEntrances.Where(roomEntrance =>
        {
            List<Exit> roomExits = roomEntrance.Exits;
            return roomExits.Any(exit => exit.ExitDirection == ExitDirection.Right && exit.gameObject.activeSelf);
        }).Select(roomEntrance => roomEntrance.gameObject).ToList();
    }
}