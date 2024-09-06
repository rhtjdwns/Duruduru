using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Phase2 : Elite_PhaseState
{
    public Elite_Phase2(ElitePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Phase2Monster.gameObject.SetActive(true);
        _manager.Phase2Monster.Enter();
    }
    public override void Stay()
    {

        if (_manager.Phase2Monster.Stat.Hp > 0)
        {
            if (_manager.Phase2Monster.Stat.Hp <= _manager.TargetHealthList[_manager.TargetHealthIndex])
            {
                if (_manager.Phase2Monster.CurrentState == Define.EliteMonsterState.IDLE)
                {
                    _manager.Phase2Monster.ReadySkill(Define.EliteMonsterSkill.THUNDERSTROKE);
                    _manager.TargetHealthIndex++;
                }

            }

            _manager.Phase2Monster.Stay();
        }
        else
        {
            _manager.Phase2Monster.FinishSkill();

            _manager.ChangeStageState(Define.ElitePhaseState.FINISH);
            _manager.Phase2Monster.ChangeCurrentState(Define.EliteMonsterState.DIE);
        }     
    }

    public override void Exit()
    {
    }

   
}
