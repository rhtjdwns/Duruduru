using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elite_PhaseState 
{
    protected ElitePhaseManager _manager;
    public Elite_PhaseState(ElitePhaseManager manager)
    {
        _manager = manager;
    }

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
