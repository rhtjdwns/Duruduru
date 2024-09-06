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

    #region ����
    private INomalMonsterView _nomalMonsterView;

    [SerializeField] private float _moveRange; // Idle �̵� �Ÿ�

    [Space]
    [SerializeField] private float _perceptionDistance;                                                                                    // �ν� �Ÿ�
    [SerializeField] private float _perceptionAngle;                                                                                       // �ν� ����
    [SerializeField] private List<PerceptionRange> _perceptionRanges = new List<PerceptionRange>();                                        // �ν� ����
    private Dictionary<Define.PerceptionType, Nomal_State> _perceptionStateStorage = new Dictionary<Define.PerceptionType, Nomal_State>(); // �ν� ���� �����
    private Define.PerceptionType _currentPerceptionState;                                                                                 // ���� �λ� ���� 
    
    [SerializeField] private float _maxAggroGauge; // �ִ� ��׷� ������
    private float _aggroGauge = 0;                 // ���� ��׷� ������

    public Vector2 SpawnPoint { get; set; }

    [Space]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Vector3 _colliderSize;


    #endregion

    #region ������Ƽ
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

        // �˹� �� �����ϴ� �̺�Ʈ
        OnKnockback += () =>
        {
            float dir = _player.position.x - transform.position.x;
            Direction = dir;
        };

        
    }

    private void Update()
    {

        CheckPerceptionState(); // ������ ���� ��Ű�� �Լ�
        UpdatePerceptionState(); // ������ Ȯ�� �� �ν� ���� ������Ʈ

        // �ν� ���� �ȿ� ������ ��
        if (_perceptionStateStorage[_currentPerceptionState].IsEntered)
        {
            _perceptionStateStorage[_currentPerceptionState]?.Stay();
        }

    }


    // ��ä�� �ȿ� �÷��̾ �ִ��� Ȯ��
    private void CheckPerceptionState() 
    {
        // ���������� �÷��̾�� ���ϴ� ���� ���

        Vector3 playerPos = new Vector3(_player.position.x, _player.position.y, _player.position.z);
        Vector3 directionToPlayer = playerPos - transform.position;
        directionToPlayer.z = 0; // ����(z��)�� ������� ����

        // �÷��̾���� �Ÿ� ���
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
            if (distanceToPlayer > _perceptionDistance || angleToPlayer > _perceptionAngle / 2.0f) // �÷��̾ ��ä���� ������ ���� ���� ��
            {
                AggroGauge -= 20 * Time.deltaTime;
            }
            else // �÷��̾ ��ä���� ������ ���� ���� ��
            {
                // ������ ����
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

    // �ν� ���� ��ȭ ������Ʈ �Լ�
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

    // ���� �Լ�
    public void Attack()
    {
        bool isHit = Physics.CheckBox(_hitPoint.position, _colliderSize / 2, _hitPoint.rotation, _playerLayer);

        if (isHit)
        {
            print("����");
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

            // ���� ������ �� ���� ����
            float halfAngle = _perceptionAngle / 2.0f;
            float startAngle = -halfAngle;
            float endAngle = halfAngle;

            // �߽����������� ����
            Vector3 startPosition = transform.position;

            // ���� ���� ���
            float angleStep = _perceptionAngle / _segments;

            float length = _perceptionDistance * ((float)_perceptionRanges[j].range_Ratio / 100.0f);

            // ��ä���� �׸��� ���� ������ ���
            Vector3 firstPoint = startPosition + Quaternion.Euler(0, 0, startAngle) * new Vector3(_direction, 0, 0) * length;
            Vector3 lastPoint = startPosition + Quaternion.Euler(0, 0, endAngle) * new Vector3(_direction, 0, 0) * length;

            Gizmos.DrawLine(startPosition, firstPoint);

            // ������ ���鼭 ��ä���� �׸���
            for (int i = 1; i <= _segments; i++)
            {
                float currentAngle = startAngle + i * angleStep;
                Vector3 nextPoint = startPosition + Quaternion.Euler(0, 0, currentAngle) * new Vector3(_direction, 0, 0) * length;

                // ���� �׸���

                Gizmos.DrawLine(firstPoint, nextPoint);

                // ���� ���� ���� ������ ����
                firstPoint = nextPoint;
            }

            // ������ ���� �߽����� �׸���
            Gizmos.DrawLine(startPosition, lastPoint);
        }


    }

}
