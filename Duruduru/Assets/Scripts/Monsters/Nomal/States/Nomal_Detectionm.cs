using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Detectionm : Nomal_State
{
    private float _attackTimer = 0;

    public Nomal_Detectionm(NomalMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("¹ß°¢");
        _attackTimer = 0;

    }
    public override void Stay()
    {

        if (_monster.Stat.AttackRange < Vector3.Distance(_monster.Player.position, _monster.transform.position))
        {
            _attackTimer = 0;
        }
        else
        {

            if (_attackTimer >= _monster.Stat.AttackDelay)
            {
                _monster.Attack();
                _attackTimer = 0;
            }
            else
            {
                _attackTimer += Time.deltaTime;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
