using System.Collections;
using System.Collections.Generic;
using Cheese;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Transform teleporterParent;
    private List<Cheese.Teleporter> teleporters;

    // Start is called before the first frame update
    void Start()
    {
        SetupReferences();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupReferences()
    {
        foreach (var child in teleporterParent)
        {
            teleporters.Add(GetComponentInChildren<Cheese.Teleporter>());
        }
    }

    public void EnableTeleport(Direction direction, Cheese.Teleporter teleporter, Room room)
    {
        Cheese.Teleporter roomTeleporter;

        foreach (var tele in teleporters)
        {
            if (tele.direction == direction)
            {
                roomTeleporter = tele;
            }
        }
    }
}
