using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player _player;

    #region Hit
    // 플레이어 타격 위치에 사용하는 이벤트 함수
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.ColliderSize / 2, _player.HitPoint.rotation, _player.MonsterLayer);

        if (hitMonsters.Length <= 0) return;

        _player.Attack.IsHit = true;

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

            if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
            {
                HitMainTempo(monster);
            }
            else
            {
                HitPointTempo(monster);
            }
            // 히트 파티클 생성
            GameObject hitParticle = SpawnHitParticle(monster);


            Vector3 hitPos = monsterCollider.ClosestPoint(_player.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }
    }
    private void HitMainTempo(Monster monster)
    {
        // 메인 템포일 때 데미지 처리
        monster.TakeDamage(_player.GetTotalDamage());
    }
    private void HitPointTempo(Monster monster)
    {
        Define.CircleState state = _player.Attack.PointTempoCircle.CircleState;
        float damage = (state == Define.CircleState.GOOD) ? _player.GetTotalDamage(false) : _player.GetTotalDamage();
        monster.TakeDamage(damage);
    }
    private GameObject SpawnHitParticle(Monster monster)
    {
        GameObject hitParticle = null;

        if (monster.IsGuarded)
        {
            float direction = transform.position.x - monster.transform.position.x;

            direction = direction > 0 ? 1 : -1;

            // 가드 충돌 이펙트 생성
            hitParticle = ObjectPool.Instance.Spawn("FX_EliteHitGuard", 1);
            hitParticle.transform.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            if (_player.Attack.CurrentTempoData.type == Define.TempoType.POINT)
            {
                hitParticle = ObjectPool.Instance.Spawn("P_point_attack", 1);
            }
            else
            {
                hitParticle = ObjectPool.Instance.Spawn("P_main_attack", 1);
            }
        }

        return hitParticle;
    }

    #endregion

    private void StartPointTempo()
    {
        _player.IsInvincible = true;
    }

    // 포인트 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void FinishPointTempo()
    {
        float addStamina = 0;

        if (_player.Attack.IsHit)
        {
            addStamina = _player.Attack.CurrentTempoData.maxStamina;

            if (_player.Attack.PointTempoCircle.CircleState == Define.CircleState.GOOD) // 타이밍이 Good일 경우
            {
                addStamina = _player.Attack.CurrentTempoData.minStamina;
            }

            _player.Attack.UpgradeCount++;

        }

        _player.Stat.Stamina += addStamina;
        _player.UpdateStamina();

        _player.Attack.IsHit = false;

        _player.Attack.PointTempoCircle = null;

        _player.IsInvincible = false;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // 메인 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void Finish(float delay)
    {
        float addStamina = _player.Attack.IsHit ? _player.Attack.CurrentTempoData.maxDamage : 0;

        _player.Stat.Stamina += addStamina;
        _player.UpdateStamina();

        _player.Attack.IsHit = false;
        _player.Attack.CheckDelay = delay;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);
    }

    // 공격 사거리 안에 적이 있으면 적 앞으로 이동하는 이벤트 함수
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }


        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * _player.Attack.CurrentTempoData.distance, Color.red);
    }

    // 시간 크기 변경
    private void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }
    // 시간 크기 복구
    private void ReturnTimeScale()
    {
        Time.timeScale = 1;
    }

    //타임라인 실행 함수
    private void StartTimeline(string name)
    {
        TimelineManager.Instance.PlayTimeline(name);
    }

    private void PlayerSfx(Define.PlayerSfxType type)
    {
        switch (type)
        {
            case Define.PlayerSfxType.MAIN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Hit_" + (_player.Attack.CurrentTempoData.attackNumber + 1), transform);
                break;
            case Define.PlayerSfxType.POINT:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Hit", transform);
                break;
            case Define.PlayerSfxType.DASH:
                break;
            case Define.PlayerSfxType.JUMP:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Jump", transform);
                break;
            case Define.PlayerSfxType.RUN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Running", transform);
                break;
            case Define.PlayerSfxType.STUN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Overload_Occurred", transform);
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Overload_Recovery", transform);
                break;
        }
    }
}
