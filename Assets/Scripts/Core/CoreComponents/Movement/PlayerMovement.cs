using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CoreSystem
{
    public class PlayerMovement : Movement
    {
        [SerializeField] private PlayerMovementDataSO MovementData;

        public event Action<int> OnPlayerFlipped;

        public bool IsFlipping { get; set; }

        private Player _player;

        protected override void Start()
        {
            base.Start();

            _player = GetComponentInParent<Player>();
        }

        public void Move(float xInput, float lerp)
        {
            CheckGravity();

            float targetSpeed = xInput * MovementData.moveMaxSpeed;

            targetSpeed = Mathf.Lerp(CurrentVelocity.x, targetSpeed, lerp);

            if (Mathf.Abs(targetSpeed) > 0.01f)
            {
                Accelerate(MovementData.moveAccelAmount, MovementData.accelInAir, targetSpeed);
            }
            else
            {
                Decelerate(MovementData.moveDeccelAmount, MovementData.deccelInAir, targetSpeed);
            }
        }

        public void CheckGravity()
        {
            if (CurrentVelocity.y < -Mathf.Epsilon)
            {
                SetGravityScale(MovementData.gravityScale * MovementData.fallGravityMult);
                SetVelocityY(Mathf.Max(-FallSpeed, -MovementData.maxFallSpeed));
            }
            else
            {
                SetGravityScale(MovementData.gravityScale);
            }
        }

        #region Accelerate()
        public void Accelerate(float moveAccelAmount, float accelInAir, float targetSpeed)
        {
            float accelRate;

            if (_collisionSenses.Ground)
                accelRate = moveAccelAmount;
            else
                accelRate = moveAccelAmount * accelInAir;

            ApplyMovement(accelRate, targetSpeed);
        }

        public void Accelerate(float moveAccelAmount, float targetSpeed)
        {
            float accelRate = moveAccelAmount;

            ApplyMovement(accelRate, targetSpeed);
        }
        #endregion

        public bool CheckIfShouldLateFlip(int xInput)
        {
            bool performed = false;
            if (xInput != 0)
            {
                if (!IsFlipping && xInput != FacingDirection)
                {
                    IsFlipping = true;
                    performed = true;
                }
                if (IsFlipping && xInput == FacingDirection)
                {
                    Flip();
                    var currentState = _player.Animator.GetCurrentAnimatorStateInfo(0);
                    var stateName = currentState.fullPathHash;
                    _player.Animator.Play(stateName, 0, 0.0f);
                    IsFlipping = true;
                    performed = true;
                }
            }

            return performed;
        }

        public override bool CheckIfShouldFlip(float XInput)
        {
            bool flipped = base.CheckIfShouldFlip(XInput);
            if (!flipped && IsFlipping && XInput != FacingDirection)
            {
                Flip();
                return true;
            }
            return flipped;
        }

        public override void Flip()
        {
            base.Flip();

            IsFlipping = false;
            OnPlayerFlipped?.Invoke(FacingDirection);
        }

        public override void EnableGravity() => RB.gravityScale = MovementData.gravityScale;

        public override void ResetDrag()
        {
            RB.drag = 0f;
            RB.gravityScale = MovementData.gravityScale;
        }
        public override void SetDrag(float drag)
        {
            RB.drag = drag;
            RB.gravityScale = MovementData.gravityScale + drag;
        }
    }
}