using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] MapRoomUI roomUIPrefab;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] Transform roomParent;
    [SerializeField] RoomEntrances startingRoom;

    float roomWidth;
    float roomHeight;

    MapRoomUI[,] mapUI;

    private void Awake()
    {
        MissionManager.MissionStarted.AddListener(OnMissionStarted);
        RoomEntrances.RoomEntered.AddListener(OnRoomEntered);
    }

    private void Start()
    {
        // OnMissionStarted(0f);

    }

    private void OnMissionStarted(float missionTime)
    {
        CreateMapUI();
    }

    private void CreateMapUI()
    {
        Room[,] map = MissionManager.instance.RoomMap;
        mapUI = new MapRoomUI[map.GetLength(0), map.GetLength(1)];

        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    RoomEntrances entrances = map[i, j].GetComponent<RoomEntrances>();

                    MapRoomUI mapRoom = Instantiate(roomUIPrefab);
                    mapRoom.Init(entrances.Type);
                    mapUI[i, j] = mapRoom;
                    RectTransform roomRect = mapRoom.GetComponent<RectTransform>();
                    roomWidth = roomRect.rect.width;
                    roomHeight = roomRect.rect.height;
                    roomRect.SetParent(roomParent, false);


                    Vector2 centerDelta = new Vector2(entrances.x - startingRoom.x, entrances.y - startingRoom.y);
                    roomRect.anchoredPosition = new Vector3(roomWidth, roomHeight, 0) * centerDelta;

                    CreateDoors(mapRoom, entrances);
                }
            }
        }
        mapUI[startingRoom.x, startingRoom.y].ExploreRoom();
    }

    private void OnRoomEntered(RoomEntrances entrances)
    {
        roomParent.GetComponent<RectTransform>().anchoredPosition = new Vector2((startingRoom.x - entrances.x) * roomWidth, (startingRoom.y - entrances.y) * roomHeight);
        mapUI[entrances.x, entrances.y].ExploreRoom();
    }

    void CreateDoors(MapRoomUI mapRoom, RoomEntrances entrances)
    {
        RectTransform mapRoomRect = mapRoom.GetComponent<RectTransform>();

        foreach (var exit in entrances.ActiveExits)
        {
            if (!exit.gameObject.activeSelf)
            {
                return;
            }
            Vector2 posDelta = Vector2.zero;

            switch (exit.ExitDirection)
            {
                case RoomEntrances.ExitDirection.Left:
                    posDelta = new Vector2(-roomWidth / 2, 0);
                    break;
                case RoomEntrances.ExitDirection.Right:
                    posDelta = new Vector2(roomWidth / 2, 0);
                    break;
                case RoomEntrances.ExitDirection.Top:
                    posDelta = new Vector2(0, roomHeight / 2);
                    break;
                case RoomEntrances.ExitDirection.Bot:
                    posDelta = new Vector2(0, -roomHeight / 2);
                    break;
            }

            RectTransform doorRect = Instantiate(doorPrefab).GetComponent<RectTransform>();
            doorRect.SetParent(mapRoomRect, false);
            doorRect.anchoredPosition = posDelta;
        }
    }
}
