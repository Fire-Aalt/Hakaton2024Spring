using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private int _displayedValue;

        private void Start()
        {
            _displayedValue = 0;
            UpdateScore(_displayedValue);
        }

        private void UpdateScore(int value)
        {
            _displayedValue += value;
            _text.text = _displayedValue.ToString();
        }

        private void OnEnable()
        {
            CollectibleManager.OnCoinValueChanged += UpdateScore;
        }

        private void OnDisable()
        {
            CollectibleManager.OnCoinValueChanged -= UpdateScore;
        }
    }
}