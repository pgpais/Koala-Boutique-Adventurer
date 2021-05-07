using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text seedText;
    // Start is called before the first frame update
    void Start()
    {
        seedText.text = "Seed: " + MissionManager.instance.Seed.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
