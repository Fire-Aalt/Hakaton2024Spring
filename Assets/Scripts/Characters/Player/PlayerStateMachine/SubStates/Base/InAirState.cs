using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerInAirState : PlayerState<PlayerInAirData>
    {
        private float xInputRaw;
        private int xInput;
        private bool isGrounded;

        public bool coyoteTime;
        public bool isJumping;

        public bool SaveJumpCoyoteTime { get; private set; }
        public bool CanceledSaveJump { get; private set; }

        public PlayerInAirState(Player player, PlayerInAirData data) : base(player, data)
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
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckCoyoteTime();

            xInput = inputHandler.NormMoveInput.x;
            xInputRaw = inputHandler.MoveInput.x;

            CheckForShorterJump();

            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.TransitionTo(player.LandState);
            }
            else if (player.JumpState.CanJump() && coyoteTime)
            {
                stateMachine.TransitionTo(player.JumpState);
            }
            else if (player.DoubleJumpState.CanDoubleJump() && !coyoteTime)
            {
                stateMachine.TransitionTo(player.DoubleJumpState);
            }
            else
            {
                Movement.CheckIfShouldFlip(xInput);
            }
        }

        public void CheckForShorterJump()
        {
            if (isJumping)
            {
                if (inputHandler.JumpInputUp)
                {
                    Movement.SetVelocityY(Movement.CurrentVelocity.y * player.MovementData.jumpCutVelocityMult);

                    isJumping = false;
                }
                else if (Movement.CurrentVelocity.y <= 0f)
                {
                    isJumping = false;
                }
            }
        }

        private void CheckCoyoteTime()
        {
            if (coyoteTime && Time.time > enterTime + playerData.coyoteTime)
            {
                coyoteTime = false;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Movement.Move(xInputRaw, 1);
        }

        public void StartCoyoteTime() => coyoteTime = true;
    }
}