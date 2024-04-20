using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerIdleState : PlayerGroundedState<PlayerIdleData>
    {
        private bool isTouchingWall;

        public PlayerIdleState(Player player, PlayerIdleData data) : base(player, data)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            if (CollisionSenses)
            {
                isTouchingWall = CollisionSenses.Wall;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!isExitingState)
            {
                if (!isGrounded)
                {
                    stateMachine.TransitionTo(player.InAirState);
                }
                else if (!(Movement.FacingDirection == xInput && isTouchingWall) && xInput != 0)
                {
                    stateMachine.TransitionTo(player.RunState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Movement.Move(0, 1);
        }
    }
}