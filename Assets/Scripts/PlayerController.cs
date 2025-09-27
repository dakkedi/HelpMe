using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float _health = 10f;

    [SerializeField] private float _gravitationalPullRadius = 5f;
    public float GetPullRadius() { return _gravitationalPullRadius; }


    private float _currentHealth = 0;

    public static PlayerController Instance { get; private set; }

    public event Action<int> PlayerDamaged;
    public void DoPlayerDamaged(int damage)
    {
        PlayerDamaged?.Invoke(damage);
    }

    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _currentHealth = _health;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdatePlayerHpUi();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = _moveInput * moveSpeed;
        float accelRate = _moveInput.magnitude > 0.1f ? acceleration : deceleration;

        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);
        _rb.linearVelocity = _currentVelocity;

        // Clamp player position to grid boundaries
        var gridManager = GridManager.Instance;
        Vector2 clampedPosition = _rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, gridManager.MinX, gridManager.MaxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, gridManager.MinY, gridManager.MaxY);
        _rb.position = clampedPosition;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UpdatePlayerHpUi();
        if (_currentHealth <= 0)
        {
            // for now, reload the scene on death
            Debug.Log("player did, insert logic for ending game.");
        }
    }

    private void UpdatePlayerHpUi()
    {
        GameManager.Instance.UpdateHpUi(Mathf.CeilToInt(_currentHealth)); 
    }
}
