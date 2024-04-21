using Game.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BoostManager : MonoBehaviour
    {
        [SerializeField] private int _healthUpgradeAmount;
        [SerializeField] private float _pointsDoubleMultiplier;
        [SerializeField] private int _pointsDoubleDuration;
        [SerializeField] private int _shieldDuration;

        private Core core;
        private PlayerStats playerStats;
        private PlayerHurtbox playerHurtbox;

        public static float PointsDoubleTimeLeft { get; private set; }
        public static float ShieldTimeLeft { get; private set; }

        private void Start()
        {
            core = GetComponentInParent<Player>().Core;
            playerStats = core.GetCoreComponent<PlayerStats>();
            playerHurtbox = core.GetCoreComponent<PlayerHurtbox>();
        }

        private void ApplyBoost(BoostType type)
        {
            switch (type)
            {
                case BoostType.HealthUpgrade:
                    playerStats.UpdateMaxHealth(1);
                    break;
                case BoostType.PointsDouble:
                    PointsDoubleTimeLeft += _pointsDoubleDuration;
                    PointsDoubleTimeLeft = Mathf.Clamp(PointsDoubleTimeLeft, 0, 99);
                    CollectibleManager.Current.CoinMultiplier = _pointsDoubleMultiplier;
                    break;
                case BoostType.Shield:
                    ShieldTimeLeft += _shieldDuration;
                    ShieldTimeLeft = Mathf.Clamp(ShieldTimeLeft, 0, 99);
                    playerHurtbox.Invincibility.IsActionInvincible = true;
                    break;
                case BoostType.AddHealth:
                    playerStats.UpdateCurrentHealth(1);
                    break;
            }
        }

        private void Update()
        {
            if (PointsDoubleTimeLeft > 0)
            {
                PointsDoubleTimeLeft -= Time.deltaTime;

                if (PointsDoubleTimeLeft <= 0)
                {
                    CollectibleManager.Current.CoinMultiplier = 1f;
                }
            }
            if (ShieldTimeLeft > 0)
            {
                ShieldTimeLeft -= Time.deltaTime;

                if (ShieldTimeLeft <= 0)
                {
                    playerHurtbox.Invincibility.IsActionInvincible = false;
                }
            }
        }

        private void OnEnable()
        {
            Boost.OnBoostCollected += ApplyBoost;
        }

        private void OnDisable()
        {
            Boost.OnBoostCollected -= ApplyBoost;
        }
    }
}
