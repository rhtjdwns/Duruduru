using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NomalMonster : Monster
{
    [System.Serializable]
    public class PerceptionRange
    {
        [Range(0, 100)]
        public int range_Ratio;
        public float gaugeIncrementPerSecond;
        public Color color;
    }

    #region 변수
    private INomalMonsterView _nomalMonsterView;

    [SerializeField] private float _moveRange; // Idle 이동 거리

    [Space]
    [SerializeField] private float _perceptionDistance;                                                                                    // 인식 거리
    [SerializeField] private float _perceptionAngle;                                                                                       // 인식 각도
    [SerializeField] private List<PerceptionRange> _perceptionRanges = new List<PerceptionRange>();                                        // 인식 범위
    private Dictionary<Define.PerceptionType, Nomal_State> _perceptionStateStorage = new Dictionary<Define.PerceptionType, Nomal_State>(); // 인식 상태 저장소
    private Define.PerceptionType _currentPerceptionState;                                                                                 // 현재 인색 상태 
    
    [SerializeField] private float _maxAggroGauge; // 최대 어그로 게이지
    private float _aggroGauge = 0;                 // 현재 어그로 게이지

    public Vector2 SpawnPoint { get; set; }

    [Space]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Vector3 _colliderSize;


    #endregion

    #region 프로퍼티
    public float MoveRange { get => _moveRange; }

    public float PerceptionDistance { get => _perceptionDistance; }
    public float PerceptionAngle { get => _perceptionAngle; }
    public List<PerceptionRange> PerceptionRanges { get => _perceptionRanges; }
    public Define.PerceptionType CurrentPerceptionState
    {
        get
        {
            return _currentPerceptionState;
        }
        set
        {
            if (_perceptionStateStorage.ContainsKey(_currentPerceptionState))
            {
                _perceptionStateStorage[_currentPerceptionState]?.Exit();
            }
            _currentPerceptionState = value;
            _perceptionStateStorage[_currentPerceptionState]?.Enter();
        }
    }

    public float MaxAggroGauge { get => _maxAggroGauge; }
    public float AggroGauge
    {
        get => _aggroGauge;
        set
        {

            if (value < 0)
            {
                _aggroGauge = 0;
            }
            else
            {
                _aggroGauge = value;

            }

            if (_aggroGauge > _maxAggroGauge)
            {
                _aggroGauge = _maxAggroGauge;
            }

        }
    }
   

    #endregion

    protected override void Init()
    {
        _nomalMonsterView = GetComponent<INomalMonsterView>();

        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>().transform;

        _stat.Init();
    }
    private void Start()
    {
        _perceptionStateStorage.Add(Define.PerceptionType.PATROL, new Nomal_Patrol(this));
        _perceptionStateStorage.Add(Define.PerceptionType.BOUNDARY, new Nomal_Boundary(this));
        _perceptionStateStorage.Add(Define.PerceptionType.DETECTIONM, new Nomal_Detectionm(this));

        CurrentPerceptionState = Define.PerceptionType.PATROL;

        _stat.Hp = _stat.MaxHp;

        // 넉백 시 실행하는 이벤트
        OnKnockback += () =>
        {
            float dir = _player.position.x - transform.position.x;
            Direction = dir;
        };

        
    }

    private void Update()
    {

        CheckPerceptionState(); // 게이지 증가 시키는 함수
        UpdatePerceptionState(); // 게이지 확인 후 인식 상태 업데이트

        // 인식 범위 안에 들어왔을 때
        if (_perceptionStateStorage[_currentPerceptionState].IsEntered)
        {
            _perceptionStateStorage[_currentPerceptionState]?.Stay();
        }

    }


    // 부채꼴 안에 플레이어가 있는지 확인
    private void CheckPerceptionState() 
    {
        // 기준점에서 플레이어로 향하는 벡터 계산

        Vector3 playerPos = new Vector3(_player.position.x, _player.position.y, _player.position.z);
        Vector3 directionToPlayer = playerPos - transform.position;
        directionToPlayer.z = 0; // 높이(z축)는 고려하지 않음

        // 플레이어와의 거리 계산
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(new Vector3(_direction, 0, 0), directionToPlayer);

        if (CurrentPerceptionState == Define.PerceptionType.DETECTIONM)
        {
            if (distanceToPlayer > _perceptionDistance)
            {
                AggroGauge -= 20 * Time.deltaTime;
            }
        }
        else
        {
            if (distanceToPlayer > _perceptionDistance || angleToPlayer > _perceptionAngle / 2.0f) // 플레이어가 부채꼴의 반지름 내에 없을 때
            {
                AggroGauge -= 20 * Time.deltaTime;
            }
            else // 플레이어가 부채꼴의 반지름 내에 있을 때
            {
                // 게이지 증가
                if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[0].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[0].gaugeIncrementPerSecond * Time.deltaTime;
                }
                else if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[1].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[1].gaugeIncrementPerSecond * Time.deltaTime;
                }
                else if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[2].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[2].gaugeIncrementPerSecond * Time.deltaTime;
                }

            }
        }

        UpdatePerceptionGauge();
    }

    // 인식 상태 변화 업데이트 함수
    private void UpdatePerceptionState()
    {
        if (_aggroGauge == 0)
        {
            if (CurrentPerceptionState != Define.PerceptionType.PATROL)
            {
                CurrentPerceptionState = Define.PerceptionType.PATROL;
            }
        }
        else if (_aggroGauge == 100)
        {
            if (CurrentPerceptionState != Define.PerceptionType.DETECTIONM)
            {
                CurrentPerceptionState = Define.PerceptionType.DETECTIONM;
            }
        }
        else
        {
            if (CurrentPerceptionState != Define.PerceptionType.BOUNDARY)
            {
                CurrentPerceptionState = Define.PerceptionType.BOUNDARY;
            }
        }
    }

    // 공격 함수
    public void Attack()
    {
        bool isHit = Physics.CheckBox(_hitPoint.position, _colliderSize / 2, _hitPoint.rotation, _playerLayer);

        if (isHit)
        {
            print("공격");
            _player.GetComponent<Player>().TakeDamage(_stat.Damage);
        }

    }

    #region View

    public void UpdatePerceptionGauge()
    {
        _nomalMonsterView.UpdatePerceptionGaugeImage(_aggroGauge/_maxAggroGauge);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);

        if (SpawnPoint != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(SpawnPoint.x - _moveRange, transform.position.y, 0), new Vector3(SpawnPoint.x + _moveRange, transform.position.y, 0));
        }
    }

    private void OnDrawGizmosSelected()
    {
        int _segments = 10;

        for (int j = _perceptionRanges.Count - 1; j >= 0; j--)
        {
            Gizmos.color = _perceptionRanges[j].color;

            // 시작 각도와 끝 각도 설정
            float halfAngle = _perceptionAngle / 2.0f;
            float startAngle = -halfAngle;
            float endAngle = halfAngle;

            // 중심점에서부터 시작
            Vector3 startPosition = transform.position;

            // 각도 간격 계산
            float angleStep = _perceptionAngle / _segments;

            float length = _perceptionDistance * ((float)_perceptionRanges[j].range_Ratio / 100.0f);

            // 부채꼴을 그리기 위해 시작점 계산
            Vector3 firstPoint = startPosition + Quaternion.Euler(0, 0, startAngle) * new Vector3(_direction, 0, 0) * length;
            Vector3 lastPoint = startPosition + Quaternion.Euler(0, 0, endAngle) * new Vector3(_direction, 0, 0) * length;

            Gizmos.DrawLine(startPosition, firstPoint);

            // 각도를 돌면서 부채꼴을 그리기
            for (int i = 1; i <= _segments; i++)
            {
                float currentAngle = startAngle + i * angleStep;
                Vector3 nextPoint = startPosition + Quaternion.Euler(0, 0, currentAngle) * new Vector3(_direction, 0, 0) * length;

                // 선을 그리기

                Gizmos.DrawLine(firstPoint, nextPoint);

                // 다음 점을 현재 점으로 갱신
                firstPoint = nextPoint;
            }

            // 마지막 선을 중심으로 그리기
            Gizmos.DrawLine(startPosition, lastPoint);
        }


    }

}
