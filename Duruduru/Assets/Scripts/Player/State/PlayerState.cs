using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player _player;

    public PlayerState(Player player)
    {
        _player = player;
    }

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
