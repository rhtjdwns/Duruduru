using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/Stat/Monster Stat")]
public class MonsterStat : Stat
{

    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDelay;
        
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

    public float AttackRange { get => _attackRange; }
    public float AttackDelay { get => _attackDelay; }


    public override void Init()
    {
        _hp = _maxHp;
    }



}
