using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffData : ScriptableObject
{
    protected Player _player;

    public Define.BuffType type;
    public Define.BuffInfo info;

    [Space]
    public float value;

    [Space]
    public Color color;

    public BuffPlatform Platform { get; set; }

    public bool IsFinished { get; set; } = false;

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
