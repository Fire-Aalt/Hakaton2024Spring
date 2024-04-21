using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerStateMachine
    {
        public const int MAX_ANIMATION_HISTORY_SIZE = 3;

        public PlayerState CurrentState { get; private set; }
        public PlayerState PreviousState { get; private set; }

        public string CurrentAnimationState { get; set; }
        public Queue<string> AnimationStateHistory { get; set; }

        public void Initialize(PlayerState startingState)
        {
            CurrentState = startingState;
            PreviousState = startingState;
            AnimationStateHistory = new Queue<string>();
            CurrentState.Enter();
        }

        public void TransitionTo(PlayerState newState, bool awaitAnimation = false)
        {
            CurrentState.Exit();

            PreviousState = CurrentState;
            CurrentState = newState;

            CurrentState.EnterAnimationState(awaitAnimation);
            CurrentState.Enter();
        }

        public void AddToAnimationHistory(string animationState)
        {
            if (AnimationStateHistory.Count == MAX_ANIMATION_HISTORY_SIZE)
            {
                AnimationStateHistory.Dequeue();
            }
            AnimationStateHistory.Enqueue(animationState);
        }
    }
}
