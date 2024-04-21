using System;
using UnityEngine;

namespace Game.CoreSystem
{
    public class PlayerStats : Stats
    {
        public static event Action<int> OnHealthInitialized;

        public static event Action<int> OnCurrentHealthChanged;
        public static event Action<int> OnMaxHealthChanged;

        protected override bool ExposeProperties => true;

        protected override void Start()
        {
            base.Start();

            OnHealthInitialized?.Invoke(MaxHealth);
        }

        #region Health Funcs
        public override void UpdateCurrentHealth(int amount)
        {
            if (CurrentHealth == 0)
            {
                return;
            }
            base.UpdateCurrentHealth(amount);

            OnCurrentHealthChanged?.Invoke(CurrentHealth);
        }

        public override void UpdateMaxHealth(int amount)
        {
            base.UpdateMaxHealth(amount);

            OnMaxHealthChanged?.Invoke(MaxHealth);
        }
        #endregion
    }
}