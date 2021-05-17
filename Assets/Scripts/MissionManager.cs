using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using static RoomEntrances;
using Random = System.Random;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public static UnityEvent<float> MissionStarted = new UnityEvent<float>();

    [field: SerializeField] public float LanternTimeLimit { get; private set; } = 300f;


    [field: SerializeField] public int Seed { get; private set; } = 100;
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
    public Random Rand { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        remainingExitsToCreate = howManyRooms;
        initialRoomTransform = initialRoom.transform;
        unspawnedExits = new Queue<Exit>();
        roomMap = new Room[howManyRooms * 2, howManyRooms * 2]; //! Can be terrible memory eater
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            Seed = GameManager.instance.CurrentMission.seed;
        }

        Rand = new Random(Seed);
        GenerateMap();

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        MissionStarted.Invoke(LanternTimeLimit);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinishLevel()
    {
        //! DEBUG
        InventoryManager.instance.AddRandomValuable();


        InventoryManager.instance.AddInventoryToGlobalItems();

        if (GameManager.instance != null)
        {
            GameManager.instance.FinishLevel();
        }

    }

    void GenerateMap()
    {
        roomMap[howManyRooms / 2, howManyRooms / 2] = initialRoom;
        var initialRoomEntrances = initialRoom.GetComponent<RoomEntrances>();
        initialRoomEntrances.x = howManyRooms / 2;
        initialRoomEntrances.y = howManyRooms / 2;

        foreach (var exit in initialRoomEntrances.Exits)
        {
            exit.SetCoordinates(initialRoomEntrances.x, initialRoomEntrances.y);
            unspawnedExits.Enqueue(exit);
        }
        remainingExitsToCreate -= 4;

        while (unspawnedExits.Count > 0)
        {
            Exit curExit = unspawnedExits.Dequeue();
            // Debug.Log("Doing exit " + curExit.ExitDirection);

            GameObject roomToSpawn = GetRoomForExit(curExit);

            if (roomMap[curExit.x, curExit.y] != null)
            {
                // skip exit
                // TODO:  Deactivate teleporter object?
                curExit.Teleporter.gameObject.SetActive(false);
                remainingExitsToCreate++;
                // Debug.Log($"ROOM COLLISION AT ({curExit.x}, {curExit.y})");
                continue;
            }
            else
            {
                // Add room at exit
                RoomEntrances newRoomEntrances = SpawnRoom(roomToSpawn, curExit);
                newRoomEntrances.x = curExit.x;
                newRoomEntrances.y = curExit.y;
                roomMap[curExit.x, curExit.y] = newRoomEntrances.GetComponent<Room>();

                remainingExitsToCreate -= newRoomEntrances.NExits - 1;
                if (remainingExitsToCreate < 0)
                    Debug.LogError("smth went worng, negative exits to creat");

                foreach (var newRoomExit in newRoomEntrances.Exits)
                {
                    if (!newRoomExit.gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (newRoomExit.ExitDirection == curExit.RequiredEntranceDirection)
                    {
                        curExit.Teleporter.Destination = newRoomExit.Teleporter;
                        curExit.Teleporter.TargetRoom = newRoomExit.Teleporter.CurrentRoom;

                        newRoomExit.Teleporter.Destination = curExit.Teleporter;
                        newRoomExit.Teleporter.TargetRoom = curExit.Teleporter.CurrentRoom;
                        continue;
                    }

                    newRoomExit.SetCoordinates(newRoomEntrances.x, newRoomEntrances.y);
                    unspawnedExits.Enqueue(newRoomExit);
                    // Debug.Log("Exit in prevRoom: " + curExit.ExitDirection + " | Exit in nextRoom: " + exit.ExitDirection + " | reqDirection: " + curExit.RequiredEntranceDirection);
                }
            }
        }
    }

    private GameObject GetRoomForExit(Exit curExit)
    {
        ExitDirection requiredDirection = curExit.RequiredEntranceDirection;

        List<GameObject> roomList = roomPrefabs.GetRoomListFromDirection(requiredDirection);

        // TODO: Check if matrix spot is occupied
        return SelectRoomFromList(roomList);
    }

    private GameObject SelectRoomFromList(List<GameObject> roomList)
    {
        int startingIndex = Rand.Next(roomList.Count);

        int i = 0;
        for (i = 0; i < roomList.Count; i++)
        {
            GameObject room = roomList[(i + startingIndex) % roomList.Count];

            int nRoomAdittionalExits = room.GetComponent<RoomEntrances>().NExits - 1; //-1 because one of the exits is already connected
            // int nRoomAdittionalExits = 5;

            // Debug.Log("chose room at index: " + ((i + startingPoint) % roomList.Count) + " | nexits = " + nRoomAdittionalExits);

            if (remainingExitsToCreate <= howManyDeadEnds)
            {

                List<GameObject> deadEnds = roomList.FindAll(delegate (GameObject obj)
                {
                    RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                    return entrances.NExits == 1;
                });

                return SelectDeadEnd(deadEnds);
            }
            else
            if (nRoomAdittionalExits <= remainingExitsToCreate && nRoomAdittionalExits > 0)
            {
                // TODO: Check for exit collision with other rooms
                // Debug.Log("Spawning normal room | nexits = " + nRoomAdittionalExits);
                return room;
            }
            else
            {
                continue;
            }
        }

        Debug.LogError("Couldn't find any room to fit this exit. Why? Stopped at index: " + ((i + startingIndex) % roomList.Count) + " | remianingExits: " + remainingExitsToCreate);
        return null;
    }

    private GameObject SelectDeadEnd(List<GameObject> deadEndList)
    {
        // TODO: Make conditions for how many of each room type should show up.
        //? (one exit; distance from center; how many valuables;) 

        return deadEndList.Find(delegate (GameObject obj)
                {
                    RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                    return entrances.Type == RoomType.Exit;
                });
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
