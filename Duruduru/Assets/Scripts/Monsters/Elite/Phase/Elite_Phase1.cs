using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Phase1 : Elite_PhaseState
{
    public Elite_Phase1(ElitePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Phase1Monster.Enter();
    }
    public override void Stay()
    {

        if (_manager.Phase1Monster.Stat.Hp <= _manager.TargetHealthList[_manager.TargetHealthIndex])
        {
            if (_manager.Phase1Monster.CurrentState == Define.EliteMonsterState.IDLE)
            {
                _manager.ChangeStageState(Define.ElitePhaseState.PHASECHANGE);
            }
        }
        else
        {
            _manager.Phase1Monster.Stay();
        }
    }

    public override void Exit()
    {
        _manager.Phase1Monster.gameObject.SetActive(false);
    }
}
