using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera targetCamera; // �ٶ� ī�޶�

    private void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        // ī�޶� �������� ���� ���, ���� ī�޶� �ٶ󺸵��� ����
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // ������Ʈ�� ī�޶� �ٶ󺸵��� ����
        transform.LookAt(targetCamera.transform);
    }
}
