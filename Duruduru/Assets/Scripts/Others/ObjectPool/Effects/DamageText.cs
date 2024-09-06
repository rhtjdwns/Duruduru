using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    private Vector3 _startPos;

    public float DamageValue { get; set; }
    [SerializeField] private TextMeshProUGUI _damageText;

    private void OnEnable()
    {
        _startPos = transform.localPosition;

        transform.DOLocalMoveY(transform.localPosition.y + 1, 0.5f);
    }

    private void Update()
    {
        _damageText.text = DamageValue.ToString();
    }

    private void OnDisable()
    {
        _damageText.text = "";
        transform.localPosition = _startPos;
    }


}
