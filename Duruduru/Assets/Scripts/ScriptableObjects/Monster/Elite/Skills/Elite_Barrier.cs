using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Barrier", menuName = "ScriptableObjects/EliteMonster/Skill/Barrier", order = 1)]
public class Elite_Barrier : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    [SerializeField] private float _defense;   // 데미지 감소량

    private float _lastHp;

    private GameObject _guardEffect;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _totalTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {

        Debug.Log("가드");
        _lastHp = _monster.Stat.Hp;
        _monster.Stat.Defense = _defense;

        // 가드 이펙트 생성
        _guardEffect = ObjectPool.Instance.Spawn("FX_EliteGuard");
        _guardEffect.transform.position = _monster.transform.position;

        _monster.GetComponent<SphereCollider>().enabled = true;

        _monster.IsGuarded = true;
    }
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Guard"))
        {
            _monster.Ani.SetBool("Guard", true);
        }

        if (_totalTime >= _info.totalTime)
        {
            if (_lastHp > _monster.Stat.Hp) // 플레이어가 가드 상태인 몬스터 공격 시(체력 변화가 있을 때)
            {
                _monster.ChangeCurrentSkill(Define.EliteMonsterSkill.SUPERPUNCH); // 멀리 치기 실행
                IsCompleted = false;
            }
            else
            {
                _monster.FinishSkill();
            }
            Debug.Log("가드 끝");
        }
        else
        {
            _totalTime += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _monster.Ani.SetBool("Guard", false);

        _monster.GetComponent<SphereCollider>().enabled = false;

        // 가드 이펙트 제거
        ObjectPool.Instance.Remove(_guardEffect);
      

        _monster.Stat.Defense = 0;
        _totalTime = 0;
        _coolTime = 0;

        _monster.IsGuarded = false;
        IsCompleted = false;
    }

}
