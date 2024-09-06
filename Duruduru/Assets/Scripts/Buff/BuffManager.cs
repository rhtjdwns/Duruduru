using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : Singleton<BuffManager>
{

    [SerializeField] private List<BuffData> _buffStorage = new List<BuffData>(); // 버프 저장소

    private List<BuffData> _currentBuffList = new List<BuffData>();              // 현재 버프 리스트
    private List<BuffData> _removeBuffList = new List<BuffData>();               // 제거 버프 리스트


    private void Update()
    {

        foreach (BuffData data in _currentBuffList)
        {
            data.Stay();
        }
         
        // 끝난 버프 제거
        if (_removeBuffList.Count <= 0) return;

        foreach (BuffData data in _removeBuffList)
        {
            _currentBuffList.Remove(data);
        }

        _removeBuffList.Clear();
    }

    // 버프 추가
    public void AddBuff(Define.BuffInfo buff) 
    {

        if (GetCurretBuff(buff) != null) // 현재 같은 버프가 있는지 확인
        {
            RemoveBuff(buff); // 같은 버프 제거
        }

        BuffData buffData = GetBuff(buff);

        if (buffData == null) return;
        
        buffData.Enter();
        _currentBuffList.Add(buffData);
    }

    // 버프 제거
    public void RemoveBuff(Define.BuffInfo buff) // 버프 추가
    {
        BuffData buffData = GetCurretBuff(buff);

        if (buffData == null) return;

        buffData.Exit();
        _removeBuffList.Add(buffData);
    }

    // 버프 저장소에서 찾기
    public BuffData GetBuff(Define.BuffInfo info) 
    {
        foreach (BuffData buff in _buffStorage)
        {
            if (buff.info == info)
            {
                return buff;
            }
        }

        return null;
    }

    // 현재 버프 리스트에서 찾기
    private BuffData GetCurretBuff(Define.BuffInfo info)
    {
        foreach (BuffData buff in _currentBuffList)
        {
            if (buff.info == info)
            {
                return buff;
            }
        }

        return null;
    }
}