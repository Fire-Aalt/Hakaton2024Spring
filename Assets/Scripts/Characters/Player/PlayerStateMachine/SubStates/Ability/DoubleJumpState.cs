using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerDoubleJumpState : PlayerAbilityState<PlayerDoubleJumpData>
    {
        private bool canDoubleJump;
        public PlayerDoubleJumpState(Player player, PlayerDoubleJumpData data) : base(player, data)
        {
        }

        public override void Enter()
        {
            base.Enter();
            DoubleJump();
            isAbilityDone = true;
        }

        public void DoubleJump()
        {
            canDoubleJump = false;
            inputHandler.UseJumpInputDown();
            player.InAirState.isJumping = true;

            //PlayMMFeedback(data.DoubleJumpFeedback);

            Movement.SetVelocityY(player.MovementData.doubleJumpForce);
        }

        public bool CanDoubleJump() => data.Unlocked && inputHandler.JumpInputDown && canDoubleJump;
        public void ResetDoubleJump() => canDoubleJump = true;
    }
}