
namespace Game
{
    public class PlayerDeathState : PlayerState<PlayerDeathData>
    {
        private readonly Timer _changeLocationTimer = new();

        public PlayerDeathState(Player player, PlayerDeathData data) : base(player, data)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Movement.DisableGravity();
            Combat.Invincibility.IsActionInvincible = true;

            _changeLocationTimer.StartTimer(data.DeathFeedback.TotalDuration);

            PlayMMFeedback(data.DeathFeedback);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _changeLocationTimer.Tick();

            Movement.SetVelocityZero();
        }

        public override void Exit()
        {
            base.Exit();

            Movement.EnableGravity();

            Combat.Invincibility.IsActionInvincible = false;
        }
    }
}