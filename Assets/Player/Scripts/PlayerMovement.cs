using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementStats _playerMovementStats;
    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_playerMovementStats == null)
        {
            Debug.LogError("PlayerMovementStats on PlayerMovement component is null");
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = _moveInput * _playerMovementStats.MoveSpeed;
        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, _playerMovementStats.Acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = _currentVelocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        // Debug.Log("Move input: " + _moveInput.ToString());
    }
}
