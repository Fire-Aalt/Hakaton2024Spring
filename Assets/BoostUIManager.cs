using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BoostUIManager : MonoBehaviour
    {
        [SerializeField] private HealthUIManager _healthUIManager;
        [SerializeField] private BoostUI _pointsDouble;
        [SerializeField] private BoostUI _shield;
        [SerializeField] private int _newRowThreshould;

        [SerializeField] private float _newRowHeight;
        [SerializeField] private float _firstRowHeight;

        bool _isPointsDoubleActive;
        bool _isShieldActive;
        RectTransform _rect;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _pointsDouble.SetActive(false);
            _shield.SetActive(false);
        }

        private void Update()
        {
            if (BoostManager.PointsDoubleTimeLeft > 0)
            {
                _pointsDouble.SetActive(true);
                _pointsDouble.UpdateText(BoostManager.PointsDoubleTimeLeft);
                _isPointsDoubleActive = true;
            }
            else if (_isPointsDoubleActive)
            {
                _pointsDouble.SetActive(false);
                _isPointsDoubleActive = false;
            }

            if (BoostManager.ShieldTimeLeft > 0)
            {
                _shield.SetActive(true);
                _shield.UpdateText(BoostManager.ShieldTimeLeft);
                _isShieldActive = true;
            }
            else if (_isShieldActive)
            {
                _shield.SetActive(false);
                _isShieldActive = false;
            }

            CheckForAdaptive();
        }

        private void CheckForAdaptive()
        {
            if (Screen.width < _newRowThreshould)
            {
                _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _newRowHeight);
            }
            else
            {
                _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _firstRowHeight);
            }
        }
    }
}
