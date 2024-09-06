using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elite_State
{
    protected EliteMonster _monster;

    public Elite_State(EliteMonster monster)
    {
        _monster = monster;
    }

    public abstract void Enter();


    public abstract void Stay();

    public abstract void Exit();

}
