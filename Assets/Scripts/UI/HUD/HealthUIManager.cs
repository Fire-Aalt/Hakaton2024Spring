using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CoreSystem;

namespace Game
{
    public class HealthUIManager : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private HeartUI _heart;

        private readonly List<HeartUI> _hearts = new();
        private int _currentHearts;
        private int _maxHearts;

        public float RightBorder { get; private set; }

        private void Awake()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void OnEnable()
        {
            PlayerStats.OnCurrentHealthChanged += HandleCurrentHealthChanged;
            PlayerStats.OnMaxHealthChanged += HandleMaxHealthChanged;
            PlayerStats.OnHealthInitialized += HandleHealthInitialized;
        }

        private void OnDisable()
        {
            PlayerStats.OnCurrentHealthChanged -= HandleCurrentHealthChanged;
            PlayerStats.OnMaxHealthChanged -= HandleMaxHealthChanged;
            PlayerStats.OnHealthInitialized -= HandleHealthInitialized;
        }

        private void HandleHealthInitialized(int startHealth)
        {
            HandleMaxHealthChanged(startHealth);
        }

        private void HandleCurrentHealthChanged(int newCurrentHearts)
        {
            if (newCurrentHearts > _currentHearts)
            {
                for (int i = _currentHearts; i < newCurrentHearts; i++)
                {
                    _hearts[i].Heal();
                }
            }
            else if (newCurrentHearts < _currentHearts)
            {
                for (int i = _currentHearts; i > newCurrentHearts; i--)
                {
                    _hearts[i - 1].Hurt();
                }
            }

            _currentHearts = newCurrentHearts;
        }

        private void HandleMaxHealthChanged(int newMaxHearts)
        {
            _currentHearts += newMaxHearts - _maxHearts;
            _maxHearts = newMaxHearts;
            foreach (HeartUI heart in _hearts)
            {
                Destroy(heart.gameObject);
            }
            _hearts.Clear();

            for (int i = 0; i < newMaxHearts; i++)
            {
                HeartUI heart = Instantiate(_heart, transform);
                _hearts.Add(heart);
            }
            for (int i = 0; i < newMaxHearts; i++)
            {
                if (i < _currentHearts)
                {
                    _hearts[i].Initialize(FillState.Full);
                }
                else
                {
                    _hearts[i].Initialize(FillState.Empty);
                }
            }
        }
    }
}