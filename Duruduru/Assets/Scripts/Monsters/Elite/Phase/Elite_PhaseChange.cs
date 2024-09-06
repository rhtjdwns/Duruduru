using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_PhaseChange : Elite_PhaseState
{
    public Elite_PhaseChange(ElitePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Phase2Monster.Stat = _manager.Phase1Monster.Stat;
        _manager.Phase2Monster.Stat = _manager.Phase1Monster.Stat;

        _manager.Phase2Monster.gameObject.SetActive(true);
        _manager.ChangeStageState(Define.ElitePhaseState.PHASE2);
    }
    public override void Stay()
    {

    }

    public override void Exit()
    {

    }
}
