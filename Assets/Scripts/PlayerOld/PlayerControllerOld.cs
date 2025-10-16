using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerControllerOld : MonoBehaviour
{
    // [SerializeField] private SO_Player _playerStats;
    [SerializeField] private GameObject _playerSprite;

    // private float _currentHealth = 0;
    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;

    private void Awake()
    {
        // _currentHealth = _playerStats.Health;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdatePlayerHpUi();
    }

    private void FixedUpdate()
    {
        // Sprite direction
        Vector2 direction = _rb.linearVelocity.normalized;
        if (direction.x < 0 && _playerSprite.transform.localScale.x > 0)
        {
            _playerSprite.transform.localScale = new Vector2(Mathf.Abs(_playerSprite.transform.localScale.x) * -1, _playerSprite.transform.localScale.y);
        }
        else if (direction.x > 0 && _playerSprite.transform.localScale.x < 0)
        {
            _playerSprite.transform.localScale = new Vector2(Mathf.Abs(_playerSprite.transform.localScale.x), _playerSprite.transform.localScale.y);
        }

        // Player movement
        // Vector2 targetVelocity = _moveInput * _playerStats.MoveSpeed;
        // float accelRate = _moveInput.magnitude > 0.1f ? _playerStats.Acceleration : _playerStats.Deceleration;

        // _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);
        // _rb.linearVelocity = _currentVelocity;

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
        // _currentHealth -= damage;
        // UpdatePlayerHpUi();
        // if (_currentHealth <= 0)
        // {
        //     // for now, reload the scene on death
        //     Debug.Log("player did, insert logic for ending game.");
        // }
    }

    private void UpdatePlayerHpUi()
    {
        // GameManager.Instance.UpdateHpUi(Mathf.CeilToInt(_currentHealth));
    }
    
    // public float GetPullRadius() { return _playerStats.GravitationalPullRadius; }
}
