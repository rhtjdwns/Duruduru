using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAttack
{
    #region ����
    private Player _player;

    private Define.AttackState _currentAttackState;
    private Dictionary<Define.AttackState, PlayerAttackState> _attackStateStorage;

    private Queue<TempoAttackData> _mainTempoQueue;
    private TempoAttackData _currentTempoData;

    private int _upgradeCount;

    public TempoCircle PointTempoCircle { get; set; }

    // �̺�Ʈ
    public bool IsHit { get; set; }
    public float CheckDelay { get; set; } // üũ ���� ���� �ð�
    #endregion

    #region ������Ƽ

    public Define.AttackState CurrentAttackkState { get => _currentAttackState; }
  
    public TempoAttackData CurrentTempoData { get=> _currentTempoData; }// ���� ���� ������ 
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

        //�÷��̾� ���� ����
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
        // ����ȭ üũ
        if (_player.CheckOverload()) 
        {
            if (_player.CurrentState == Define.PlayerState.NONE)
            {
                _player.CurrentState = Define.PlayerState.OVERLOAD;
            }
        }

       
        if (_currentAttackState != Define.AttackState.ATTACK)
        { 
            // ���� Ű �Է�
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


    #region ���� ����
    public void AttackMainTempo() // ���� ����
    {
        if (_player.Ani.GetBool("isGrounded"))
        {         
            _currentTempoData = _mainTempoQueue.Dequeue();

            ChangeCurrentAttackState(Define.AttackState.ATTACK);

            // ť�� ��������� �ʱ�ȭ
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

    #region ����Ʈ ����

    // ����Ʈ ���� ����
    public void AttackPointTempo()
    {
        // ����Ʈ ���� ���׷��̵� ���� Ȯ��
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

    #region ���� ��Ŭ

    // ���� �� ����
    public void CreateTempoCircle()
    {
        if (PointTempoCircle != null) return;

        SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Ready", _player.transform);

        GameObject tempoCircle = ObjectPool.Instance.Spawn("TempoCircle", 0, _player.transform);
        tempoCircle.transform.position = _player.transform.position + new Vector3(0, 1, -0.1f);

        PointTempoCircle = tempoCircle.GetComponent<TempoCircle>();
        PointTempoCircle.Init(_player.transform);           // ���� �� �ʱ�ȭ

        PointTempoCircle.ShrinkDuration = 1;        // ���� �� �ð� �� �߰�

        PointTempoCircle.SetTempoCircleAction(SuccessTempoCircle, FailureTempoCircle, FinishTempoCircle);
    }


   

    // ���� ��Ŭ ����
    private void SuccessTempoCircle()
    {
        AttackPointTempo();
        ResetMainTempoQueue();
    }

    // ���� ��Ŭ ����
    private void FailureTempoCircle()
    {
        PointTempoCircle = null;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // ���� ��Ŭ ��
    private void FinishTempoCircle()
    {

    }
    #endregion



}
