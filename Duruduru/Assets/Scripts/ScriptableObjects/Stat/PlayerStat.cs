using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObjects/Stat/Player Stat")]
public class PlayerStat : Stat
{

    [Header("점프")]
    [SerializeField] private float _jumpForce;// 점프 힘

    [Header("대쉬")]
    [SerializeField] private float _dashDelay = 5f;
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private float _dashDuration = 0.2f;

    [Header("스태미나")]
    [SerializeField] private float _maxStamina;
    private float _stamina;

    [Header("스턴")]
    [SerializeField] private float _stunDelay; // 과부화 시 스턴까지 걸리는 시간
    [SerializeField] private float _stunTime; // 스턴 상태 시간
 
    public bool IsKnockedBack { get; set; } = false;

    public float Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
                _isDead = true;
            }
            else if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }
    }

    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public float DashDelay { get => _dashDelay; }
    public float DashDistance { get => _dashDistance; }
    public float DashDuration { get => _dashDuration; }
    public float MaxStamina { get => _maxStamina; }
    public float Stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = value;

            if (_stamina >= _maxStamina)
            {
                _stamina = _maxStamina;
            }
            else if (_stamina < 0)
            {
                _stamina = 0;
            }
        }
    }
    public float StunDelay { get => _stunDelay; }// 스턴 상태 시간
    public float StunTime { get => _stunTime; }// 스턴 상태 시간

    public override void Init()
    {
        _hp = _maxHp;
        _stamina = 0;
    }

}
