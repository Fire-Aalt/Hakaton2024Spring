using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerHitState : PlayerState<PlayerHitData>
    {
        private readonly Timer _shockTimer = new();

        public PlayerHitState(Player player, PlayerHitData data) : base(player, data)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _shockTimer.StartTimer(playerData.shockTime);
            _shockTimer.OnTimerDone += ExitState;

            PlayMMFeedback(data.HitFeedback);
        }

        public override void Exit()
        {
            base.Exit();

            _shockTimer.OnTimerDone -= ExitState;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _shockTimer.Tick();
        }

        private void ExitState()
        {
            stateMachine.TransitionTo(player.IdleState);
        }
    }
}