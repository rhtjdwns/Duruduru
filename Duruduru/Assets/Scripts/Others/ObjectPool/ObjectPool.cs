using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private List<GameObject> _prefabStorage = new List<GameObject>();  // 미리 생성할 객체의 프리팹
    [SerializeField] private int _initialSize = 10;  // 초기 생성할 객체의 수

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        foreach (GameObject prefab in _prefabStorage)
        {
            CreatePool(prefab);
        }
    }

    public void CreatePool(GameObject prefab)
    {
        if (!FindPrefab(prefab.name))
        {
            _prefabStorage.Add(prefab);
        }

        Queue<GameObject> queue = new Queue<GameObject>();

        GameObject poolObj = new GameObject(prefab.name + "Pool");
        poolObj.transform.SetParent(transform);

        for (int i = 0; i < _initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, poolObj.transform);

            obj.SetActive(false);
            queue.Enqueue(obj);
        }
        pool.Add(prefab.name, queue);

    }

    // 객체를 풀에서 가져오기
    public GameObject Spawn(string name, float t = 0, Transform parent = null)
    {
        if (!FindPool(name))
        {
            return null;
        }

        GameObject obj;

        if (pool[name].Count > 0)
        {
            obj = pool[name].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            // 풀에 객체가 없으면 새로 생성
            obj = Instantiate(GetPrefab(name), transform);
            Reset(obj);
        }

        if (parent)
        {
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
        }

        if (t != 0)
        {
            StartCoroutine(ReturnObj(obj, t));
        }

        return obj;
    }

    // 객체를 풀로 반환하기
    public void Remove(GameObject obj)
    {
        Reset(obj);
        obj.SetActive(false);
        pool[RemoveClone(obj.name)].Enqueue(obj);
    }

    private IEnumerator ReturnObj(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);

        if (obj.activeSelf)
        {
            Remove(obj);
        }

    }

    private void Reset(GameObject obj)
    {
        string poolName = RemoveClone(obj.name) + "Pool";

        obj.transform.SetParent(GetPool(poolName));
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }

    public GameObject GetPrefab(string name)
    {
        foreach (GameObject prefab in _prefabStorage)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }

        return null;
    }

    public bool FindPrefab(string name)
    {
        foreach (GameObject prefab in _prefabStorage)
        {
            if (prefab.name == name)
            {
                return true;
            }
        }

        return false;
    }

    public Transform GetPool(string name)
    {
        foreach (Transform pool in transform)
        {
            if (pool.name == name)
            {
                return pool;
            }
        }

        return transform;
    }

    public bool FindPool(string name)
    {
        foreach (Transform pool in transform)
        {
            if (pool.name == name + "Pool")
            {
                return true;
            }
        }

        return false;
    }


    private string RemoveClone(string name)
    {
        int index = name.IndexOf("(Clone)");
        if (index > 0)
            name = name.Substring(0, index);

        return name;
    }
}
