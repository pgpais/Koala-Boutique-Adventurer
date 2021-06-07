using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [field: SerializeField] public Transform Spawner { get; private set; }

    [SerializeField] BuffPickable buffPickablePrefab;

    public void Initialize(Buff buff)
    {
        Instantiate(buffPickablePrefab, transform).BuffToGive = buff;
    }
}
