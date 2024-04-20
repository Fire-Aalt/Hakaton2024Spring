using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "newPlayerMovementData", menuName = "Data/Player Data/Movement Data")]
    public class PlayerMovementDataSO : ScriptableObject
    {
        [Header("Gravity")]
        public float gravityScale;

        [Space(5)]
        public float fallGravityMult;
        public float maxFallSpeed;
        [Space(5)]

        [Header("Move")]
        public float moveMaxSpeed;
        public float moveAcceleration;
        [HideInInspector] public float moveAccelAmount;
        public float moveDecceleration;
        [HideInInspector] public float moveDeccelAmount;
        [Space(5)]
        [Range(0f, 1)] public float accelInAir;
        [Range(0f, 1)] public float deccelInAir;

        [Space(20)]

        [Header("Jump")]
        public float jumpForce;
        [Space(5)]
        [Header("Double Jump")]
        public float doubleJumpForce;

        [Header("Both Jumps")]
        [Range(0f, 1)] public float jumpCutVelocityMult;

        private void OnValidate()
        {
            moveAccelAmount = (50 * moveAcceleration) / moveMaxSpeed;
            moveDeccelAmount = (50 * moveDecceleration) / moveMaxSpeed;

            #region Variable Ranges
            moveAcceleration = Mathf.Clamp(moveAcceleration, 0.01f, moveMaxSpeed);
            moveDecceleration = Mathf.Clamp(moveDecceleration, 0.01f, moveMaxSpeed);
            #endregion
        }
    }
}