using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FAttack", menuName = "ScriptableObjects/EliteMonster/Skill/FAttack", order = 1)]
public class Elite_FAttack : Elite_Skill
{
    private float _coolTime;

    private TempoCircle _tempoCircle;
    [SerializeField] private float _parringTime; // 패링 시간

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);


        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {
                IsCompleted =  true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

    }

    public override void Enter()
    {
        //_monster.Ani.SetBool("FAttack", true);

        Debug.Log("일반 공격1");
        CreateTempoCircle();

        _monster.OnAttackAction += Hit;
        _monster.OnFinishSkill += Finish;
    }
    public override void Stay()
    {

        if (!_monster.Ani.GetBool("FAttack"))
        {
            _monster.Ani.SetBool("FAttack", true);
        }


        if (_tempoCircle == null) return;

        if (_tempoCircle.CircleState == Define.CircleState.NONE)
        {
            if (CheckParringBox() && _monster.Player.GetComponent<Player>().Attack.PointTempoCircle == null)
            {
                _tempoCircle.IsAvailable = true;
            }
            else
            {
                _tempoCircle.IsAvailable = false;
            }
        }
        else
        {
            if (Parring())
            {
                _monster.Ani.SetBool("FAttack", false);

                _monster.FailTime = 2;
                _monster.FinishSkill(Define.EliteMonsterState.FAIL);
            }
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("FAttack", false);

        _tempoCircle = null;
        _coolTime = 0;

        IsCompleted = false;
    }

    private void CreateTempoCircle()
    {
        if (_tempoCircle != null) return;

        //SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Ready", _player.transform);

        GameObject tempoCircle = ObjectPool.Instance.Spawn("TempoCircle", 0, _monster.transform);

        Vector3 spawnPoint = _monster.transform.position + new Vector3(0, 1, -0.1f);
        tempoCircle.transform.position = spawnPoint;

        _tempoCircle = tempoCircle.GetComponent<TempoCircle>();
        _tempoCircle.Init(_monster.Player);           // 템포 원 초기화

        _tempoCircle.ShrinkDuration = _parringTime;        // 탬포 원 시간 값 추가

        _tempoCircle.SetTempoCircleAction(() => { _monster.Player.GetComponent<Player>().Ani.SetTrigger("Parring"); });
    }

    private bool CheckParringBox()
    {
        return Physics.CheckBox(_monster.ParringPoint.position, _monster.ParringColliderSize / 2, _monster.ParringPoint.rotation, _monster.PlayerLayer);
    }

    private bool Parring()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT) // 패링 성공 확인
        {
            Debug.Log("패링");
            return true;
        }

        return false;
    }

    // 공격 함수
    private void Hit()
    {
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            Debug.Log("일반 공격1 성공");
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);

            // 히트 파티클 생성
            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_EliteAttack", 1); ;

            Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }
     
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }

}