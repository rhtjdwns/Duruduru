using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_UseSkill : Elite_State
{
    public Elite_UseSkill(EliteMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
    }
    public override void Stay()
    {
        // 현재 조건 완료된 스킬 실행

       _monster.CurrentSkill?.Stay();
    }

    public override void Exit()
    {
        _monster.ChangeCurrentSkill(Define.EliteMonsterSkill.NONE);
        Debug.Log("Skill 나감");
    }
}
