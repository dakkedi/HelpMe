using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    public static PlayerController Instance { get; private set; }

    public event Action<int> PlayerDamaged;
    public void DoPlayerDamaged(int damage) {
        PlayerDamaged?.Invoke(damage);
    }

    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;
    private Vector2 _lastDirection;
 
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        if (_moveInput.magnitude > 0.1f) {
            _lastDirection = _moveInput;
        }

        Vector2 targetVelocity = _moveInput * moveSpeed;
        float accelRate = _moveInput.magnitude > 0.1f ? acceleration : deceleration;
        
        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);
        _rb.linearVelocity = _currentVelocity;
    }
}
