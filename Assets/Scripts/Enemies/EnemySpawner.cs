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

    private List<Character> enemies;

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

        enemies = new List<Character>();

        room.GetComponent<RoomEntrances>().RoomGenerated.AddListener(SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        System.Random rand = MissionManager.instance.Rand;

        foreach (Transform child in spawnersParent)
        {
            if (child.gameObject.activeSelf)
            {
                Character enemy = Instantiate(enemiesPrefabs[rand.Next(0, enemiesPrefabs.Count)], child.position, child.rotation).GetComponent<Character>();
                enemies.Add(enemy);
                enemy.transform.parent = room.transform;
            }
        }

        DisableAI();

        room.OnPlayerEntersRoomForTheFirstTime.AddListener(OnPlayerEnteredRoom);
        room.OnPlayerEntersRoom.AddListener(OnPlayerEnteredRoom);
        room.OnPlayerExitsRoom.AddListener(OnPlayerLeftRoom);
    }

    void OnPlayerEnteredRoom()
    {
        EnableAI();
    }

    void OnPlayerLeftRoom()
    {
        DisableAI();
    }

    void EnableAI()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead) { continue; }

            enemy.UnFreeze();
            var enemyBrain = enemy.CharacterBrain;
            enemyBrain.BrainActive = true;
            enemyBrain.ResetBrain();
        }
    }

    void DisableAI()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead) { continue; }

            enemy.Freeze();
            var enemyBrain = enemy.CharacterBrain;
            enemyBrain.ResetBrain();
            enemyBrain.BrainActive = false;
        }
    }

            enemy.GetComponent<Character>().Freeze();
            enemy.ResetBrain();
            enemy.BrainActive = false;
        }
    }
}