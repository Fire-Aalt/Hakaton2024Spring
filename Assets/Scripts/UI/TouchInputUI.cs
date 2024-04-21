using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class TouchInputUI : MonoBehaviour
    {
        private bool _isJumpPressed;

        private void Start()
        {
            if (!InputManager.TouchInputEnabled)
            {
                gameObject.SetActive(false);
            }
        }

        public void LeftArrowDown() => InputManager.Current.MovementInput(Vector2Int.left);
        public void LeftArrowUp() => InputManager.Current.MovementInput(Vector2Int.zero);

        public void RightArrowDown() => InputManager.Current.MovementInput(Vector2Int.right);
        public void RightArrowUp() => InputManager.Current.MovementInput(Vector2Int.zero);

        public void JumpArrowDown()
        {
            if (!_isJumpPressed)
            {
                InputManager.Current.JumpInput(InputActionPhase.Performed);
                _isJumpPressed = true;
            }
        }

        public void JumpArrowUp()
        {
            if (_isJumpPressed)
            {
                InputManager.Current.JumpInput(InputActionPhase.Canceled);
                _isJumpPressed = false;
            }
        }
    }
}
