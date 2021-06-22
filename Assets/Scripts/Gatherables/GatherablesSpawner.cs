using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class GatherablesSpawner : MonoBehaviour
{
    [SerializeField] bool spawnGatherablesInThisRoom = true;
    [SerializeField] Gatherable prefabToSpawn;

    [Space]

    [SerializeField] Room room;
    [SerializeField] Transform gatherablesSpawnersParent;
    [SerializeField] Transform mineablesSpawnersParent;

    [Space]

    [SerializeField] List<Item> gatherablesToSpawn;
    [SerializeField] List<Item> mineablesToSpawn;

    private List<Transform> gatherableSpawners;
    private List<Transform> mineableSpawners;



    // Start is called before the first frame update
    void Start()
    {
        if (!spawnGatherablesInThisRoom)
        {
            Destroy(this);
        }

        gatherableSpawners = new List<Transform>();
        foreach (Transform child in gatherablesSpawnersParent)
        {
            // Debug.Log("spawning gatherables");
            if (child.gameObject.activeSelf)
            {
                gatherableSpawners.Add(child);
            }
        }

        mineableSpawners = new List<Transform>();
        foreach (Transform child in mineablesSpawnersParent)
        {
            // Debug.Log("spawning gatherables");
            if (child.gameObject.activeSelf)
            {
                mineableSpawners.Add(child);
            }
        }

        if (gatherableSpawners.Count <= 0 && mineableSpawners.Count <= 0)
        {
            Debug.LogWarning("No gatherable spawners found! This script will not work this way!", this);
            return;
        }

        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnGatherables);
        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnMineables);
    }

    private void SpawnGatherables()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform spawner in gatherableSpawners)
        {
            // Debug.Log("spawning gatherables");

            // var item = gatherablesToSpawn[rand.Next(0, gatherablesToSpawn.Count)];
            var item = ItemManager.instance.itemsData.GetItemByName(GameManager.instance.CurrentMission.gatherableItemName);
            Gatherable gatherable = Instantiate(prefabToSpawn, spawner.position, spawner.rotation);
            gatherable.Init(item);
            gatherable.transform.parent = room.transform;
        }
    }

    private void SpawnMineables()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform spawner in mineableSpawners)
        {
            // Debug.Log("spawning gatherables");

            var item = mineablesToSpawn[rand.Next(0, gatherablesToSpawn.Count)];
            Gatherable gatherable = Instantiate(prefabToSpawn, spawner.position, spawner.rotation);
            gatherable.Init(item);
            gatherable.transform.parent = room.transform;
        }
    }

    private void OnValidate()
    {
        foreach (var item in gatherablesToSpawn)
        {
            if (item.Type != Item.ItemType.Gatherable)
            {
                Debug.LogError($"Item {item.ItemName} is not of type Gatherable! Stopping execution...");
                if (Application.isPlaying)
                {
                    Application.Quit();
                }
            }
        }
        foreach (var item in mineablesToSpawn)
        {
            if (item.Type != Item.ItemType.Gatherable)
            {
                Debug.LogError($"Item {item.ItemName} is not of type Gatherable! Stopping execution...");
                if (Application.isPlaying)
                {
                    Application.Quit();
                }
            }
        }
    }
}
