using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAbilityState<T> : PlayerState<T> where T : PlayerStateData
    {
        protected static bool isAbilityDone;
        protected bool isGrounded;

        protected int xInput;
        protected float xInputRaw;

        public PlayerAbilityState(Player player, T data) : base(player, data)
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
            isAbilityDone = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = inputHandler.NormMoveInput.x;
            xInputRaw = inputHandler.MoveInput.x;

            if (isAbilityDone)
            {
                if (isGrounded && Movement.CurrentVelocity.y < 0.1)
                {
                    stateMachine.TransitionTo(player.IdleState);
                }
                else
                {
                    stateMachine.TransitionTo(player.InAirState);
                }
            }
            else
            {
                if (CollisionSenses.Ground && Movement.FallSpeed >= playerData.landThresholdVelocity)
                {
                    stateMachine.TransitionTo(player.LandState);
                }
            }
        }
    }
}