using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public List<AIBrain> EnemiesToSpawn => enemiesToSpawn;
    public List<AIBrain> UnlockedEnemiesToSpawn => enemiesToSpawn.FindAll((enemy) => enemy.GetComponent<UnlockableCharacter>().IsUnlocked());

    [SerializeField] List<AIBrain> enemiesToSpawn = new List<AIBrain>();
}
