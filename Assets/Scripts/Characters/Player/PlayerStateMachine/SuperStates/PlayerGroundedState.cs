
using UnityEngine;

namespace Game
{
    public class PlayerGroundedState<T> : PlayerState<T> where T : PlayerStateData
    {
        protected int xInput;
        protected float xInputRaw;
        protected bool jumpInputDown;

        protected bool isGrounded;

        public PlayerGroundedState(Player player, T data) : base(player, data)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = CollisionSenses.Ground;
        }

        public override void Enter()
        {
            base.Enter();
            player.DoubleJumpState.ResetDoubleJump();
            player.JumpState.ResetJump();

            Movement.FallSpeed = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            xInput = inputHandler.NormMoveInput.x;
            xInputRaw = inputHandler.MoveInput.x;

            jumpInputDown = inputHandler.JumpInputDown;

            if (player.StateMachine.CurrentState != player.LandState && !isExitingState)
            {
                if (!isGrounded)
                {
                    player.InAirState.StartCoyoteTime();
                    stateMachine.TransitionTo(player.InAirState);
                }
                else if (jumpInputDown)
                {
                    stateMachine.TransitionTo(player.JumpState);
                }
            }
        }
    }
}