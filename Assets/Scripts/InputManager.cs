using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // The input action asset generated from the Input System
    private InputSystem_Actions _playerInputActions;

    /// <summary>
    /// Singleton instance of InputManager.
    /// </summary>
    public static InputManager Instance { get; private set; }

    #region Input Events

    /// <summary>
    /// Fired when the player provides movement input (e.g., WASD or analog stick).
    /// Vector2 represents horizontal and vertical axes.
    /// </summary>
    public event Action<Vector2> OnMove;

    /// <summary>
    /// Fired when the player moves the look input (mouse or controller stick).
    /// Provides the input device and look vector.
    /// </summary>
    public event Action<Vector2> OnLook;

    /// <summary>
    /// Fired when the player presses the interact button.
    /// </summary>
    public event Action OnInteract;

    /// <summary>
    /// Fired when the player presses the jump button.
    /// </summary>
    public event Action OnJump;

    /// <summary>
    /// Fired when the player presses the crouch button.
    /// </summary>
    public event Action OnCrouch;

    #endregion

    // when enabling this script, subscribe to all input system asset events
    private void OnEnable()
    {
        if (_playerInputActions == null) return;

        // Subscribe to input events
        _playerInputActions.Player.Move.performed += HandleMoveInput;
        _playerInputActions.Player.Look.performed += HandleLookInput;
        _playerInputActions.Player.Move.canceled += HandleMoveInput;
        _playerInputActions.Player.Look.canceled += HandleLookInput;

        _playerInputActions.Player.Jump.performed += HandleJumpInput;
        _playerInputActions.Player.Crouch.performed += HandleCrouchInput;

        _playerInputActions.Player.Interact.performed += HandleInteractInput;
    }

    // when disabling this script, unsubscribe from all input system asset events
    private void OnDisable()
    {
        if (_playerInputActions == null) return;

        // Unsubscribe from input events
        _playerInputActions.Player.Move.performed -= HandleMoveInput;
        _playerInputActions.Player.Look.performed -= HandleLookInput;
        _playerInputActions.Player.Move.canceled -= HandleMoveInput;
        _playerInputActions.Player.Look.canceled -= HandleLookInput;

        _playerInputActions.Player.Jump.performed -= HandleJumpInput;
        _playerInputActions.Player.Crouch.performed -= HandleCrouchInput;

        _playerInputActions.Player.Interact.performed -= HandleInteractInput;
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        // Initialize input action asset
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Enable();
    }

    public void TurnOffInputs()
    {
        OnMove?.Invoke(Vector2.zero);
        OnLook?.Invoke(Vector2.zero);
        _playerInputActions.Player.Disable();
    }

    public void TurnOnInputs()
    {
        _playerInputActions.Player.Enable();
    }

    #region Handlers

    private void HandleMoveInput(InputAction.CallbackContext ctx) => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    private void HandleLookInput(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx.ReadValue<Vector2>());
    private void HandleInteractInput(InputAction.CallbackContext ctx) => OnInteract?.Invoke();
    private void HandleJumpInput(InputAction.CallbackContext ctx) => OnJump?.Invoke();
    private void HandleCrouchInput(InputAction.CallbackContext ctx) => OnCrouch?.Invoke();

    #endregion
}
