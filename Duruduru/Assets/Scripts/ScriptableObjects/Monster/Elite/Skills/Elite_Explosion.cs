using System.Collections;
using UnityEngine;
/// <summary>
/// 구현 X
/// </summary>
[CreateAssetMenu(fileName = "Explosion", menuName = "ScriptableObjects/EliteMonster/Skill/Explosion", order = 1)]
public class Elite_Explosion : Elite_Skill
{
   [SerializeField] private float _executeDuration; // 실행 시간
    private float _executeTime;

    [SerializeField] private float _executeMaxCount; // 실행 횟수
    private float _executeCount;

    private int _executeIndex;
    private bool _toRight;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _executeTime = _executeDuration;
        _executeCount = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;
    }


    public override void Enter()
    {
        Debug.Log("대지 폭발");

        int randomIndex = Random.Range(0, 2);
        _executeIndex = randomIndex == 0 ? 0 : _monster.CreatePlatform.CurPlatformList.Count - 1; // 시작 인덱스(이동 위치) 선택하기 (처음/끝)
        _monster.transform.position = _monster.CreatePlatform.CurPlatformList[_executeIndex].transform.position;

        _toRight = _executeIndex == 0 ? true : false;
        _monster.Direction = _executeIndex == 0 ? 1 : -1;
        _executeIndex = _executeIndex == 0 ? 1 : _monster.CreatePlatform.CurPlatformList.Count - 2;


    }
    public override void Stay()
    {
        if (_executeTime >= _executeDuration)
        {
            if (_executeCount >= _executeMaxCount)
            {
                _monster.FinishSkill();
                return;
            }

            CoroutineRunner.Instance.StartCoroutine(ExecuteExplosion());

            _executeTime = 0;
            _executeCount++;         
        }
        else
        {
            _executeTime += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _executeTime = _executeDuration;
        _executeCount = 0;
    }

    // 공격 실행 코루틴
    private IEnumerator ExecuteExplosion()
    {
        int index = _executeIndex;

        while (index >= 0 && index <= _monster.CreatePlatform.CurPlatformList.Count - 1)
        {       
            Vector3 executePosition = _monster.CreatePlatform.CurPlatformList[index].transform.position;

            GameObject explosion = ObjectPool.Instance.Spawn("Explosion", 1);
            explosion.transform.position = executePosition;

            explosion.GetComponent<Explosion>().totalDamage = _monster.Stat.Damage * (_info.damage / 100);

            if (_toRight)
            {
                index++;
            }
            else
            {
                index--;
            }

            yield return new WaitForSeconds(1f);
        }
        
    }
}