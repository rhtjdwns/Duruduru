using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser", menuName = "ScriptableObjects/EliteMonster/Skill/Laser", order = 1)]
public class Elite_Laser : Elite_Skill
{
    private float _coolTime;

    [SerializeField] private float _laserLength = 50.0f; // 레이저의 최대 길이
    [SerializeField] private float _laserWidth; // 레이저의 시작 지점
    //private float _laserAngle; // 레이저의 각도 (0도는 오른쪽)
    private GameObject _laser;
    public float LaserLength { get => _laserLength; }

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override void Check()
    {
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
        Debug.Log("레이저");

        Vector2 direction = _monster.Player.position - _monster.transform.position;
        _monster.Direction = direction.x;

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Laser"))
        {
            _monster.Ani.SetBool("Laser", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Laser", false);
        ObjectPool.Instance.Remove(_laser);
        _coolTime = 0;
        IsCompleted = false;
    }


    private void Attack()
    {
        _laser = ObjectPool.Instance.Spawn("Laser");
        _laser.transform.position = _monster.HitPoint.position;

        Vector3 tempVec = _laser.transform.localScale;
        tempVec.x *= _monster.Direction;
        _laser.transform.localScale = tempVec;

        _laser.GetComponent<Laser>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);
        CoroutineRunner.Instance.StartCoroutine(SetActiveCollider(true, 1.5f));
        CoroutineRunner.Instance.StartCoroutine(SetActiveCollider(false, 3f));
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }

    private IEnumerator SetActiveCollider(bool value, float t)
    {
        yield return new WaitForSeconds(t);
        _laser.GetComponent<CapsuleCollider>().enabled = value;
    }

}
