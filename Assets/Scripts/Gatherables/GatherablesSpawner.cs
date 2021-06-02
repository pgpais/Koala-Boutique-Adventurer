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
    [SerializeField] Transform spawnersParent;

    [Space]

    [SerializeField] List<Item> gatherablesToSpawn;

    private List<Transform> spawners;



    // Start is called before the first frame update
    void Start()
    {
        if (!spawnGatherablesInThisRoom)
        {
            Destroy(this);
        }

        spawners = new List<Transform>();
        foreach (Transform child in spawnersParent)
        {
            // Debug.Log("spawning gatherables");
            if (child.gameObject.activeSelf)
            {
                spawners.Add(child);
            }
        }

        if (spawners.Count <= 0)
        {
            Debug.LogWarning("No gatherable spawners found! This script will not work this way!", this);
            return;
        }

        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnGatherables);
    }

    private void SpawnGatherables()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform spawner in spawners)
        {
            Debug.Log("spawning gatherables");

            var item = gatherablesToSpawn[rand.Next(0, gatherablesToSpawn.Count)];
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
    }
}
