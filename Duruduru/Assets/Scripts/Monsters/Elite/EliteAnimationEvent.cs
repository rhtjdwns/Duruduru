using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EliteAnimationEvent : MonoBehaviour
{
    [SerializeField] private EliteMonster _monster;

    private void Attack()
    {
        _monster.OnAttackAction?.Invoke();
    }


    private void Finish()
    {
        _monster.OnFinishSkill?.Invoke();
    }
    private void MoveToX()
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (!Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.WallLayer) &&
            !Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.PlayerLayer))
        {

            transform.parent.DOMoveX(transform.position.x + _monster.CurrentSkill.Info.range * rayDirection.x, 0.1f);
        }

        /*  else if (Physics.Raycast(rayOrigin, rayDirection, out hit, value * rayDirection.x, _monster.PlayerLayer))
          {
              ///transform.parent.DOMoveX(hit.point.x, 0.1f);
          }*/



        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * _monster.CurrentSkill.Info.range, Color.green);
    }
    private void MoveToClosestPlayer(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.PlayerLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 1f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }


        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * _monster.CurrentSkill.Info.range, Color.red);
    }
}
