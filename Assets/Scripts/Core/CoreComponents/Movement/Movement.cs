using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CoreSystem
{
    public class Movement : CoreComponent
    {
        [SerializeField] private float GravityScale;
        [SerializeField] private Rigidbody2D _rigidbody;
        public Rigidbody2D RB { get; private set; }

        public bool CanSetVelocity { get; set; }

        public int FacingDirection { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }

        public float FallSpeed { get; set; }

        private Vector2 workspace;

        protected CollisionSenses _collisionSenses;

        protected override void Awake()
        {
            base.Awake();

            if (core.HasCoreComponent<CollisionSenses>())
                _collisionSenses = core.GetCoreComponent<CollisionSenses>();

            if (_rigidbody != null)
                RB = _rigidbody;
            else
                RB = GetComponentInParent<Rigidbody2D>();

            FacingDirection = 1;
            CanSetVelocity = true;
        }

        public override void LogicUpdate()
        {
            CurrentVelocity = RB.velocity;
            if (_collisionSenses != null && !_collisionSenses.Ground && CurrentVelocity.y < -Mathf.Epsilon)
            {
                FallSpeed = -CurrentVelocity.y;
            }
        }

        #region Set Functions

        public void SetVelocityZero(bool ignoreRestriction = false)
        {
            workspace = Vector2.zero;
            SetFinalVelocity(ignoreRestriction);
        }

        public void SetVelocity(float velocity, Vector2 angle, bool ignoreRestriction = false)
        {
            angle.Normalize();
            workspace = new Vector2(angle.x * velocity, angle.y * velocity);
            SetFinalVelocity(ignoreRestriction);
        }

        public void SetVelocity(float velocityX, float velocityY, bool ignoreRestriction = false)
        {
            workspace = new Vector2(velocityX, velocityY);
            SetFinalVelocity(ignoreRestriction);
        }

        public void SetVelocityX(float velocity, bool ignoreRestriction = false)
        {
            workspace = new Vector2(velocity, CurrentVelocity.y);
            SetFinalVelocity(ignoreRestriction);
        }

        public void SetVelocityY(float velocity, bool ignoreRestriction = false)
        {
            workspace = new Vector2(CurrentVelocity.x, velocity);
            SetFinalVelocity(ignoreRestriction);
        }

        public void AddVelocity(float velocity, Vector2 angle, bool ignoreRestriction = false)
        {
            angle.Normalize();
            workspace = new Vector2(CurrentVelocity.x + angle.x * velocity, CurrentVelocity.y + angle.y * velocity);
            SetFinalVelocity(ignoreRestriction);
        }

        public void AddForce(Vector2 velocity, ForceMode2D mode, bool ignoreRestriction)
        {
            if (ignoreRestriction || CanSetVelocity)
            {
                RB.AddForce(velocity, mode);
            }
        }

        public void FixedAccelerateX(float acceleration, float maxSpeed, int direction)
        {
            float speedDif = (direction * maxSpeed) - CurrentVelocity.x;
            float movement = speedDif * acceleration;
            workspace = movement * Vector2.right;
            if (CanSetVelocity)
            {
                RB.AddForce(workspace, ForceMode2D.Force);
            }
        }

        private void SetFinalVelocity(bool ignoreRestriction)
        {
            if (CanSetVelocity || ignoreRestriction)
            {
                RB.velocity = workspace;
                CurrentVelocity = workspace;
            }
        }

        #endregion

        #region Deccelerate()
        public void Decelerate(float moveDeccelAmount, float deccelInAir, float targetSpeed)
        {
            float accelRate;

            if (_collisionSenses.Ground)
                accelRate = moveDeccelAmount;
            else
                accelRate = moveDeccelAmount * deccelInAir;

            ApplyMovement(accelRate, targetSpeed);
        }
        public void Deccelerate(float moveDeccelAmount, float targetSpeed)
        {
            float accelRate = moveDeccelAmount;

            ApplyMovement(accelRate, targetSpeed);
        }
        #endregion

        protected void ApplyMovement(float accelRate, float targetSpeed)
        {
            float speedDif = targetSpeed - CurrentVelocity.x;
            float movement = speedDif * accelRate;

            AddForce(movement * Vector2.right, ForceMode2D.Force, false);
        }

        public virtual bool CheckIfShouldFlip(float XInput)
        {
            if (XInput != 0 && XInput != FacingDirection)
            {
                Flip();
                return true;
            }
            return false;
        }

        public virtual void Flip()
        {
            FacingDirection *= -1;
            RB.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public void DisableGravity()
        {
            RB.gravityScale = 0f;
            SetVelocityZero();
        } 

        public void SetGravityScale(float newGravityScale) => RB.gravityScale = newGravityScale;

        public virtual void EnableGravity() => RB.gravityScale = GravityScale;

        public virtual void ResetDrag()
        {
            RB.drag = 0f;
            RB.gravityScale = GravityScale;
        }
        public virtual void SetDrag(float drag)
        {
            RB.drag = drag;
            RB.gravityScale = GravityScale + drag;
        }
    }
}
