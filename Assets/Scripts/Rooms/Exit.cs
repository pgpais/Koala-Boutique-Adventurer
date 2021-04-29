using MoreMountains.TopDownEngine;
using UnityEngine;
using static RoomEntrances;

[RequireComponent(typeof(Teleporter))]
public class Exit : MonoBehaviour
{
    public ExitDirection ExitDirection => exitDirection;
    [SerializeField] ExitDirection exitDirection;

    public Transform RoomSpawnPoint => roomSpawnPoint;
    [SerializeField] Transform roomSpawnPoint;

    public ExitDirection RequiredEntranceDirection => requiredEntranceDirection;
    private ExitDirection requiredEntranceDirection;

    private void Awake()
    {
        switch (exitDirection)
        {
            case ExitDirection.Top:
                requiredEntranceDirection = ExitDirection.Bot;
                break;
            case ExitDirection.Bot:
                requiredEntranceDirection = ExitDirection.Top;
                break;
            case ExitDirection.Left:
                requiredEntranceDirection = ExitDirection.Right;
                break;
            case ExitDirection.Right:
                requiredEntranceDirection = ExitDirection.Left;
                break;
            default:
                break;
        }

    }
}