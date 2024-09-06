using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TempoCircle : MonoBehaviour
{
    private Transform _player;

    [SerializeField] private GameObject _checkCircle; // �� ��������Ʈ �̹���
    [SerializeField] private GameObject _perfectCircle; // �� ��������Ʈ �̹���
    [SerializeField] private GameObject _goodCircle; // �� ��������Ʈ �̹���
    [SerializeField] private float _shrinkDuration = 1f; // ���� �پ��µ� �ɸ��� �ð� (��)
    

    [SerializeField] private Vector2 _perfectTime; // �Ϻ��� Ÿ�̹� (��)
    [SerializeField] private Vector2 _goodTime; // ���� Ÿ�̹� (��)
    private Vector2 _perfectScale; // �Ϻ��� Ÿ�̹� (��)
    private Vector2 _goodScale; // ���� Ÿ�̹� (��)

    [Space]
    [SerializeField] private GameObject _perfectPrefab;
    [SerializeField] private GameObject _goodPrefab;
    [SerializeField] private GameObject _badPrefab;
    [SerializeField] private GameObject _missPrefab;


    private float timer = 0f;
    [SerializeField] private bool isShrinking = true; //��� �� Ȯ��

    private Define.CircleState _circleState = Define.CircleState.NONE;

    public Action OnSuccess;
    public Action OnFailure;
    public Action OnFinish;

    public float ShrinkDuration { get => _shrinkDuration; set => _shrinkDuration = value; }
    public Define.CircleState CircleState { get => _circleState; }

    public bool IsAvailable { get; set; } = true;

    void Update()
    {
        if (timer >= _shrinkDuration)
        {
            if (isShrinking)
            {
                //Debug.Log("Miss!");
                _circleState = Define.CircleState.MISS;
                OnFailure?.Invoke();

                isShrinking = false;
                _checkCircle.SetActive(false);

                SpawnFx(_circleState);
            }

            Finish();
        }
        else
        {
            timer += Time.deltaTime;

            if (isShrinking)
            {
                float scale = Mathf.Lerp(1.0f, 0, timer / _shrinkDuration);
                _checkCircle.transform.localScale = new Vector3(scale, scale, 1.0f);
                Player player = _player.GetComponent<Player>();

                if (player.CurrentState != Define.PlayerState.STUN && player.Attack.CurrentAttackkState != Define.AttackState.ATTACK && IsAvailable)
                {

                    if (_circleState == Define.CircleState.NONE)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            CheckTiming();

                            isShrinking = false;
                        }
                    }
                }
            }
        }

    }

    // �ʱ�ȭ �Լ�
    public void Init(Transform player = null)
    {
        timer = 0.0f;
        _shrinkDuration = 1;

        _checkCircle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _checkCircle.SetActive(true);

        _perfectScale.x = Mathf.Lerp(0, 1, _perfectTime.x / _shrinkDuration);
        _perfectScale.y = Mathf.Lerp(0, 1, _perfectTime.y / _shrinkDuration);
        _perfectCircle.transform.localScale = new Vector3(_perfectScale.x + 0.02f, _perfectScale.x + 0.02f, _perfectScale.x); ;

        _goodScale.x = Mathf.Lerp(0, 1, _goodTime.x / _shrinkDuration);
        _goodScale.y = Mathf.Lerp(0, 1, _goodTime.y / _shrinkDuration);
        _goodCircle.transform.localScale = new Vector3(_goodScale.x + 0.02f, _goodScale.x + 0.02f, _goodScale.x); 

        isShrinking = true;
        _circleState = Define.CircleState.NONE;

        _player = player;

        IsAvailable = true;
    }

    // Ÿ�̹� Ȯ�� �Լ�
    private void CheckTiming()
    {
        if (_perfectScale.x <= _checkCircle.transform.localScale.x && _checkCircle.transform.localScale.x < _perfectScale.y)
        {
            _circleState = Define.CircleState.PERFECT;
            OnSuccess?.Invoke();
        }
        else if (_goodScale.x <= _checkCircle.transform.localScale.x && _checkCircle.transform.localScale.x < _goodScale.y)
        {
            _circleState = Define.CircleState.GOOD;
            OnSuccess?.Invoke();
        }
        else if(_goodTime.y < _checkCircle.transform.localScale.x || _perfectScale.x > _checkCircle.transform.localScale.x)
        {
            _circleState = Define.CircleState.BAD;
            OnFailure?.Invoke();
        }

        SpawnFx(_circleState);
       
    }

    // ����Ʈ ���� �Լ�
    private void SpawnFx(Define.CircleState state)
    {      
        GameObject temp = null;

        switch (state)
        {
            case Define.CircleState.PERFECT:
                temp = Instantiate(_perfectPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);              
                break;
            case Define.CircleState.GOOD:
                temp = Instantiate(_goodPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
            case Define.CircleState.BAD:
                temp = Instantiate(_badPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
            case Define.CircleState.MISS:
                temp = Instantiate(_missPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
        }
      
        Destroy(temp, 1f);

    }

    private void Finish()
    {      
        ObjectPool.Instance.Remove(gameObject);
        OnFinish?.Invoke();
        
        OnSuccess = null;
        OnFailure = null;
        OnFinish = null;
    }

    public void SetTempoCircleAction(Action success = null, Action Fail = null, Action Finish = null)
    {
        // ���� �̺�Ʈ �߰�
        OnSuccess += success;
        OnFailure += Fail;
        OnFinish += Finish;
    }
}
