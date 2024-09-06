using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Boundary : Nomal_State
{
    public Nomal_Boundary(NomalMonster monster) : base(monster)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("°æ°è");
    }
    public override void Stay()
    {

    }
    public override void Exit()
    {
        base.Exit();
    }
}
