using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravity;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip doubleJumpClip;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private Vector3 _finalDirection;
    private float _verticalVelocity = -2f;
    private bool _hasDoubleJump;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += HandleMove;
            InputManager.Instance.OnJump += HandleJump;
        }

        PlayerCrouch.OnPlayerCrouch += ChangePlayerSpeed;

        StartCoroutine(PlayFootstepSound());
    }

    private void Update()
    {
        if (_controller.isGrounded) _hasDoubleJump = true;

        ApplyGravity();

        _moveDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _finalDirection = _moveDirection * moveSpeed;

        _finalDirection.y = _verticalVelocity;

        _controller.Move(_finalDirection * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        else if (!_controller.isGrounded)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private IEnumerator PlayFootstepSound()
    {
        while (true)
        {
            if (_controller.isGrounded && _moveInput != Vector2.zero && !PlayerCrouch.IsCrouch)
            {
                footstepAudioSource.PlayOneShot(footstepClip);
                yield return new WaitForSeconds(0.6f);
            }
            yield return null;
        }
    }

    private void HandleMove(Vector2 input)
    {
        _moveInput = input;
    }

    private void HandleJump()
    {
        if (_controller.isGrounded && !PlayerCrouch.IsCrouch)
        {
            _verticalVelocity = jumpPower;
            playerAudioSource.PlayOneShot(jumpClip);
        }

        else if (!_controller.isGrounded && _hasDoubleJump && !PlayerCrouch.IsCrouch)
        {
            _verticalVelocity = jumpPower;
            playerAudioSource.PlayOneShot(doubleJumpClip);
            _hasDoubleJump = false;
        }
    }

    private void ChangePlayerSpeed(bool isCrouch)
    {
        if (isCrouch)
        {
            moveSpeed = 3f;
        }

        else
        {
            moveSpeed = 5f;
        }
    }
}
