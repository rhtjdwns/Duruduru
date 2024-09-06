using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStat _stat;
    private PlayerView _view;

    private PlayerAttack _attack;
    private PlayerController _controller;

    private Rigidbody _rb;
    private Animator _ani;

    [SerializeField] private Define.PlayerState _currentState = Define.PlayerState.NONE;
    private Dictionary<Define.PlayerState, PlayerState> _stateStorage = new Dictionary<Define.PlayerState, PlayerState>();

    [SerializeField] private Transform _playerModel;

    public bool IsInvincible { get; set; } = false;

    [SerializeField] private Transform _rightSparkPoint;
    [SerializeField] private Transform _leftSparkPoint;

    [Header("움직임")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _blockLayer;

    [Header("공격")]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Transform _endPoint;    // 넉백 지점
    [SerializeField] private Vector3 _colliderSize;
    [SerializeField] private LayerMask _monsterLayer;

    [SerializeField] private List<TempoAttackData> _mainTempoAttackDatas;
    [SerializeField] private List<TempoAttackData> _pointTempoAttackDatas;


    public PlayerStat Stat { get { return _stat; } }
    public PlayerAttack Attack { get { return _attack; } }
    public PlayerController Controller { get { return _controller; } }
    public Rigidbody Rb { get { return _rb; } }
    public Animator Ani { get { return _ani; } }
    public Define.PlayerState CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _stateStorage[_currentState]?.Exit();
            _currentState = value;
            _stateStorage[_currentState]?.Enter();
        }
    }
   
    public Transform PlayerModel { get => _playerModel; }
    public Transform RightSparkPoint { get => _rightSparkPoint; }
    public Transform LeftSparkPoint { get => _leftSparkPoint; }
    public Transform GroundCheckPoint { get => _groundCheckPoint; }
    public float GroundCheckRadius { get => _groundCheckRadius; }
    public LayerMask GroundLayer { get => _groundLayer; }
    public LayerMask WallLayer { get => _wallLayer; }
    public LayerMask BlockLayer { get => _blockLayer; }

    public Transform HitPoint { get => _hitPoint; }
    public Transform EndPoint { get => _endPoint; }
    public Vector3 ColliderSize { get => _colliderSize; }
    public LayerMask MonsterLayer { get => _monsterLayer; }
    public List<TempoAttackData> MainTempoAttackDatas { get => _mainTempoAttackDatas; }
    public List<TempoAttackData> PointTempoAttackDatas { get => _pointTempoAttackDatas; }

    private void Awake()
    {
        _view = GetComponent<PlayerView>();

        _attack = new PlayerAttack(this);
        _controller = new PlayerController(this);

        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();

        _stat.Init();
    }

    private void Start()
    {
        _attack.Initialize();
        _controller.Initialize();

        //플레이어 상태
        _stateStorage.Add(Define.PlayerState.NONE, new NoneState(this));
        _stateStorage.Add(Define.PlayerState.OVERLOAD, new OverloadState(this));
        _stateStorage.Add(Define.PlayerState.STUN, new StunState(this));
    }

    private void Update()
    {
        _stateStorage[_currentState]?.Stay();

        switch (_currentState)
        {
            case Define.PlayerState.STUN:
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                //_attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
                break;
            case Define.PlayerState.OVERLOAD:
            case Define.PlayerState.NONE:
                //_atkStateStorage[_curAtkState]?.Stay();
                _attack.Update();
                _controller.Update();

                break;
        }

    }

    public float GetTotalDamage(bool value = true)
    {
        if (value)
        {
            return _stat.Damage + _attack.CurrentTempoData.maxDamage;
        }
        else
        {
            return _stat.Damage + _attack.CurrentTempoData.minDamage;
        }   
    }

    public void TakeDamage(float value)
    {
        if (_stat.IsKnockedBack) return;

        _stat.Hp -= value * ((100 - _stat.Defense) / 100);
        UpdateHealth();
    }

    //넉백 함수
    public void Knockback(Vector3 point, float t = 0)
    {
        transform.DOMove(point,t);
    }
    // 넉백 시작
 

    public void Heal(float value)
    {
        _stat.Hp += value;
        UpdateHealth();
    }

    public void PowerUp(float value)
    {
        _stat.Damage += value;
    }

    // 과부화 상태인지 확인(스테미너가 최대 스테미나랑 같을 때)
    public bool CheckOverload()
    {
        if (_stat.Stamina == _stat.MaxStamina)
        {
            return true;
        }
        return false;
    }

    #region View
    public void UpdateHealth()
    {
        _view.UpdateHpBar(_stat.Hp / _stat.MaxHp);
    }
    public void UpdateStamina()
    {
        _view.UpdateStaminaBar(_stat.Stamina / _stat.MaxStamina);
    }
    public void UpdateUpgradeCount()
    {
        _view.UpdateUpgradeCountSlider(_attack.UpgradeCount);
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
