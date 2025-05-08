using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputSystemActions;
    public event Action<Vector2> TouchStarted;
    public event Action<Vector2> TouchEnded;
    public Vector2 TouchStartPosition { get; private set; }
    public Vector2 TouchCurrentPosition { get; private set; }
    public bool TouchHeld { get; private set; } = false;

    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputSystemActions.Enable();
        _inputSystemActions.Player.TouchPoint.performed += OnTouchPerformed;
        _inputSystemActions.Player.TouchPoint.canceled += OnTouchCanceled;
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.TouchPoint.performed -= OnTouchPerformed;
        _inputSystemActions.Player.TouchPoint.canceled -= OnTouchCanceled;
        _inputSystemActions.Disable();
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Touch");
        TouchHeld = true;
        Vector2 TouchPosition = context.ReadValue<Vector2>();
        TouchStartPosition = TouchPosition;
        TouchCurrentPosition = TouchPosition;
        TouchStarted?.Invoke(TouchPosition);
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Release");
        TouchHeld = false;
        TouchEnded?.Invoke(TouchCurrentPosition);

        TouchStartPosition = Vector2.zero;
        TouchCurrentPosition = Vector2.zero;
    }

    private void Update()
    {
        if (TouchHeld)
        {
            TouchCurrentPosition = _inputSystemActions.Player.TouchPoint.ReadValue<Vector2>();
        }
    }
}
