using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CharacterMovement : MoreMountains.TopDownEngine.CharacterMovement
{
    public override void ProcessAbility()
    {
        HandlePaused();
        base.ProcessAbility();
    }

    void HandlePaused()
    {
        //TODO: #65 Stop moving on death
        if (MoreMountains.TopDownEngine.GameManager.Instance.Paused)
        {
            _horizontalMovement = 0f;
            _verticalMovement = 0f;
            SetMovement();
        }
    }
}
