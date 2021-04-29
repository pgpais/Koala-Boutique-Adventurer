
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTestingManager : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        if (IsGlobal())
        {
            DestroyLocalTestingObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyLocalTestingObjects()
    {
        foreach (var obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }

    bool IsGlobal()
    {
        return false;
    }
}
