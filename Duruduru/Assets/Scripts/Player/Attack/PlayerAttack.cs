using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAttack
{
    #region 변수
    private Player _player;

    private Define.AttackState _currentAttackState;
    private Dictionary<Define.AttackState, PlayerAttackState> _attackStateStorage;

    private Queue<TempoAttackData> _mainTempoQueue;
    private TempoAttackData _currentTempoData;

    private int _upgradeCount;

    public TempoCircle PointTempoCircle { get; set; }

    // 이벤트
    public bool IsHit { get; set; }
    public float CheckDelay { get; set; } // 체크 상태 유지 시간
    #endregion

    #region 프로퍼티

    public Define.AttackState CurrentAttackkState { get => _currentAttackState; }
  
    public TempoAttackData CurrentTempoData { get=> _currentTempoData; }// 현재 템포 데이터 
    public int UpgradeCount
    {
        get => _upgradeCount;
        set
        {
            _upgradeCount = value;
            _upgradeCount = _upgradeCount % 4;

            if (_upgradeCount == 3)
            {
                _player.Ani.SetBool("IsUpgraded", true);
            }
            else
            {
                _player.Ani.SetBool("IsUpgraded", false);
            }

            _player.UpdateUpgradeCount();
        }
    }  
    #endregion

    public PlayerAttack(Player player)
    {
        _player = player;
    }

    public void Initialize()
    {
        _currentAttackState = Define.AttackState.FINISH;
        _attackStateStorage = new Dictionary<Define.AttackState, PlayerAttackState>();

        _mainTempoQueue = new Queue<TempoAttackData>();
        _currentTempoData = null;

        _upgradeCount = 0;
        PointTempoCircle = null;

        IsHit = false;

        CheckDelay = 0;

        //플레이어 공격 상태
        _attackStateStorage.Add(Define.AttackState.ATTACK, new AttackState(_player));
        _attackStateStorage.Add(Define.AttackState.CHECK, new CheckState(_player));
        _attackStateStorage.Add(Define.AttackState.FINISH, new FinishState(_player));

        foreach (var storage in _attackStateStorage)
        {
            storage.Value.Initialize();
        }

        ResetMainTempoQueue();
    }

    public void Update()
    {
        // 과부화 체크
        if (_player.CheckOverload()) 
        {
            if (_player.CurrentState == Define.PlayerState.NONE)
            {
                _player.CurrentState = Define.PlayerState.OVERLOAD;
            }
        }

       
        if (_currentAttackState != Define.AttackState.ATTACK)
        { 
            // 공격 키 입력
            if (Input.GetKeyDown(KeyCode.A))
            {
                AttackMainTempo();
            }
        }

        _attackStateStorage[_currentAttackState]?.Stay();
    }

    public void ChangeCurrentAttackState(Define.AttackState state)
    {
        _attackStateStorage[_currentAttackState]?.Exit();
        _currentAttackState = state;
        _attackStateStorage[_currentAttackState]?.Enter();
    }


    #region 메인 템포
    public void AttackMainTempo() // 공격 실행
    {
        if (_player.Ani.GetBool("isGrounded"))
        {         
            _currentTempoData = _mainTempoQueue.Dequeue();

            ChangeCurrentAttackState(Define.AttackState.ATTACK);

            // 큐가 비어있으면 초기화
            if (_mainTempoQueue.Count == 0)
            {
                ResetMainTempoQueue();
            }
        }
    }

    public void ResetMainTempoQueue()
    {
        _mainTempoQueue.Clear();

        foreach (TempoAttackData data in _player.MainTempoAttackDatas)
        {
            _mainTempoQueue.Enqueue(data);
        }
    }

    #endregion

    #region 포인트 템포

    // 포인트 템포 실행
    public void AttackPointTempo()
    {
        // 포인트 템포 업그레이드 상태 확인
        if (_player.Ani.GetBool("IsUpgraded"))
        {
            _currentTempoData = _player.PointTempoAttackDatas[1];
        }
        else
        {
            _currentTempoData = _player.PointTempoAttackDatas[0];
        }

        _player.Ani.SetTrigger("PointTempo");
        ChangeCurrentAttackState(Define.AttackState.ATTACK);
    }
    #endregion

    #region 템포 서클

    // 템포 원 생성
    public void CreateTempoCircle()
    {
        if (PointTempoCircle != null) return;

        SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Ready", _player.transform);

        GameObject tempoCircle = ObjectPool.Instance.Spawn("TempoCircle", 0, _player.transform);
        tempoCircle.transform.position = _player.transform.position + new Vector3(0, 1, -0.1f);

        PointTempoCircle = tempoCircle.GetComponent<TempoCircle>();
        PointTempoCircle.Init(_player.transform);           // 템포 원 초기화

        PointTempoCircle.ShrinkDuration = 1;        // 탬포 원 시간 값 추가

        PointTempoCircle.SetTempoCircleAction(SuccessTempoCircle, FailureTempoCircle, FinishTempoCircle);
    }


   

    // 템포 서클 성공
    private void SuccessTempoCircle()
    {
        AttackPointTempo();
        ResetMainTempoQueue();
    }

    // 템포 서클 실패
    private void FailureTempoCircle()
    {
        PointTempoCircle = null;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // 템포 서클 끝
    private void FinishTempoCircle()
    {

    }
    #endregion



}
