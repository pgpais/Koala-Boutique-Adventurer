using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MoreMountains.TopDownEngine.InputManager
{
    protected override void LateUpdate()
    {
        base.LateUpdate();

        GetControlType();
    }

    void GetControlType()
    {
        if (UsingMouse())
        {
            WeaponForcedMode = MoreMountains.TopDownEngine.WeaponAim.AimControls.Mouse;
        }
        else if (UsingGamepad())
        {
            WeaponForcedMode = MoreMountains.TopDownEngine.WeaponAim.AimControls.SecondaryThenPrimaryMovement;
        }
    }

    bool UsingMouse() => Input.GetKeyDown(KeyCode.W) ||
        Input.GetKeyDown(KeyCode.A) ||
        Input.GetKeyDown(KeyCode.S) ||
        Input.GetKeyDown(KeyCode.D) ||
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Mouse0) ||
        Input.GetKeyDown(KeyCode.Mouse1);

    bool UsingGamepad() => Input.GetKeyDown(KeyCode.JoystickButton0) ||
        Input.GetKeyDown(KeyCode.JoystickButton1) ||
        Input.GetKeyDown(KeyCode.JoystickButton2) ||
        Input.GetKeyDown(KeyCode.JoystickButton3);
}
