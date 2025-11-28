using System;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public static event Action<bool> OnPlayerCrouch;
    public static bool IsCrouch { get; private set; }

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnCrouch += HandleCrouch;
        }
    }

    private void HandleCrouch()
    {
        if (transform.localScale.y == 1f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            IsCrouch = true;
            OnPlayerCrouch?.Invoke(true);
        }

        else if (transform.localScale.y == 0.5f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            IsCrouch = false;
            OnPlayerCrouch?.Invoke(false);
        }
    }
}
