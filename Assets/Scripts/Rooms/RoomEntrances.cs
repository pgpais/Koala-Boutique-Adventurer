using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;

public class RoomEntrances : MonoBehaviour
{
    [field: SerializeField] public UnityEvent RoomGenerated { get; private set; }

    public enum ExitDirection
    {
        Top,
        Bot,
        Left,
        Right
    }

    public enum RoomType
    {
        Default,
        Exit,
        Loot,
        Trap,
        Enemies,
        Secret,
        Buff,
        Healing

    }
    public RoomType Type => type;
    [SerializeField] RoomType type;

    public int x, y;

    public int NExits => nExits;
    [SerializeField] int nExits;

    public List<Exit> Exits => exits;
    [SerializeField] List<Exit> exits;

    Dictionary<ExitDirection, Teleporter> exitDirections;

    public bool HasDirection(ExitDirection direction) => exitDirections.ContainsKey(direction);
    public Teleporter GetTeleporterFromDirection(ExitDirection direction) => exitDirections[direction];

    private void Awake()
    {
        nExits = 0;
        foreach (var exit in exits)
        {
            if (exit.gameObject.activeSelf)
            {
                nExits++;
            }
        }

        exitDirections = new Dictionary<ExitDirection, Teleporter>();
        AddExitsToDictionary();

        RoomGenerated = new UnityEvent();
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        // Debug.Log("Room generated");
        RoomGenerated.Invoke();
    }

    private void AddExitsToDictionary()
    {
        foreach (var exit in exits)
        {
            exitDirections.Add(exit.ExitDirection, exit.GetComponent<Teleporter>());
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
