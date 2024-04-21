using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Boost : MonoBehaviour
    {
        public static event Action<BoostType> OnBoostCollected;

        [SerializeField] private BoostType _boostType;

        private bool _canBeCollected = true;
        private bool _collected;

        public void Collect()
        {
            if (!_canBeCollected || _collected)
            {
                return;
            }

            _canBeCollected = false;
            _collected = true;
            OnBoostCollected?.Invoke(_boostType);
            Destroy(gameObject);
        }
    }

    public enum BoostType
    {
        HealthUpgrade,
        PointsDouble,
        Shield
    }
}
