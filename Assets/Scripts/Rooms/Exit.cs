using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using static RoomEntrances;

[RequireComponent(typeof(Teleporter))]
public class Exit : MonoBehaviour
{
    public UnityEvent<Exit> ExitAdded;

    public int x, y;


    public ExitDirection ExitDirection => exitDirection;
    [SerializeField] ExitDirection exitDirection;

    public Transform RoomSpawnPoint => roomSpawnPoint;
    [SerializeField] Transform roomSpawnPoint;

    public ExitDirection RequiredEntranceDirection => requiredEntranceDirection;
    private ExitDirection requiredEntranceDirection;

    public Teleporter Teleporter => teleporter;
    Teleporter teleporter;

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
                Debug.LogError("This Exit has a weird direction! HALP", this);
                break;
        }

        teleporter = GetComponent<Teleporter>();

        ExitAdded = new UnityEvent<Exit>();
    }

    public void SetCoordinates(int roomX, int roomY)
    {
        switch (exitDirection)
        {
            case ExitDirection.Top:
                x = roomX;
                y = roomY + 1;
                break;
            case ExitDirection.Bot:
                x = roomX;
                y = roomY - 1;
                break;
            case ExitDirection.Left:
                x = roomX - 1;
                y = roomY;
                break;
            case ExitDirection.Right:
                x = roomX + 1;
                y = roomY;
                break;
            default:
                Debug.LogError("This Exit has a weird direction! HALP", this);
                break;
        }
    }

    public void EnableExit()
    {
        if (gameObject.activeSelf)
        {
            return;
        }
        else
        {
            gameObject.SetActive(true);
            ExitAdded.Invoke(this);
        }
    }
}