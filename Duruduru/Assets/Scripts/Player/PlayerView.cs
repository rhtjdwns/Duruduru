using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private Slider _upgradeCountSlider;

    public void UpdateHpBar(float value)
    {
        _hpBarImage.fillAmount = value;
    }
    public void UpdateStaminaBar(float value)
    {
        _staminaBarImage.fillAmount = value;
    }
    public void UpdateUpgradeCountSlider(int value)
    {
        _upgradeCountSlider.value = value;
    }
}
