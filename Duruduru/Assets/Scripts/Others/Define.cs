

public class Define
{

    public enum UIType
    {
        ALL, BUTTON, IMAGE, CANVASGROUP, TMPRO, TOGGLE, SLIDER
    }

    public enum KeyType
    {
        DOWN, HOLD, UP
    }

    public enum BuffType // ENTER : 범위에 들어가면 실행 , STAY : 범위에 들어가 있으면 실행, EXIT : 범위에서 나가면 실행 
    {
        ENTER, STAY, EXIT, NONE
    }

    public enum BuffInfo
    {
        NONE,
        SUPERJUMP,

        #region buff
        SPEEDUP,
        POWERUP,
        HEAL,
        #endregion

        #region debuff
        TICKDAMAGE

        #endregion
        
    }

   


    #region 플레이어
    public enum TempoType
    {
        MAIN, POINT, NONE
    }

    public enum AttackState
    {
        ATTACK, CHECK, FINISH
    }

    public enum PlayerState
    {
        OVERLOAD, STUN, NONE
    }

    public enum CircleState
    {
        MISS, BAD, GOOD, PERFECT, NONE
    }

    public enum PlayerSfxType
    {
        MAIN, POINT, DASH, RUN, JUMP, STUN, NONE
    }
    #endregion

    #region 몬스터
    public enum PerceptionType
    {
        PATROL, BOUNDARY, DETECTIONM
    }

    #region 엘리트 몬스터
    public enum ElitePhaseState
    {
        START, PHASE1, PHASECHANGE, PHASE2, FINISH, NONE
    }
    public enum EliteMonsterState
    {
        IDLE, USESKILL, GROGGY, FAIL, DIE, NONE
    }
    public enum EliteMonsterSkill
    {
        FATTACK, SATTACK, LASER, LAUNCH, RUSH, SUPERPUNCH, BARRIER, THUNDERSTROKE, EXPLOSION, PLATFORM, NONE
    }
    #endregion

    #endregion

}
