using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static RoomEntrances;

public class MapRoomUI : MonoBehaviour
{
    [SerializeField] RoomTypeToIcon data;
    [SerializeField] Image roomIcon;
    [SerializeField] RoomType roomType;
    [SerializeField] bool explored;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(RoomType roomType)
    {
        if (data.TypeToIcon.ContainsKey(roomType))
            roomIcon.sprite = data.TypeToIcon[roomType];
        gameObject.SetActive(false);
    }

    public void ExploreRoom()
    {
        explored = true;
        gameObject.SetActive(true);
    }
}
