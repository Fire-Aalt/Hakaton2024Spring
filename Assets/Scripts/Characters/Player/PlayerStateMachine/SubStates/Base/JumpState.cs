using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerJumpState : PlayerAbilityState<PlayerJumpData>
    {
        public bool HasJump { get; private set; }

        public PlayerJumpState(Player player, PlayerJumpData data) : base(player, data)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Jump();
            isAbilityDone = true;
        }

        public void Jump()
        {
            HasJump = false;
            inputHandler.UseJumpInputDown();
            player.InAirState.isJumping = true;

            //player.FXController.JumpFX(PlayerFXController.JumpType.Jump);

            Movement.SetVelocityY(player.MovementData.jumpForce);
        }

        public void ResetJump() => HasJump = true;
        public bool CanJump() => data.Unlocked && inputHandler.JumpInputDown && HasJump && CollisionSenses.Ground;
    }
}