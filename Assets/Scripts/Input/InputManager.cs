using RenderDream.GameEssentials;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public static event Action<Vector2> MoveEvent;
    public static event Action JumpEvent;
    public static event Action JumpCancelEvent;

    public static event Action InteractEvent;
    public static event Action InteractCancelEvent;

    public static event Action OpenInventoryEvent;
    public static event Action CloseInventoryEvent;

    [SerializeField][Range(0, 1)] private float minJoystickInput;
    [SerializeField][Range(0, 1)] private float maxJoystickInput;

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 rawMovementInput = context.ReadValue<Vector2>();
        Vector2 calibratedMoveInput = new(ApplyDeadzone(rawMovementInput.x), ApplyDeadzone(rawMovementInput.y));
        MoveEvent?.Invoke(calibratedMoveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            InteractCancelEvent?.Invoke();
        }
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
