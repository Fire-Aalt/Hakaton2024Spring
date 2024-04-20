using UnityEngine;
using Game.CoreSystem;
using MoreMountains.Feedbacks;

namespace Game
{
    public abstract class PlayerState
    {
        public string FINISH_STATE_KEY => PlayerAnimator.FINISH_STATE_KEY;

        protected Core core;

        protected Player player;
        protected PlayerStateMachine stateMachine;
        protected PlayerDataSO playerData;
        protected PlayerInputHandler inputHandler;

        protected bool awaitAnimation;
        protected bool isExitingState;

        public float enterTime;
        public float exitTime;

        protected PlayerMovement Movement { get => movement ??= core.GetCoreComponent<PlayerMovement>(); }
        private PlayerMovement movement;

        protected CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>(); }
        private CollisionSenses collisionSenses;

        protected PlayerHurtbox Combat { get => combat ??= core.GetCoreComponent<PlayerHurtbox>(); }
        private PlayerHurtbox combat;

        protected PlayerStats Stats { get => stats ??= core.GetCoreComponent<PlayerStats>(); }
        private PlayerStats stats;

        public PlayerState(Player player)
        {
            this.player = player;

            core = player.Core;
            stateMachine = player.StateMachine;
            playerData = player.PlayerData;
            inputHandler = player.InputHandler;
        }

        public virtual void EnterAnimationState(bool awaitAnimation)
        {
            this.awaitAnimation = awaitAnimation;
            if (!this.awaitAnimation)
            {
                EnterCurrentAnimationState();
            }
        }

        public virtual void Enter()
        {
            isExitingState = false;
            enterTime = Time.time;

            DoChecks();
        }

        public virtual void Exit()
        {
            isExitingState = true;
            exitTime = Time.time;
        }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public void EnterCurrentAnimationState()
        {
            if (stateMachine.CurrentAnimationState != null)
            {
                if (!string.IsNullOrEmpty(stateMachine.CurrentAnimationState))
                {
                    player.Animator.SetBool(stateMachine.CurrentAnimationState, false);
                }
            }

            stateMachine.CurrentAnimationState = GetAnimBoolName();
            if (!string.IsNullOrEmpty(stateMachine.CurrentAnimationState))
            {
                player.Animator.SetBool(stateMachine.CurrentAnimationState, true);
            }

            stateMachine.AddToAnimationHistory(stateMachine.CurrentAnimationState);
            awaitAnimation = false;
        }

        protected void PlayMMFeedback(MMF_Player mMF_Player)
        {
            mMF_Player.StopFeedbacks();
            mMF_Player.PlayFeedbacks();
        }

        public abstract string GetAnimBoolName();

        public virtual void DoChecks() { }

        public virtual void DrawGizmos() { }

        public virtual void AnimationTrigger(string key) { }

        public virtual void AnimationFinishTrigger()
        {
            if (awaitAnimation)
            {
                EnterCurrentAnimationState();
            }
        }
    }

    public class PlayerState<T> : PlayerState where T : PlayerStateData
    {
        protected T data;

        public PlayerState(Player player, T data) : base(player)
        {
            this.data = data;
        }

        public override string GetAnimBoolName() => data.AnimBoolName;
    }
}