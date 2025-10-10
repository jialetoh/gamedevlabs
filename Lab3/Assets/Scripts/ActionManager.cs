using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    public UnityEvent jump;
    public UnityEvent jumpHold;
    public UnityEvent<int> moveCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickAction(InputAction.CallbackContext context)
    {
        // if (context.started)
        //     Debug.Log("mouse click started");
        // else if (context.performed)
        // {
        //     Debug.Log("mouse click performed");
        // }
        // else if (context.canceled)
        //     Debug.Log("mouse click cancelled");
    }

    public void OnPointAction(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        //     Vector2 point = context.ReadValue<Vector2>();
        //     Debug.Log($"Point detected: {point}");
        // }
    }

    public void OnJumpHoldAction(InputAction.CallbackContext context)
    {
        if (context.performed) jumpHold.Invoke();
        // if (context.started)
        //     Debug.Log("JumpHold was started");
        // else if (context.performed)
        // {
        //     Debug.Log("JumpHold was performed");
        //     Debug.Log(context.duration);
        //     jumpHold.Invoke();
        // }
        // else if (context.canceled)
        //     Debug.Log("JumpHold was cancelled");
    }

    // // triggered upon performed interaction (custom successful hold)
    // public void OnJumphold(InputValue value)
    // {
    //     Debug.Log($"OnJumpHold performed with value {value.Get()}");
    //     // TODO

    // }

    // called twice, when pressed and unpressed
    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.performed) jump.Invoke();
        // if (context.started)
        //     Debug.Log("Jump was started");
        // else if (context.performed)
        // {
        //     jump.Invoke();
        //     Debug.Log("Jump was performed");
        // }
        // else if (context.canceled)
        //     Debug.Log("Jump was cancelled");

    }
    // // triggered upon performed interaction (default successful press)
    // public void OnJump()
    // {
    //     Debug.Log("OnJump called");
    //     // TODO
    // }

    // called twice, when pressed and unpressed
    public void OnMoveAction(InputAction.CallbackContext context)
    {
        // Debug.Log("OnMoveAction callback invoked");
        if (context.started)
        {
            // Debug.Log("move started");
            int faceRight = context.ReadValue<float>() > 0 ? 1 : -1;
            moveCheck.Invoke(faceRight);
        }
        if (context.canceled)
        {
            // Debug.Log("move stopped");
            moveCheck.Invoke(0);
        }
    }

    // // triggered upon 1D value change (default successful press and cancelled)
    // public void OnMove(InputValue input)
    // {
    //     if (input.Get() == null)
    //     {
    //         Debug.Log("Move released");
    //     }
    //     else
    //     {
    //         Debug.Log($"Move triggered, with value {input.Get()}"); // will return null when released
    //     }
    //     // TODO
    // }
}
