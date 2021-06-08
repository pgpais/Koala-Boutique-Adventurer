using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [field: SerializeField] public Transform Spawner { get; private set; }

    [SerializeField] BuffPickable buffPickablePrefab;

    [SerializeField] bool spawnOnStart;

    private void Start()
    {

        var entrances = GetComponent<RoomEntrances>();
        entrances.RoomGenerated.AddListener(Initialize);
        Debug.Log("listener");
    }

    public void Initialize()
    {
        var rand = MissionManager.instance.Rand;
        var missionManager = MissionManager.instance;
        SpawnBuff(missionManager.BuffList.buffs[rand.Next(0, missionManager.BuffList.buffs.Count)]);
    }

    void SpawnBuff(Buff buff)
    {
        Instantiate(buffPickablePrefab, Spawner).buffToGive = buff;
    }
}
