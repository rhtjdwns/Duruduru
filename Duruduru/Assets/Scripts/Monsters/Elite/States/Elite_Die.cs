using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Die : Elite_State
{
    private float timer;

    public Elite_Die(EliteMonster monster) : base(monster)
    {
        timer = 0;
    }

    public override void Enter()
    {
        _monster.Ani.SetBool("Die", true);
    }
    public override void Stay()
    {
        
        if (timer >= _monster.DieTime)
        {
           
            _monster.gameObject.SetActive(false);
            _monster.ChangeCurrentState(Define.EliteMonsterState.NONE);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _monster.Ani.SetBool("Die", false);
        timer = 0;
    }
}
