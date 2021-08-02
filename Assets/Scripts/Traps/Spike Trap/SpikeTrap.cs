using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SpikeTrap : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float animationOffset;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("animOffset", animationOffset);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
