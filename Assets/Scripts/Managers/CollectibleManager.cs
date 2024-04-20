using RenderDream.GameEssentials;
using System;
using UnityEngine;

namespace Game
{
    public class CollectibleManager : Singleton<CollectibleManager>
    {
        public static event Action<int> OnCoinValueChanged;

        [SerializeField] private LayerMask _collectibleLayer; 

        public bool CanCollect { get; set; }
        public int CoinValue { get; private set; }

        private BoxCollider2D _collider;
        private Collider2D[] _results;

        protected override void Awake()
        {
            base.Awake();

            _collider = GetComponentInParent<Player>().Collider;
            _results = new Collider2D[100];
            CanCollect = true;
        }

        private void FixedUpdate()
        {
            if (CanCollect)
                SearchForCollectibles();
        }

        private void SearchForCollectibles()
        {
            Rect colliderInfo = new()
            {
                position = (Vector2)_collider.transform.position + _collider.offset,
                size = _collider.size
            };

            int count = Physics2D.OverlapBoxNonAlloc(colliderInfo.position, colliderInfo.size, 0f, _results, _collectibleLayer);

            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _results[i];

                if (collider.TryGetComponent<ICollectible>(out var collectible))
                {
                    collectible.Collect();
                }
            }
        }

        private void OnEnable()
        {
            Coin.OnCoinCollected += ChangeCoinValue;
        }

        private void OnDisable()
        {
            Coin.OnCoinCollected -= ChangeCoinValue;
        }

        public void ChangeCoinValue(int value)
        {
            CoinValue += value;
            OnCoinValueChanged?.Invoke(value);
        }
    }
}
