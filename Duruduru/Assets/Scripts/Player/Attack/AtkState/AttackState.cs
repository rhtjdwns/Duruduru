using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerAttackState
{
    public AttackState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }
    public override void Enter()
    {
        _player.Ani.SetBool("AttackState", true);

        if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
        {
            _player.Ani.SetInteger("AtkCount", _player.Attack.CurrentTempoData.attackNumber);
        }
    
    }
    public override void Stay()
    {

    }
    public override void Exit()
    {
        if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
        {
            if (_player.Attack.CurrentTempoData.attackNumber == 3)
            {
                _player.Attack.CreateTempoCircle();
            }

        }
    
        _player.Ani.SetBool("AttackState", false);
    }
}
