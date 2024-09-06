using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Nomal_State
{
    protected NomalMonster _monster;
    protected bool _isEntered;
    public bool IsEntered { get => _isEntered; }
    public Nomal_State(NomalMonster monster)
    {
        _monster = monster;
    }

    public virtual void Enter()
    {
        _isEntered = true;
    }
    public abstract void Stay();
    public virtual void Exit()
    {
        _isEntered = false;
    }
}
