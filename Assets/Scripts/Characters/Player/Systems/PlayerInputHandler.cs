using RenderDream.GameEssentials;
using System;
using UnityEngine;

namespace Game
{
    public class PlayerInputHandler : Singleton<PlayerInputHandler>
    {
        public bool DisabledInput { get; private set; }

        private Vector2 _cachedMoveInput;
        public Vector2 MoveInput { get; private set; }
        private Vector2Int _cachedNormMoveInput;
        public Vector2Int NormMoveInput { get; private set; }

        public bool JumpInputDown { get; private set; }
        public bool JumpInputUp { get; private set; }
        public bool InteractInput { get; private set; }

        [field: SerializeField] public float InputHoldTime { get; private set; }
        [SerializeField] private float jumpInputHoldTime = 0.2f;

        private Timer jumpInputHoldTimer;

        protected override void Awake()
        {
            base.Awake();

            jumpInputHoldTimer = new Timer();
        }

        private void Update()
        {
            jumpInputHoldTimer.Tick();
        }

        public void SetInputState(bool isActive)
        {
            DisabledInput = !isActive;
            if (DisabledInput)
            {
                JumpInputDown = false;
                JumpInputUp = false;
                MoveInput = Vector2.zero;
                NormMoveInput = Vector2Int.zero;
            }
            else
            {
                MoveInput = _cachedMoveInput;
                NormMoveInput = _cachedNormMoveInput;
            }
        }

        private void HandleMove(Vector2 rawMovementInput)
        {
            _cachedMoveInput = rawMovementInput;
            _cachedNormMoveInput = new Vector2Int(Math.Sign(rawMovementInput.x), Math.Sign(rawMovementInput.y));
            if (!DisabledInput)
            {
                MoveInput = _cachedMoveInput;
                NormMoveInput = _cachedNormMoveInput;
            }
        }

        private void HandleJump()
        {
            
            if (!DisabledInput)
            {
                JumpInputDown = true;
                JumpInputUp = false;
                jumpInputHoldTimer.StartTimer(jumpInputHoldTime);
            }
        }

        private void HandleCancelJump()
        {
            if (!DisabledInput)
            {
                JumpInputDown = false;
                JumpInputUp = true;
                jumpInputHoldTimer.StopTimer();
            }
        }

        private void HandleInteractStart()
        {
            if (!DisabledInput)
                InteractInput = true;
        }

        private void HandleInteractCancel()
        {
            if (!DisabledInput)
                InteractInput = false;
        }

        public void UseJumpInputDown() => JumpInputDown = false;
        public void UseJumpInputUp() => JumpInputUp = false;
        public void UseInteractInput() => InteractInput = false;

        private void OnEnable()
        {
            jumpInputHoldTimer.OnTimerDone += UseJumpInputDown;

            InputManager.MoveEvent += HandleMove;
            InputManager.JumpEvent += HandleJump;
            InputManager.JumpCancelEvent += HandleCancelJump;

            InputManager.InteractEvent += HandleInteractStart;
            InputManager.InteractCancelEvent += HandleInteractCancel;
        }

        private void OnDisable()
        {
            jumpInputHoldTimer.OnTimerDone -= UseJumpInputDown;

            InputManager.MoveEvent -= HandleMove;
            InputManager.JumpEvent -= HandleJump;
            InputManager.JumpCancelEvent -= HandleCancelJump;

            InputManager.InteractEvent -= HandleInteractStart;
            InputManager.InteractCancelEvent -= HandleInteractCancel;
        }
    }
}