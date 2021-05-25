using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class GatherablesSpawner : MonoBehaviour
{
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
        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnGatherables);

    }

    private void SpawnGatherables()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform child in spawnersParent)
        {
            Debug.Log("spawning gatherables");
            if (child.gameObject.activeSelf)
            {
                var item = gatherablesToSpawn[rand.Next(0, gatherablesToSpawn.Count)];
                Gatherable gatherable = Instantiate(prefabToSpawn, child.position, child.rotation);
                gatherable.Init(item);
                gatherable.transform.parent = room.transform;
            }
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
