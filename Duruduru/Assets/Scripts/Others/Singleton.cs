using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<T>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    obj = new GameObject(typeof(T).Name + "_Singleton").AddComponent<T>();
                    _instance = obj;
                }

            }


            return _instance;
        }
    }

    /*  private void Awake()
      {
          var objs = FindObjectsOfType<T>();
          if (objs.Length != 1)
          {
              Destroy(gameObject);
              print("ΩÃ±€≈Ê ªË¡¶");
              return;
          }

          DontDestroyOnLoad(gameObject);
      }*/
}
