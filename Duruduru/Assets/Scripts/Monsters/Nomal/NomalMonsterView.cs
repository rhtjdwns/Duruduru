using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface INomalMonsterView
{
    public void UpdatePerceptionGaugeImage(float value);
 
}

public class NomalMonsterView : MonsterView, INomalMonsterView
{
    [SerializeField] private Canvas _nomalMonsterCanvas;
    [SerializeField] private Image _perceptionGauge;

    public void Awake()
    {
        _nomalMonsterCanvas.worldCamera = Camera.main;
    }

    public void UpdatePerceptionGaugeImage(float value)
    {
        _perceptionGauge.fillAmount = value;
    }
}
