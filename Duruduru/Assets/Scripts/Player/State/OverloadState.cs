using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadState : PlayerState
{
    private float timer = 0;

    public OverloadState(Player player) : base(player)
    {
        _player = player;
    }

    public override void Enter()
    {
        timer = 0;
    }

    public override void Stay()
    {      
        if (timer < _player.Stat.StunDelay)
        {
            if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
            {               
               // Debug.Log("����ȭ �ð� ������.....");
                timer += Time.deltaTime;
            }

            if (!_player.CheckOverload()) // ����ȭ ���°� �ƴ� ��
            {
                //Debug.Log("Ż��");
                _player.CurrentState = Define.PlayerState.NONE;
            }

        }
        else
        {
            _player.CurrentState = Define.PlayerState.STUN;
        }
    }

    public override void Exit()
    {
       
    }
}
