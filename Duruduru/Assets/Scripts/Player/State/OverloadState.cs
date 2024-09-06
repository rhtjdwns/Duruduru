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
               // Debug.Log("과부화 시간 측정중.....");
                timer += Time.deltaTime;
            }

            if (!_player.CheckOverload()) // 과부화 상태가 아닐 때
            {
                //Debug.Log("탈출");
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
