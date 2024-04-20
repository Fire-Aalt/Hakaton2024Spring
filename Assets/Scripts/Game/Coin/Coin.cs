using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event Action<int> OnCoinCollected;

    [Header("Properties")]
    [SerializeField] private int _coinValue;

    private bool _canBeCollected;
    private bool _collected;

    public void Collect()
    {
        if (!_canBeCollected || _collected)
        {
            return;
        }

        _canBeCollected = false;
        _collected = true;
        OnCoinCollected?.Invoke(_coinValue);
        Destroy(gameObject);
    }
}