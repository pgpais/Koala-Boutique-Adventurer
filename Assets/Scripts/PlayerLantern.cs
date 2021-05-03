using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLantern : MonoBehaviour
{
    [Tooltip("How long the player has to finish the run (in seconds).")]
    [SerializeField] float timeLimit = 5f;
    [SerializeField] float maxLightRadius = 10f;

    float timeRemaining;

    Light2D lantern;

    // Start is called before the first frame update
    void Awake()
    {
        lantern = GetComponentInChildren<Light2D>();
        timeRemaining = timeLimit;

        if (lantern == null)
        {
            Debug.LogException(new MissingComponentException("This objects needs to have a point light in a child object! fix this!"));
        }
    }

    private void Start()
    {
        lantern.pointLightOuterRadius = maxLightRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Clamp(timeRemaining, 0, timeLimit);

            DimLight();
        }
    }

    void DimLight()
    {
        //TODO: use some sort of curve? (light dims faster when time is almost up)
        //? this is so the player doesn't spend a long time with the light radius just showing the player
        lantern.pointLightOuterRadius = (timeRemaining / timeLimit) * maxLightRadius;
    }
}
