using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerAbilityInventory : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private Transform _abilityDataRoot;

        private readonly HashSet<PlayerAbilities> _abilities;
        private PlayerAbilityData[] _playerAbilityDatas;

        private void Awake()
        {
            _playerAbilityDatas = _abilityDataRoot.GetComponentsInChildren<PlayerAbilityData>();

            foreach (var abilityData in _playerAbilityDatas)
            {
                if (abilityData.Ability == PlayerAbilities.None)
                    abilityData.Unlocked = true;
            }
        }

        public void UnlockAbility(PlayerAbilities playerAbility)
        {
            var data = _playerAbilityDatas.FirstOrDefault(d => d.Ability == playerAbility);

            if (data != null)
                data.Unlocked = true;
        }

        public void UnlockAbility(HashSet<PlayerAbilities> playerAbilities)
        {
            foreach (var playerAbility in playerAbilities)
            {
                UnlockAbility(playerAbility);
            }
        }

        public void AddAbility(PlayerAbilities ability)
        {
            _abilities.Add(ability);
            UnlockAbility(ability);
        }

        //private void OnEnable()
        //{
        //    InteractableAbility.OnAddPlayerAbility += AddAbility;
        //}

        //private void OnDisable()
        //{
        //    InteractableAbility.OnAddPlayerAbility -= AddAbility;
        //}
    }

    public enum PlayerAbilities
    {
        None,
        Climb,
        DoubleJump
    }
}
