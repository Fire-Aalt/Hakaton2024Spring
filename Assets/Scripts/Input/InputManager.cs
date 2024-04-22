using RenderDream.GameEssentials;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public static bool TouchInputEnabled;

    public static event Action<Vector2> MoveEvent;
    public static event Action JumpEvent;
    public static event Action JumpCancelEvent;

    public static event Action InteractEvent;
    public static event Action InteractCancelEvent;

    public static event Action OpenInventoryEvent;
    public static event Action CloseInventoryEvent;

    [SerializeField] private bool _isTouchEnabled;

    [SerializeField][Range(0, 1)] private float minJoystickInput;
    [SerializeField][Range(0, 1)] private float maxJoystickInput;

    private bool _inited = false;

    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        if (_isTouchEnabled)
        {
            TouchInputEnabled = true;
            _inited = true;
        }
#endif
    }

    private void Update()
    {
        if (!_inited && Input.touchCount > 0)
        {
            TouchInputEnabled = true;
            _inited = true;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpInput(context.phase);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractInput(context.phase);
    }

    public void OnOpenUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OpenInventoryEvent?.Invoke();
        }
    }

    public void OnCloseUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CloseInventoryEvent?.Invoke();
        }
    }
    
    public void MovementInput(Vector2 vector)
    {
        Vector2 calibratedMoveInput = new(ApplyDeadzone(vector.x), ApplyDeadzone(vector.y));
        MoveEvent?.Invoke(calibratedMoveInput);
    }

    public void JumpInput(InputActionPhase phase)
    {
        if (phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
        if (phase == InputActionPhase.Canceled)
        {
            JumpCancelEvent?.Invoke();
        }
    }

    public void InteractInput(InputActionPhase phase)
    {
        if (phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
        if (phase == InputActionPhase.Canceled)
        {
            InteractCancelEvent?.Invoke();
        }
    }

    private float ApplyDeadzone(float input)
    {
        float mult = Mathf.Sign(input);
        input = Mathf.Abs(input);

        if (input <= minJoystickInput)
        {
            input = 0;
        }
        else if (input >= maxJoystickInput)
        {
            input = mult;
        }
        else
        {
            input *= mult;
        }

        return input;
    }
}
