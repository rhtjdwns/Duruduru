using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TickDamage", menuName = "ScriptableObjects/Buff Data/Tick Damage", order = int.MaxValue)]
public class TickDamageBuff : BuffData
{
    [SerializeField]private float _tickDamageDelay; // ∆Ω µ•πÃ¡ˆ µÙ∑π¿Ã
    private float _timer = 0;

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

    }

    public override void Stay()
    {
        if (_timer >= _tickDamageDelay)
        {
            _player.TakeDamage(value);
            _timer = 0;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {

    }

}
