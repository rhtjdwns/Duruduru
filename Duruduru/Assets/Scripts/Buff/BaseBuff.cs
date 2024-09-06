using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseBuff : MonoBehaviour
{
   
    private Image _image;
    private TextMeshProUGUI _timerText;

    public void Awake()
    {
        _image = GetComponent<Image>();
        _timerText = transform.Find("BuffTimerText").GetComponent<TextMeshProUGUI>();
    }

}
