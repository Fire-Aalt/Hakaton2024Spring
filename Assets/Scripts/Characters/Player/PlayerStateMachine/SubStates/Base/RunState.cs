using UnityEngine;

namespace Game
{
    public class PlayerRunState : PlayerGroundedState<PlayerRunData>
    {
        private bool isTouchingWall;

        public PlayerRunState(Player player, PlayerRunData data) : base(player, data)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isTouchingWall = CollisionSenses.Wall;
        }

        public override void Enter()
        {
            base.Enter();

            //player.FXController.FootstepVFX(PlayerFXController.FootstepType.Run, true);
        }

        public override void Exit()
        {
            base.Exit();

            //player.FXController.FootstepVFX(PlayerFXController.FootstepType.Run, false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                Movement.CheckIfShouldFlip(xInput);

                if ((xInput == 0 && Mathf.Abs(Movement.CurrentVelocity.x) < 0.05f) || (Movement.FacingDirection == xInput && isTouchingWall))
                {
                    stateMachine.TransitionTo(player.IdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Movement.Move(xInputRaw, 1);
        }
    }
}