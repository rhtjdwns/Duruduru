using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonsterView : MonoBehaviour
{
    [SerializeField] protected Image _hpBarImage;

    public void UpdateHpBar(float value)
    {
        _hpBarImage.fillAmount = value;
    }

}
