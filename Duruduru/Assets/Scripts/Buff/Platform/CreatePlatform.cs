using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePlatform : MonoBehaviour
{
    [System.Serializable]
    private class buffInfoList
    {     
        public List<Define.BuffInfo> infoList = new List<Define.BuffInfo>(); // 버프 정보 리스트
    }

    #region 변수
    // 플랫폼 정보 리스트
    [SerializeField] private List<buffInfoList> _platformList = new List<buffInfoList>();
    private List<BuffPlatform> _curPlatformList = new List<BuffPlatform>();

    // 프리팹
    [SerializeField] private GameObject _platformPrefab;

    // 오토 모드(자동 교체)
    [SerializeField] private bool _isAutoMode = true;

    // 플랫폼이 바뀌는 시간
    [SerializeField] private float _changeTime; // 바뀌는 시간
    private float _timer;

    private int _index = 0;
    #endregion

    #region 프로퍼티 
    public List<BuffPlatform> CurPlatformList { get => _curPlatformList; }
    public bool IsAutoMode { get => _isAutoMode; set => _isAutoMode = value; }
    public int Index
    {
        get => _index;
        set
        {
            _index = value;
            _index = _index % _platformList.Count;
        }
    }
    #endregion;

    private void Awake()
    {
        // 생성한 플랫폼이 존재하면 데이터 추가
        if (_curPlatformList.Count <= 0) 
        {
            int i = 0;

            foreach (Transform t in transform)
            {
                t.GetComponent<BuffPlatform>().Change(_platformList[0].infoList[i]);
                _curPlatformList.Add(t.GetComponent<BuffPlatform>());

                i++;
            }
        }
    }

    void Update()
    {
        if (_isAutoMode) // 오토 모드일 때
        {
            if (_changeTime == 0 || _platformList.Count < 2) return;

            if (_timer >= _changeTime)
            {
                ChangeToNext();
                _timer = 0;
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        else // 오토 모드가 아닐 때
        {

        }
       
    }

#if UNITY_EDITOR
    // 초기 플랫폼 생성 함수(에디터 상에서 사용)
    public void Create()
    {
        if (_platformList.Count == 0) return;
        if (_platformList[_index].infoList.Count == 0) return;

        Delete(); // 생성 전 삭제

        float width = _platformPrefab.GetComponent<Renderer>().bounds.size.x;
        int half = _platformList[_index].infoList.Count / 2;
        float posX = transform.position.x - (width * half);

        if (_platformList[_index].infoList.Count % 2 == 0) // 생성 개수가 짝수일 경우
        {
            posX += width / 2;
        }

        for (int i = 0; i< _platformList[_index].infoList.Count; i++) // 플랫폼 생성
        {
            GameObject temp = Instantiate(_platformPrefab, new Vector3(posX, transform.position.y, transform.position.z), Quaternion.identity, transform);
            temp.GetComponent<BuffPlatform>().ChangeColor(_platformList[_index].infoList[i]);
            posX += width;
        }
    }

    // 생성된 플랫폼 삭제 함수
    public void Delete()
    {
        for (int i = transform.childCount-1; i>=0;i--)
        {
            Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
        }
    }
#endif

    // 다음 플랫폼 정보로 교체 함수
    private void ChangeToNext() 
    {
        Index++;

        for (int i = 0; i < _curPlatformList.Count; i++)
        {
            _curPlatformList[i].Change(_platformList[_index].infoList[i]);
        }       
    }

    // 플랫폼 교체 함수
    public void Change(int index, Define.BuffInfo info) 
    {
        if (_curPlatformList.Count <= 1) return;

        _curPlatformList[index].Change(info);
    }

    // 생성할 플랫폼 크기 보여주는 용도
    private void OnDrawGizmos()
    {
        if (_platformList.Count == 0) return;

        Vector3 size = _platformPrefab.GetComponent<Renderer>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x * _platformList[0].infoList.Count, size.y, size.z));
    }
}
