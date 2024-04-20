using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _scoreContainer;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _spacing;
        private int _displayedValue = 0;
        private int _displayedDigit = 1;

        private void Start()
        {
            UpdateScore(_displayedValue);
        }

        private void UpdateScore(int value)
        {
            _displayedValue += value;
            _text.text = _displayedValue.ToString();

            if (_text.text.Length > _displayedDigit)
            {
                _displayedDigit = _text.text.Length;
                float width = _text.preferredWidth;
                _scoreContainer.sizeDelta = new Vector2(_spacing + width, _scoreContainer.sizeDelta.y);
            }
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