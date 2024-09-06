using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elite_Skill : ScriptableObject
{
    [SerializeField] protected Elite_SkillInfo _info;
    public Elite_SkillInfo Info { get => _info; }

    protected EliteMonster _monster;
    public bool IsCompleted { get; set; }



    public virtual void Init(EliteMonster monster)
    {
        _monster = monster;
        IsCompleted = false;
    }
    public abstract void Check();
    public abstract void Enter();

    public abstract void Stay();
    public abstract void Exit();

}
