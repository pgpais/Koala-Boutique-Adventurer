using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] bool spawnEnemiesInRoom = true;

    [Space]

    [SerializeField] Room room;
    [SerializeField] Transform spawnersParent;

    [Space]

    [SerializeField] List<AIBrain> enemiesPrefabs;

    private
    List<AIBrain> enemies;

    private void Start()
    {
        if (!spawnEnemiesInRoom)
        {
            Destroy(this);
        }

        if (enemiesPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy spawners found! This script will not work this way!", this);
            return;
        }

        enemies = new List<AIBrain>();

        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform child in spawnersParent)
        {
            if (child.gameObject.activeSelf)
            {
                AIBrain enemy = Instantiate(enemiesPrefabs[rand.Next(0, enemiesPrefabs.Count)], child.position, child.rotation);
                enemies.Add(enemy);
                enemy.gameObject.SetActive(false);
                enemy.transform.parent = room.transform;
            }
        }

        room.OnPlayerEntersRoomForTheFirstTime.AddListener(OnPlayerEnteredRoom);
        room.OnPlayerEntersRoom.AddListener(OnPlayerEnteredRoom);
        room.OnPlayerExitsRoom.AddListener(OnPlayerLeftRoom);
    }

    void OnPlayerEnteredRoom()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    void OnPlayerLeftRoom()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}