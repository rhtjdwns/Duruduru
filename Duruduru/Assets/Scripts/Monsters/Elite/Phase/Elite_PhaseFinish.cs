using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_PhaseFinish : Elite_PhaseState
{
    public Elite_PhaseFinish(ElitePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {

    }
    public override void Stay()
    {
        if (_manager.Phase2Monster.gameObject.activeSelf)
        {
            _manager.Phase2Monster.Stay();
        }

    }

    public override void Exit()
    {
        
    }
}
