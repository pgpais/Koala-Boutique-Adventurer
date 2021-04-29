using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;


public class RoomEntrances : MonoBehaviour
{
    public enum ExitDirection
    {
        Top,
        Bot,
        Left,
        Right
    }

    // public int nEntrances => exitDirections.Count;
    // public int nEntrancesPrefab
    // {
    //     get
    //     {
    //         //! terrible way of doing it, can't this be stored? (maybe run script in editor? OnValidate?)
    //         int nExits = 0;
    //         foreach (var exit in Exits)
    //         {
    //             if (exit.gameObject.activeSelf)
    //             {
    //                 nExits++;
    //             }
    //         }
    //         return nExits;
    //     }
    // }

    public int NExits => nExits;
    [SerializeField] int nExits;

    public List<Exit> Exits => exits;
    [SerializeField] List<Exit> exits;

    Dictionary<ExitDirection, Teleporter> exitDirections;

    public bool HasDirection(ExitDirection direction) => exitDirections.ContainsKey(direction);
    public Teleporter GetTeleporterFromDirection(ExitDirection direction) => exitDirections[direction];

    private void Awake()
    {
        // exitDirections = new Dictionary<ExitDirection, Teleporter>();
        // AddExitsToDictionary();
    }

    private void AddExitsToDictionary()
    {
        foreach (var exit in exits)
        {
            if (exit.gameObject.activeSelf)
            {
                exitDirections.Add(exit.ExitDirection, exit.GetComponent<Teleporter>());
            }
        }
    }

    private void OnValidate()
    {
        if (exits.Count <= 0)
        {
            Debug.LogException(new Exception("No exits in this room. This shouldn't happen?"));
            return;
        }

        for (var i = 0; i < exits.Count; i++)
        {
            for (var j = 0; j < exits.Count; j++)
            {
                if (i == j) continue;

                if (exits[i].ExitDirection == exits[j].ExitDirection)
                {
                    Debug.LogException(new Exception("Room has two equal exit directions! This will lead to errors!"), this);
                    return;
                }
            }
        }
    }
}
