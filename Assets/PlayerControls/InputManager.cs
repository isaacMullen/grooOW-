using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputActions;

public class InputManager : MonoBehaviour, IPlayerActions
{     
    private InputActions inputActions;

    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {           
            Actions.OnMovePerformed?.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            Actions.OnMovePerformed?.Invoke(Vector2.zero);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pressed Space");
            Actions.OnJumpPerformed?.Invoke();
        }       
    }
}
