using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Launch", menuName = "ScriptableObjects/EliteMonster/Skill/Launch", order = 1)]
public class Elilte_Launch : Elite_Skill
{
    private float _coolTime;

    [SerializeField] private float _startDelay;
    private float _startTime;

    [SerializeField] private float _energyBallSpeed;
    private GameObject _energyBall;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _startTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {

                IsCompleted = true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

    }

    public override void Enter()
    {
        Debug.Log("투사체");
        _monster.Direction = _monster.Player.transform.position.x - _monster.transform.position.x; // 플레이어 바라보기

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;

        
    }
    public override void Stay()
    {
        if (_startTime >= _startDelay)
        {
            if (_monster.Ani.GetBool("Launch")) return;

            _monster.Ani.SetBool("Launch", true);
        }
        else
        {
            _startTime += Time.deltaTime;
        }

    }
    public override void Exit()
    {
        _monster.Ani.SetBool("Launch", false);

        if (_energyBall.activeSelf)
        {
            GameObject explosion = ObjectPool.Instance.Spawn("ElectricBallExplosion");
            explosion.transform.position = _energyBall.transform.position;
        }
        
        _startTime = 0;
        _coolTime = 0;

        IsCompleted = false;
    }

    private IEnumerator ExcuteLaunch()
    {
        _energyBall = ObjectPool.Instance.Spawn("ElectricBall",5);
        _energyBall.transform.SetParent(_monster.HitPoint);
        _energyBall.transform.localPosition = Vector3.zero;
        _energyBall.GetComponent<ElectricBall>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);
        _energyBall.GetComponent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(0.3f);
        _energyBall.transform.SetParent(null);
        _energyBall.GetComponent<ElectricBall>().Speed = _energyBallSpeed;
        _energyBall.GetComponent<ElectricBall>().Direction = _monster.Direction;
        _energyBall.GetComponent<SphereCollider>().enabled = true;
    }

    private void Attack()
    {
        CoroutineRunner.Instance.StartCoroutine(ExcuteLaunch());
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
