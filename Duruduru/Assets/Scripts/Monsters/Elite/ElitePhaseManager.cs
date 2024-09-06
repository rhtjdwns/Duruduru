using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElitePhaseManager : MonoBehaviour
{
    [SerializeField] private EliteMonster _phase1Monster;
    [SerializeField] private EliteMonster _phase2Monster;

    [SerializeField] private Define.ElitePhaseState _currentPhaseState = Define.ElitePhaseState.NONE;
    private Dictionary<Define.ElitePhaseState, Elite_PhaseState> _phaseStateStorage = new Dictionary<Define.ElitePhaseState, Elite_PhaseState>();

    private List<float> _targetHealthList = new List<float>();
    private int _targetHealthIndex = 0;

    public EliteMonster Phase1Monster { get => _phase1Monster; }
    public EliteMonster Phase2Monster { get => _phase2Monster; }
    public List<float> TargetHealthList { get => _targetHealthList; }
    public int TargetHealthIndex { get => _targetHealthIndex; set => _targetHealthIndex = value; }

  


    private void Awake()
    {
        _phaseStateStorage.Add(Define.ElitePhaseState.START,new Elite_PhaseStart(this));
        _phaseStateStorage.Add(Define.ElitePhaseState.PHASE1,new Elite_Phase1(this));
        _phaseStateStorage.Add(Define.ElitePhaseState.PHASECHANGE, new Elite_PhaseChange(this));
        _phaseStateStorage.Add(Define.ElitePhaseState.PHASE2, new Elite_Phase2(this));
        _phaseStateStorage.Add(Define.ElitePhaseState.FINISH, new Elite_PhaseFinish(this));
    }

    private void Start()
    {
        _targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.5f);
        _targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.3f);
        _targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.1f);
        _targetHealthList.Add(0);

        _phase2Monster.gameObject.SetActive(false);

        ChangeStageState(Define.ElitePhaseState.PHASE1);
    }

    void Update()
    {
        if (_currentPhaseState != Define.ElitePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Stay();
        }
    }

    public void ChangeStageState(Define.ElitePhaseState state)
    {
        if (_currentPhaseState != Define.ElitePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Exit();
        }
        _currentPhaseState = state;
        _phaseStateStorage[_currentPhaseState]?.Enter();
    }
}
