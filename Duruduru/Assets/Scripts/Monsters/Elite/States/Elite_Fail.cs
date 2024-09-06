using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Fail : Elite_State
{
    private float timer;

    public Elite_Fail(EliteMonster monster) : base(monster)
    {
        timer = 0;
    }

    public override void Enter()
    {
        _monster.Ani.SetBool("Fail", true);
    }
    public override void Stay()
    {
        if (timer >= _monster.FailTime)
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
        _monster.Ani.SetBool("Fail", false);
        timer = 0;
    }

    private IEnumerator StartFailAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        _monster.Ani.SetBool("Fail", true);
    }
}
