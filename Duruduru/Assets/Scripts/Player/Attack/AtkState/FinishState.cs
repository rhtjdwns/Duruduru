using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishState : PlayerAttackState
{
    public FinishState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }

    public override void Enter()
    {
        _player.Ani.SetBool("FinishState", true);

        _player.Attack.ResetMainTempoQueue(); // ���� ���� ť �ʱ�ȭ

    }

    public override void Stay()
    {

    }

    public override void Exit()
    {

        _player.Ani.SetBool("FinishState", false);
    }
}
