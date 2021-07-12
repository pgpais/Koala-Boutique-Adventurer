using UnityEngine;
using UnityEngine.UI;

public class SecretDoorUI : MonoBehaviour
{

    [SerializeField] InputField passCodeField;

    SecretDoor secretDoor;

    void Init(SecretDoor secretDoor)
    {
        this.secretDoor = secretDoor;
    }
}