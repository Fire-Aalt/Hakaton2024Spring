using UnityEngine;

namespace Game
{
    public class PlayerLandState : PlayerGroundedState<PlayerLandData>
    {
        private Vector2 _landPos;

        public PlayerLandState(Player player, PlayerLandData data) : base(player, data)
        {
        }

        public override void EnterAnimationState(bool awaitAnimation)
        {
            base.EnterAnimationState(awaitAnimation);
        }

        public override void Enter()
        {
            base.Enter();

            _landPos = player.transform.position;

            Movement.SetVelocityZero();
            player.transform.position = _landPos;
            //player.FXController.LandFX(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (player.JumpState.CanJump())
                {
                    stateMachine.TransitionTo(player.JumpState);
                }
                else if (xInput != 0)
                {
                    stateMachine.TransitionTo(player.RunState);
                }
                else
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