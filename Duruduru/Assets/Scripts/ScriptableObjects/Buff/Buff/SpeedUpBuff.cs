using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUp", menuName = "ScriptableObjects/Buff Data/Speed Up")]
public class SpeedUpBuff : BuffData
{
    [SerializeField] private float _duration;
    private float _timer = 0;

    private float _originValue; // 기존 공격력 값

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

        // 이속 증가
        _originValue = _player.Stat.SprintSpeed;
        _player.Stat.SprintSpeed = value;
    }

    public override void Stay()
    {
        if (_timer >= _duration)
        {
            BuffManager.Instance.RemoveBuff(Define.BuffInfo.SPEEDUP); // 버프 제거
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {
        _player.Stat.SprintSpeed = _originValue; // 공격력 복구
    }
}
