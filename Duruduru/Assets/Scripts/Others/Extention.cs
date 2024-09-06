using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extention
{

    //월드 좌표를 캔버스 좌표로 바꾸는 확장 함수
    public static Vector2 WorldToCanvasPosition(this RectTransform rectTransform, string canvasName, Vector3 worldPosition, bool worldCamera = false)
    {
        Canvas canvas = GameObject.Find(canvasName).GetComponent<Canvas>();

        // 1. 월드 좌표를 화면 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        // 2. 화면 좌표를 캔버스 좌표로 변환
        Vector2 localPoint;
        if (worldCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, canvas.worldCamera, out localPoint);
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localPoint);
        }

        return localPoint; // 캔버스 상의 좌표 반환
    }
}
