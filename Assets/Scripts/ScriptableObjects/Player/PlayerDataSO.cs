using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
    public class PlayerDataSO : ScriptableObject
    {
        [Header("Hit State")]
        public float shockTime = 0.2f;

        [Header("In Air State")]
        public float coyoteTime = 0.2f;

        [Header("Land State")]
        public float landThresholdVelocity = -22f;

        [Header("Physics")]
        public LayerMask groundLayer;
        public LayerMask playerLayer;
    }
}