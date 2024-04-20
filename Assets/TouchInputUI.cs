using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class TouchInputUI : MonoBehaviour
    {
        [SerializeField] private bool _isTouchEnabled;

        private void Start()
        {
#if UNITY_EDITOR
            if (!_isTouchEnabled)
            {
                gameObject.SetActive(false);
            }
#else
            if (SystemInfo.deviceType != DeviceType.Handheld)
            {
                gameObject.SetActive(false);
            }
#endif
        }

        public void LeftArrowDown() => InputManager.Current.MovementInput(Vector2Int.left);
        public void LeftArrowUp() => InputManager.Current.MovementInput(Vector2Int.zero);

        public void RightArrowDown() => InputManager.Current.MovementInput(Vector2Int.right);
        public void RightArrowUp() => InputManager.Current.MovementInput(Vector2Int.zero);

        public void JumpArrowDown() => InputManager.Current.JumpInput(InputActionPhase.Performed);
        public void JumpArrowUp() => InputManager.Current.JumpInput(InputActionPhase.Canceled);
    }
}
