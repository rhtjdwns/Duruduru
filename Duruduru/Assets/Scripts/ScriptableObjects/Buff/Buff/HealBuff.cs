using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Buff Data/Heal")]
public class HealBuff : BuffData
{
    public override void Enter()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

        Debug.Log("힐");
        _player.Heal(value);  // 체력 추가(힐)
    }

    public override void Stay()
    {
        BuffManager.Instance.RemoveBuff(Define.BuffInfo.HEAL); // 버프 제거
    }

    public override void Exit()
    {
        Platform.Change(Define.BuffInfo.NONE); // 버프 교체
    }
}
