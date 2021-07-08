using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AIDecisionGamePaused : AIDecision
{
    public override bool Decide()
    {
        return MoreMountains.TopDownEngine.GameManager.Instance.Paused;
    }
}
