using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using static RoomEntrances;
using Random = System.Random;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public static UnityEvent<float> MissionStarted = new UnityEvent<float>();
    public static UnityEvent MissionEnded = new UnityEvent();

    public Room[,] RoomMap => roomMap;

    [field: SerializeField] public float LanternTimeLimit { get; private set; } = 300f;


    [field: SerializeField] public int Seed { get; private set; } = 100;
    [SerializeField] RoomPrefabs roomPrefabs;

    public BuffList BuffList => buffList;
    [SerializeField] BuffList buffList;

    [Space]

    [SerializeField] Room initialRoom;

    [Tooltip("How many rooms should be in the map (including initial). Could be difficulty?")]
    [SerializeField] int howManyRooms = 10;
    [SerializeField] int difficulty = 5;
    [Tooltip("How many more easy rooms should exist for every difficult room")]
    [SerializeField] int easyToDifficultRatio = 1;
    [SerializeField] int howManyDeadEnds = 1;
    private Transform initialRoomTransform;
    private Room[,] roomMap;

    private Queue<Exit> unspawnedExits;
    private int remainingExitsToCreate;
    private int currentDifficulty = 0;
    private int easyRooms = 0;
    private int difficultRooms = 0;
    public Random Rand { get; private set; }


    private int howManyMissionExitsCreated = 0;
    private int howManyHealingRoomsCreated = 0;
    private int howManyLootRoomsCreated = 0;
    private int howManyBuffRoomsCreated = 0;
    private int howManySecretRoomsCreated = 0;

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
        if (easyToDifficultRatio == 0)
        {
            difficulty = 0;
        }
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

        MissionEnded.Invoke();
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

            if (roomMap[curExit.x, curExit.y] != null) // Room already created at exit position
            {
                // Debug.Log($"Room at ({curExit.x}, {curExit.y}) = {roomMap[curExit.x, curExit.y]}");
                // Debug.Log($"Has direction {curExit.RequiredEntranceDirection}? {roomMap[curExit.x, curExit.y].GetComponent<RoomEntrances>().HasDirection(curExit.RequiredEntranceDirection)}");


                ResolveExitCollision(curExit, roomMap[curExit.x, curExit.y].GetComponent<RoomEntrances>());

                // Debug.Log($"ROOM COLLISION AT ({curExit.x}, {curExit.y})");
                continue;
            }
            else
            {
                GameObject roomToSpawn = GetRoomForExit(curExit);
                // Add room at exit
                RoomEntrances newRoomEntrances = SpawnRoom(roomToSpawn, curExit);




                Room newRoom = newRoomEntrances.GetComponent<Room>();
                newRoomEntrances.x = curExit.x;
                newRoomEntrances.y = curExit.y;
                roomMap[curExit.x, curExit.y] = newRoom;

                currentDifficulty += (int)newRoomEntrances.Difficulty;
                if (newRoomEntrances.Difficulty == RoomDifficulty.Easy)
                {
                    easyRooms++;
                }
                else
                {
                    difficultRooms++;
                }

                remainingExitsToCreate -= newRoomEntrances.NExits - 1;
                if (remainingExitsToCreate < 0)
                    Debug.LogError("smth went worng, negative exits to creat");

                foreach (var newRoomExit in newRoomEntrances.Exits)
                {
                    newRoomExit.SetCoordinates(newRoomEntrances.x, newRoomEntrances.y);

                    if (!newRoomExit.gameObject.activeSelf)
                    {
                        // if (roomMap[newRoomExit.x, newRoomExit.y] != null)
                        // {
                        //     ResolveExitCollision(newRoomExit, roomMap[newRoomExit.x, newRoomExit.y].GetComponent<RoomEntrances>());
                        // }
                        continue;
                    }

                    if (newRoomExit.ExitDirection == curExit.RequiredEntranceDirection)
                    {

                        SetupTeleporters(curExit.Teleporter, newRoomExit.Teleporter);
                        continue;
                    }

                    unspawnedExits.Enqueue(newRoomExit);
                    // Debug.Log("Exit in prevRoom: " + curExit.ExitDirection + " | Exit in nextRoom: " + exit.ExitDirection + " | reqDirection: " + curExit.RequiredEntranceDirection);
                }

                HandleNewRoomType(newRoomEntrances);
            }
        }
    }

    private void HandleNewRoomType(RoomEntrances newRoomEntrances)
    {
        switch (newRoomEntrances.Type)
        {
            case RoomType.Exit:
                howManyMissionExitsCreated++;
                break;
            case RoomType.Buff:
                howManyBuffRoomsCreated++;
                break;
            case RoomType.Healing:
                howManyHealingRoomsCreated++;
                break;
            case RoomType.Loot:
                howManyLootRoomsCreated++;
                break;
            case RoomType.Secret:
                howManySecretRoomsCreated++;
                HandleSecretRoom(newRoomEntrances);
                break;
            default:
                break;
        }

        if (newRoomEntrances.Type == RoomType.Exit)
        {
            howManyMissionExitsCreated++;
        }
    }

    private void HandleSecretRoom(RoomEntrances entrances)
    {
        List<Exit> activeExits = entrances.Exits.FindAll((exit) =>
        {
            return exit.gameObject.activeSelf;
        });

        if (activeExits.Count > 1)
        {
            Debug.LogWarning("For some reason secret room has more than one exit!");
        }

        Teleporter exitTP = activeExits[0].Teleporter;
        var room = exitTP.Destination.CurrentRoom;
        Exit destinationExit = exitTP.Destination.GetComponent<Exit>();

        Debug.Log($"{entrances.gameObject.name} has exit to room {exitTP.Destination.CurrentRoom.gameObject.name}");

        var health = destinationExit.gameObject.AddComponent<Health>();
        health.DestroyOnDeath = false;
        health.ChangeLayerOnDeath = false;
        health.DisableModelOnDeath = false;
        health.DisableCollisionsOnDeath = false;
        health.DisableControllerOnDeath = false;
        health.DisableChildCollisionsOnDeath = false;
        health.ChangeLayersRecursivelyOnDeath = false;

        destinationExit.EnableExit(false);
    }

    private void SetupTeleporters(Teleporter current, Teleporter destination)
    {
        current.Destination = destination;
        current.TargetRoom = destination.CurrentRoom;

        destination.Destination = current;
        destination.TargetRoom = current.CurrentRoom;
    }

    private void ResolveExitCollision(Exit curExit, RoomEntrances otherEntrances)
    {
        if (otherEntrances.NExits == 1)
        {
            // Debug.Log("Tried connecting room to exit. just return.");
            curExit.gameObject.SetActive(false);
            return;
        }

        // TODO: #11 Enable door if next to other room (Maybe some can be secret?)
        Teleporter otherTeleporter = otherEntrances.GetTeleporterFromDirection(curExit.RequiredEntranceDirection);
        otherTeleporter.GetComponent<Exit>().EnableExit();
        curExit.EnableExit();
        SetupTeleporters(curExit.Teleporter, otherTeleporter);
        remainingExitsToCreate++;
    }

    private GameObject GetRoomForExit(Exit curExit)
    {
        ExitDirection requiredDirection = curExit.RequiredEntranceDirection;

        List<GameObject> roomList = roomPrefabs.GetRoomListFromDirection(requiredDirection);

        return SelectRoomFromList(roomList);
    }

    private GameObject SelectRoomFromList(List<GameObject> roomList)
    {
        int startingIndex = Rand.Next(roomList.Count);

        // if (remainingExitsToCreate <= howManyDeadEnds)
        // {
        //     // Dead End should be picked
        //     List<GameObject> deadEnds = roomList.FindAll(
        //         (room) => room.GetComponent<RoomEntrances>().NExits == 1
        //     );
        //     SelectDeadEnd(deadEnds);
        // }
        // else 
        // {

        // }

        int i = 0;
        for (i = 0; i < roomList.Count; i++)
        {
            GameObject room = roomList[(i + startingIndex) % roomList.Count];
            RoomEntrances entrances = room.GetComponent<RoomEntrances>();

            int nRoomAdditionalExits = entrances.NExits - 1; //-1 because one of the exits is already connected

            // Debug.Log("chose room at index: " + ((i + startingPoint) % roomList.Count) + " | nexits = " + nRoomAdittionalExits);

            if (currentDifficulty >= difficulty && remainingExitsToCreate <= 0)
            {

                // Find rooms with one exit to connect to this exit
                List<GameObject> deadEnds = roomList.FindAll(delegate (GameObject obj)
                {
                    RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                    return entrances.NExits == 1;
                });

                return SelectDeadEnd(deadEnds);
            }
            // else if (nRoomAdditionalExits <= remainingExitsToCreate && nRoomAdditionalExits > 0)
            // {
            //     // Debug.Log("Spawning normal room | nexits = " + nRoomAdittionalExits);
            //     return room;
            // }
            else if (currentDifficulty < difficulty && difficultRooms < easyRooms * easyToDifficultRatio)
            {
                int remainingDifficulty = difficulty - currentDifficulty;
                if (remainingDifficulty < ((int)RoomDifficulty.Hard))
                {
                    // TODO: choose medium
                    if (entrances.Difficulty != RoomDifficulty.Medium)
                    {
                        continue;
                    }
                    else
                    {
                        return room;
                    }
                }
                else
                {
                    // TODO: choose medium or hard
                    if (entrances.Difficulty == RoomDifficulty.Easy)
                    {
                        continue;
                    }
                    else
                    {
                        return room;
                    }
                }
            }
            else
            {
                // TODO: Spawn easy room
                if (entrances.Difficulty == RoomDifficulty.Easy && nRoomAdditionalExits <= remainingExitsToCreate && nRoomAdditionalExits > 0)
                {
                    return room;
                }
                else
                {
                    continue;
                }
            }
        }

        Debug.LogError("Couldn't find any room to fit this exit. Why? Stopped at index: " + ((i + startingIndex) % roomList.Count) + " | remianingExits: " + remainingExitsToCreate);
        return null;
    }



    private GameObject SelectDeadEnd(List<GameObject> deadEndList)
    {
        // TODO: Make conditions for how many of each room type should show up.
        //? (one exit; distance from center; how many valuables;) 
        if (howManyMissionExitsCreated < 1)
        {
            return deadEndList.Find(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Exit;
                    });
        }
        else if (howManySecretRoomsCreated < 1)
        {
            return deadEndList.Find(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Secret;
                    });
        }
        else if (howManyHealingRoomsCreated < 1)
        {
            return deadEndList.Find(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Healing;
                    });
        }
        else if (howManyLootRoomsCreated < 1 || howManyLootRoomsCreated * 2 <= howManyBuffRoomsCreated)
        {
            return deadEndList.Find(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Loot;
                    });
        }
        else
        {
            var resultRooms = deadEndList.FindAll(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Buff && entrances.Unlocked;
                    });
            if (resultRooms.Count > 0)
            {
                return resultRooms[Rand.Next(0, resultRooms.Count)];
            }
        }

        return deadEndList.Find(delegate (GameObject obj)
                    {
                        RoomEntrances entrances = obj.GetComponent<RoomEntrances>();

                        return entrances.Type == RoomType.Secret;
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


    public List<string> GetStartingBuffs()
    {
        return GameManager.instance.CurrentMission.boughtBuffs;
    }
}
