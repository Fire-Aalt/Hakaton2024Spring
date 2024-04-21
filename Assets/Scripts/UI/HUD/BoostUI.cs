using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class BoostUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public RectTransform RectTransform { get; private set; }
        public bool IsActive { get; private set; }

        private void Start()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            IsActive = isActive;
        }

        public void UpdateText(float newTime)
        {
            int seconds = Mathf.FloorToInt(newTime);

            string time = seconds.ToString();
            if (time != _text.text)
            {
                _text.text = time;
            }
        }
    }
}
