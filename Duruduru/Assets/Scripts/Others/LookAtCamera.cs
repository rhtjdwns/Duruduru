using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera targetCamera; // 바라볼 카메라

    private void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        // 카메라가 설정되지 않은 경우, 메인 카메라를 바라보도록 설정
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // 오브젝트가 카메라를 바라보도록 설정
        transform.LookAt(targetCamera.transform);
    }
}
