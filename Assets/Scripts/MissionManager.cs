using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using static RoomEntrances;

public class MissionManager : MonoBehaviour
{
    [SerializeField] RoomPrefabs roomPrefabs;

    [Space]

    [SerializeField] Room initialRoom;

    [Tooltip("How many rooms should be in the map (including initial). Could be difficulty?")]
    [SerializeField] int howManyRooms = 6;
    [SerializeField] int howManyDeadEnds = 1;
    private Transform initialRoomTransform;
    private Room[,] roomMap;
    private Queue<Exit> unspawnedExits;
    private int remainingExitsToCreate;

    private void Awake()
    {
        remainingExitsToCreate = howManyRooms;
        initialRoomTransform = initialRoom.transform;
        unspawnedExits = new Queue<Exit>();
        roomMap = new Room[howManyRooms * 2, howManyRooms * 2]; //! Can be terrible memory eater
        roomMap[howManyRooms / 2, howManyRooms / 2] = initialRoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateMap()
    {
        foreach (var exit in initialRoom.GetComponent<RoomEntrances>().Exits)
        {
            unspawnedExits.Enqueue(exit);
        }
        remainingExitsToCreate -= 4;

        while (unspawnedExits.Count > 0)
        {
            Exit curExit = unspawnedExits.Dequeue();
            Debug.Log("Doing exit " + curExit.ExitDirection);

            GameObject roomToSpawn = GetRoomForExit(curExit);

            // TODO: Add to map matrix (maybe rooms should have coordinates?)
            RoomEntrances newRoomEntrances = SpawnRoom(roomToSpawn, curExit);

            remainingExitsToCreate -= newRoomEntrances.NExits - 1;
            if (remainingExitsToCreate < 0)
                Debug.LogError("smth went worng, negative exits to creat");

            foreach (var exit in newRoomEntrances.Exits)
            {
                if (exit.ExitDirection == curExit.RequiredEntranceDirection || !exit.gameObject.activeSelf)
                    continue;
                // if (unspawnedExits.Count < 2)
                unspawnedExits.Enqueue(exit);
                Debug.Log("Exit in prevRoom: " + curExit.ExitDirection + " | Exit in nextRoom: " + exit.ExitDirection + " | reqDirection: " + curExit.RequiredEntranceDirection);
            }
        }
    }

    private GameObject GetRoomForExit(Exit curExit)
    {
        ExitDirection requiredDirection = curExit.RequiredEntranceDirection;

        List<GameObject> roomList = roomPrefabs.GetRoomListFromDirection(requiredDirection);

        // TODO: Check if matrix spot is occupied
        return SelectRandomRoomFromList(roomList);
    }

    private GameObject SelectRandomRoomFromList(List<GameObject> roomList)
    {

        int startingPoint = Random.Range(0, roomList.Count - 1);
        int i = 0;
        for (i = 0; i < roomList.Count; i++)
        {
            GameObject room = roomList[(i + startingPoint) % roomList.Count];

            int nRoomAdittionalExits = room.GetComponent<RoomEntrances>().NExits - 1; //-1 because one of the exits is already connected
            // int nRoomAdittionalExits = 5;

            Debug.Log("chose room at index: " + ((i + startingPoint) % roomList.Count) + " | nexits = " + nRoomAdittionalExits);

            // if (remainingExitsToCreate <= howManyDeadEnds)
            // {
            //     Debug.Log("Spawning dead end | nexits = " + nRoomAdittionalExits);
            //     // Spawn dead end
            //     // TODO: Dead ends could be secrets?
            //     if (nRoomAdittionalExits > 0)
            //     {
            //         continue;
            //     }
            //     else
            //     {
            //         return room;
            //     }
            // }
            // else 
            if (nRoomAdittionalExits <= remainingExitsToCreate)
            {
                // TODO: Check for exit collision with other rooms
                Debug.Log("Spawning normal room | nexits = " + nRoomAdittionalExits);
                return room;
            }
            else
            {
                continue;
            }
        }

        Debug.LogError("Couldn't find any room to fit this exit. Why? Stopped at index: " + ((i + startingPoint) % roomList.Count) + " | remianingExits: " + remainingExitsToCreate);
        return null;
    }

    private RoomEntrances SpawnRoom(GameObject roomToSpawn, Exit connectedExit)
    {
        return Instantiate(roomToSpawn, connectedExit.RoomSpawnPoint.position, connectedExit.RoomSpawnPoint.rotation).GetComponent<RoomEntrances>();
    }

    private int CountRoomExits(RoomEntrances room)
    {
        int nExits = 0;
        foreach (var exit in room.Exits)
        {
            if (exit.gameObject.activeSelf)
            {
                nExits++;
            }
        }
        return nExits;
    }
}
