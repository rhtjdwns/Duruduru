using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : PlayerState
{
    public NoneState(Player player) : base(player)
    {
        _player = player;
    }

    public override void Enter()
    {

    }

    public override void Stay()
    {
    }

    public override void Exit()
    {

    }
}
