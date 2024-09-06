using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Groggy : Elite_State
{
    private float timer;

    public Elite_Groggy(EliteMonster monster) : base(monster)
    {
        timer = 0;
    }

    public override void Enter()
    {
        _monster.Ani.SetBool("Stun", true);
    }
    public override void Stay()
    {
        if (timer >= _monster.GroggyTime)
        {
            _monster.ChangeCurrentState(Define.EliteMonsterState.IDLE);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _monster.Ani.SetBool("Stun", false);
        timer = 0;
    }
}
