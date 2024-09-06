using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperJump", menuName = "ScriptableObjects/Buff Data/Super Jump")]
public class SuperJumpBuff : BuffData
{
    private float _originValue; // 기존 공격력 값

    public override void Enter()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

        // 점프력 증가
        _originValue = _player.Stat.JumpForce;
        _player.Stat.JumpForce = value;
    }

    public override void Stay()
    {
      
    }

    public override void Exit()
    {
        _player.Stat.JumpForce = _originValue; // 점프력 증가
    }
}
