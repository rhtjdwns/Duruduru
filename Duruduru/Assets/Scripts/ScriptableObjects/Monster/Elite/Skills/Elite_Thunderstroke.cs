using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunderstroke", menuName = "ScriptableObjects/EliteMonster/Skill/Thunderstroke", order = 1)]
public class Elite_Thunderstroke : Elite_Skill
{

    private float _totalTime;

    [SerializeField] private float _startDelay;
    private float _startTime;

    [SerializeField] private float _executeDuration;
    private float _executeTime;

    [SerializeField] private float _lightningCount;

    private List<BuffPlatform> tempPlatforms;
    public override void Init(EliteMonster monster)
    {
        base.Init(monster);
        _totalTime = 0;
        _startTime = 0;
        _executeTime = _executeDuration;
       
    }

    public override void Check()
    {
        if (IsCompleted) return;
    }

    public override void Enter()
    {
        Debug.Log("³«·Ú");

        _monster.GetComponent<CapsuleCollider>().enabled = false;
        _monster.Rb.useGravity = false;
    }
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Thunder"))
        {
            _monster.Ani.SetBool("Thunder", true);
        }

        // ½ÃÀÛ µô·¹ÀÌ
        if (_startTime < _startDelay)
        {
            _startTime += Time.deltaTime;

            return;
        }

        // ½ÇÇà ½Ã°£
        if (_totalTime >= _info.totalTime)
        {
            _monster.GroggyTime = 5;
            _monster.FinishSkill(Define.EliteMonsterState.GROGGY);
        }
        else
        {
            // »ç¿ë °£°Ý
            if (_executeTime >= _executeDuration)
            {

                tempPlatforms = _monster.CreatePlatform.CurPlatformList.ToList();

                for (int i = 0; i < _lightningCount; i++)
                {
                    int randomIndex = Random.Range(0, tempPlatforms.Count);
                    Vector3 executePosition = tempPlatforms[randomIndex].transform.position;
                    tempPlatforms.RemoveAt(randomIndex);

                    CoroutineRunner.Instance.StartCoroutine(ExecuteLightning(executePosition));
                }


                _executeTime = 0;
            }
            else
            {
                _executeTime += Time.deltaTime;
            }

            _totalTime += Time.deltaTime;
        }

      
    }
    public override void Exit()
    {
        _monster.Ani.SetBool("Thunder", false);
        _totalTime = 0;
        _startTime = 0;
        _executeTime = _executeDuration;

        _monster.GetComponent<CapsuleCollider>().enabled = true;
        _monster.Rb.useGravity = true;

        IsCompleted = false;
    }

    // ³«·Ú »ý¼º ÇÔ¼ö
    private IEnumerator ExecuteLightning(Vector3 point)
    {
        GameObject lightningReady = ObjectPool.Instance.Spawn("FX_LightningReady", 1f);
        lightningReady.transform.position = point;

        yield return new WaitForSeconds(0.5f);

        GameObject lightning = ObjectPool.Instance.Spawn("Lightning", 2f);
        lightning.transform.position = point + new Vector3(0, 0.2f, 0);
        lightning.GetComponent<Lightning>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);

        lightning.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(0.5f);
        lightning.GetComponent<Collider>().enabled = false;

    }
}