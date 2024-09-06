public abstract class PlayerAttackState
{
    protected Player _player;

    public PlayerAttackState(Player player)
    {
        _player = player;
    }

    public abstract void Initialize();
    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
