using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Tooltip("Invert vertical look (Y-axis).")]
    [SerializeField] private bool invertY = false;

    [Header("References")]
    [Tooltip("Transform of the player head, used for vertical rotation.")]
    [SerializeField] private Transform playerHead;

    [Header("Mouse Sensitivity")]
    [Range(0.1f, 1f)][SerializeField] private float mouseSensitivityX;
    [Range(0.1f, 1f)][SerializeField] private float mouseSensitivityY;

    private Vector2 _lookInput;
    private float _xRotation = 0f;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLook += HandleLook;
        }
    }

    private void LateUpdate()
    {
        float mouseX, mouseY;

        mouseX = _lookInput.x * mouseSensitivityX;
        mouseY = _lookInput.y * mouseSensitivityY;

        // Invert Y if needed and clamp vertical rotation between -90 and 90
        _xRotation -= invertY ? -mouseY : mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // Apply vertical rotation to camera (player head)
        if (playerHead != null)
            playerHead.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Apply horizontal rotation to player object
        transform.Rotate(transform.up * mouseX);
    }

    private void HandleLook(Vector2 vector2)
    {
        _lookInput = vector2;
    }
}
